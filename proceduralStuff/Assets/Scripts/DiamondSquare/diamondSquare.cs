using UnityEngine;
using System.Collections;

//diamond square via
//https://github.com/hunterloftis/playfuljs-demos/blob/gh-pages/terrain/index.html

//mesh creation via
//http://catlikecoding.com/unity/tutorials/noise-derivatives/

public class diamondSquare  {

	public int detail;
	public int size;
	public int max;
	public float zScale = 0.1f;
	public float stepSize = 1f;
	public float roughness;
	public Color[] textureVals;
	public Texture2D mapTexture;
	private Mesh mesh;


	public  diamondSquare (int inDetail, float inRougness, float inZScale, float inStepSize) {

		detail = inDetail;
		roughness = inRougness;
		size = (int)Mathf.Pow(2,detail)+1;
		max = size + 1;
		textureVals = new Color[size*size];
		mapTexture = new Texture2D(size, size);

		zScale = inZScale;
		stepSize = inStepSize;

		for (int i=0; i<textureVals.Length;i++){
			textureVals[i] = Color.white;
		}

		mapTexture.SetPixels(textureVals);


	}

	public Texture2D generateTex(){

		//initialize and set corners
		setVal(0, 0, max );
		setVal(max, 0, max / 2);
		setVal(max, max, 0);
		setVal(0, max, max / 2);

		divide(max);
		mapTexture.Apply();

		return mapTexture;
	}

	public void divide(int size) {

		int half = (int)Mathf.Round(size / 2);

		float scale = roughness * size;

		if (half < 1){ 
			return;
		}
			
		for (int y = half; y < max; y += size) {
			for (int x = half; x < max; x += size) {
				float val = Random.value * scale * 2 - scale;
				square(x, y, half, val);
			}
		}

		for (int y = 0; y <= max; y += half) {
			for (int x = (y + half) % size; x <= max; x += size) {
				float val = Random.value * scale * 2 - scale;
				diamond(x, y, half, val);
			}
		}
		divide(size / 2);

	}

	public void setVal(int x, int y, float val){
		mapTexture.SetPixel(x,y,new Color(val,val,val));
	}

	public float getVal(int x, int y){
		return mapTexture.GetPixel(x,y).r;
	}

	public void square(int x, int y, int size, float offset){

		float valA = getVal(x - size, y - size);
		float valB = getVal(x + size, y - size);
		float valC = getVal(x + size, y + size);
		float valD = getVal(x - size, y + size);

		float[] values = new float[]{valA, valB, valC, valD};
		float avg = average(values);

		setVal(x,y,avg + offset);
	}

	public void diamond(int x, int y, int size, float offset){

		float valA = getVal(x, y - size);
		float valB = getVal(x + size, y);
		float valC = getVal(x, y + size);
		float valD = getVal(x - size, y);

		float[] values = new float[]{valA, valB, valC, valD};
		float avg = average(values);
		setVal(x,y,avg + offset);

	}

	public float average(float[] values) {

		float sum = 0;
		float validCount = 0;

		for(int i = 0; i<values.Length; i++){
			if(values[i] != -1){
				sum+=values[i];
				validCount++;
			}
		}

		return sum/validCount;
	}

	public Mesh generateMesh(){
		if (mesh == null) {
			mesh = new Mesh();
			mesh.name = "Surface Mesh";
		}

		mesh.Clear();
		Vector3[] vertices = new Vector3[(size + 1) * (size + 1)];
		Color[] colors = new Color[vertices.Length];
		Vector3[] normals = new Vector3[vertices.Length];
		Vector2[] uv = new Vector2[vertices.Length];
		for (int v = 0, y = 0; y <= size; y++) {
			for (int x = 0; x <= size; x++, v++) {
				float zVal = getVal(x,y) * zScale;
				vertices[v] = new Vector3(x * stepSize - (size*stepSize/2), y * stepSize - (size*stepSize/2), zVal);
				colors[v] = Color.black;
				normals[v] = Vector3.up;
				uv[v] = new Vector2(x * stepSize, y * stepSize);
			}
		}
		mesh.vertices = vertices;
		mesh.colors = colors;
//		mesh.normals = normals;

		mesh.uv = uv;

		int[] triangles = new int[size * size * 6];
		for (int t = 0, v = 0, y = 0; y < size; y++, v++) {
			for (int x = 0; x < size; x++, v++, t += 6) {
				triangles[t] = v;
				triangles[t + 1] = v + size + 1;
				triangles[t + 2] = v + 1;
				triangles[t + 3] = v + 1;
				triangles[t + 4] = v + size + 1;
				triangles[t + 5] = v + size + 2;
			}
		}
		mesh.triangles = triangles;
		mesh.RecalculateNormals();

		return mesh;

	}


}
