using UnityEngine;
using System.Collections;

public class Brush : MonoBehaviour {

	public float persistence = .5f;
	public float scale = 1.0f;
	public int octaves = 8;
	public int dims = 25;
	public float cutoff = .5f;
	public float brushRadius = .5f;
	public Color strokeColor;
	public Color shadeColor;
	public Color transColor;
	public bool showBrush = false;
	noise myNoise;


	void Start () {


		Renderer rend;
		rend = GetComponent<Renderer>();

		Texture2D brush = createBrush(persistence,octaves,dims,cutoff,brushRadius);
		if(showBrush){
			rend.material.mainTexture = brush;
		} else {
			Texture2D newTex = paintTexture(brush);
			rend.material.mainTexture = newTex;
		}


	}

	public float map(float OldMin, float OldMax, float NewMin, float NewMax, float OldValue){

		float OldRange = (OldMax - OldMin);
		float NewRange = (NewMax - NewMin);
		float NewValue = (((OldValue - OldMin) * NewRange) / OldRange) + NewMin;

		return(NewValue);
	}

	public Texture2D createBrush(float persistance, int octaves, int dims, float cutoff, float brushRadius){

		noise myNoise = new noise(persistence,octaves,dims);

		Texture2D brushTex = new Texture2D(dims, dims);
		Color[] pix = new Color[brushTex.width * brushTex.height];
		Vector2 center = new Vector2(dims/2,dims/2);

		float sample = 1.0f;

		for(int x = 0; x<dims; x+=1){

			for(int y = 0; y<dims; y+=1){


				float xVal =  x  * scale;
				float yVal =  y  * scale;
				Vector2 point = new Vector2(x,y);

				float dist = Vector2.Distance(point,center);
				if(dist<brushRadius){

					sample = myNoise.perlinNoise2D(xVal,yVal);
					sample = map(-1.0f,1.0f, 0.0f, 1.0f,sample);
					if(sample>cutoff){
						transColor.a = 0.0f;
						pix[y * brushTex.width + x] = transColor;
					} else {
						Color newColor = shadeColor + strokeColor/dims * y;
						pix[y * brushTex.width + x] = newColor;
					}

				} else {
					transColor.a = 0.0f;
					pix[y * brushTex.width + x] = transColor;
				}




			}
		}
		brushTex.SetPixels(pix);
		brushTex.Apply();

		return brushTex;

	}

	public Texture2D paintTexture(Texture2D inputTex){

		Texture2D painting = new Texture2D(500, 500);

		Color[] fill =  new Color[painting.width*painting.height];

		for (int i=0; i<fill.Length;i++){
			fill[i] = transColor;
		}
		painting.SetPixels(fill);

		Texture2D oldPainting = painting;

		noise myNoise = new noise(.25f,3,100);
		for(int x = 0; x<500; x+=inputTex.width-10){

			float sample = myNoise.perlinNoise1D(x+1);
			int height = (int)map(-1.0f,1.0f, 0f, 400f,sample);
//			int height = (int)(1000 * sample);
			int heightStep = (int)Mathf.Round(inputTex.height*.3f);
			for(int i = 1; i<height; i+=heightStep){

				Color[] stampPix = inputTex.GetPixels();
				Color[] oldPix = oldPainting.GetPixels(x,i,inputTex.width,inputTex.height);
				Color[] newpix = new Color[stampPix.Length];

				for(int j = 0; j<stampPix.Length;j++){
	
					if(oldPix[j] == transColor){
						newpix[j] = stampPix[j];
					} else {
						newpix[j] = oldPix[j];
					}
						
				}

				painting.SetPixels(x,i,inputTex.width,inputTex.height, newpix);
			}
		}

		painting.Apply();

		//todo continue painting

		return painting;
	}


}
