using UnityEngine;
using System.Collections;

public class main : MonoBehaviour {

	public float persistence = .5f;
	public float scale = 1.0f;
	public int octaves = 8;
	public int dims = 500;
	noise myNoise;

	void Start () {

		noise myNoise = new noise(persistence,octaves,dims);

		//2D noise texture
		Texture2D noiseTex;
		Color[] pix;
		Renderer rend;

		rend = GetComponent<Renderer>();
		noiseTex = new Texture2D(dims, dims);
		pix = new Color[noiseTex.width * noiseTex.height];
		rend.material.mainTexture = noiseTex;

		for(int x = 0; x<dims; x+=1){

			for(int y = 0; y<dims; y+=1){
				float xVal =  x  * scale;
				float yVal =  y  * scale;
				float sample = myNoise.perlinNoise2D(xVal,yVal);
				sample = map(-1.0f,1.0f, 0.0f, 1.0f,sample);
				pix[y * noiseTex.width + x] = new Color(sample, sample, sample);
			}
		}
		noiseTex.SetPixels(pix);
		noiseTex.Apply();
	}

	public float map(float OldMin, float OldMax, float NewMin, float NewMax, float OldValue){

		float OldRange = (OldMax - OldMin);
		float NewRange = (NewMax - NewMin);
		float NewValue = (((OldValue - OldMin) * NewRange) / OldRange) + NewMin;

		return(NewValue);
	}
		

}
