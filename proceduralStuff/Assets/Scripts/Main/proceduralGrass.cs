using UnityEngine;
using System.Collections;

public class proceduralGrass : MonoBehaviour {

	public float persistence = .5f;
	public float scale = 1.0f;
	public int octaves = 8;
	public int dims = 500;
	public float cutOff = 0.5f;
	public float randDisplace = .1f;
	public float density = 1.0f;
	public float scaleMod = 4.0f;
	public float grashHeightMod = .6f;
	public GameObject grass;
	noise myNoise;



	void Start () {
		noise myNoise = new noise(persistence,octaves,dims);


		int totalMeshCount = 0;
		for(float x = 0; x<dims; x+=density){

			for(float y = 0; y<dims; y+=density){
				float xVal =  x  * scale;
				float yVal =  y  * scale;
				float sample = myNoise.perlinNoise2D(xVal,yVal);
				float mappedSample = map(-1.0f,1.0f, 0.0f, 1.0f,sample);
				float scaleSample = map(-1.0f,1.0f, 0.0f, scaleMod,sample);
				if(mappedSample>cutOff){
					float curScale = grass.transform.localScale.x;
					float newScale = Mathf.Pow(scaleSample,2) + (mappedSample - cutOff)*10.0f;
					float randomX = Random.Range(-randDisplace, randDisplace);
					float randomY = Random.Range(-randDisplace, randDisplace);

					float yPos = getHeight(new Vector3(randomX,4.0f,randomY)) + grashHeightMod;

					GameObject newGrass = Instantiate(grass, new Vector3(x+randomX-dims/2,yPos,y+randomY-dims/2),grass.transform.rotation) as GameObject;
					float randRot = Random.Range(0, 360);
					Vector3 newRot = new Vector3(0,randRot,0);
					newGrass.transform.Rotate(newRot);
					newGrass.transform.parent = transform;
					newGrass.transform.localScale = new Vector3(newScale/5f,newScale,1.0f);

					totalMeshCount++;
				}
				
			}
		}

		int numMeshes = (totalMeshCount*4/60000) + 1;


		MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
		int currentMesh = 0;

		Material material = GetComponent<MeshRenderer>().sharedMaterial;
		Texture tex = GetComponent<MeshRenderer>().sharedMaterial.mainTexture;
		for(int i =0; i<numMeshes; i++){

			GameObject newGameObject = new GameObject();

			CombineInstance[] combine = new CombineInstance[meshFilters.Length/numMeshes];
			int j = 0;
			while (j < combine.Length) {

				combine[j].mesh = meshFilters[currentMesh].sharedMesh;
				combine[j].transform = meshFilters[currentMesh].transform.localToWorldMatrix;
				meshFilters[j].gameObject.active = false;
				j++;
				currentMesh++;
			}


			newGameObject.AddComponent<MeshFilter>();
			var meshRenderer = newGameObject.AddComponent<MeshRenderer>();
			meshRenderer.sharedMaterial = material;
			meshRenderer.sharedMaterial.mainTexture = tex;
			newGameObject.GetComponent<MeshFilter>().sharedMesh = new Mesh();
			newGameObject.GetComponent<MeshFilter>().sharedMesh.CombineMeshes(combine);
			newGameObject.isStatic = true;
			newGameObject.SetActive(true);

			newGameObject.name = "GrassChunk";

		}



	}

	public float map(float OldMin, float OldMax, float NewMin, float NewMax, float OldValue){

		float OldRange = (OldMax - OldMin);
		float NewRange = (NewMax - NewMin);
		float NewValue = (((OldValue - OldMin) * NewRange) / OldRange) + NewMin;

		return(NewValue);
	}

	public float getHeight(Vector3 position){

		RaycastHit hit;

		if (Physics.Raycast(position, -Vector3.up, out hit, 100.0f)){
			return hit.point.y;
		} else {
			return 0.0f;
		}

	}
}
