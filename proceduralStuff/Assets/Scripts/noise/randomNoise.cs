using UnityEngine;
using System.Collections;

public class randomNoise : MonoBehaviour {

	public int number = 500;

	void Start () {

		Texture2D noiseTex;
		Color[] pix;
		Renderer rend;

		System.Random fixRandX;

		fixRandX = new System.Random( 0 );

		rend = GetComponent<Renderer>();
		noiseTex = new Texture2D(number, number);
		pix = new Color[noiseTex.width * noiseTex.height];
		rend.material.mainTexture = noiseTex;

		for(int x = 0; x<number; x+=1){

			for(int y = 0; y<number; y+=1){
				float sample = ((float)fixRandX.NextDouble() * 2.0f) - 1.0f;
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
