using System;
using System.Threading;
using UnityEngine;


public class noise {

	float persistence;
	int octaves;
	noiseFunction1D[] noiseFunctions1D;
	noiseFunction2D[] noiseFunctions2D;
	int dims;

	public  noise (float p, int o, int suppliedDims) {

		persistence = p;
		octaves = o;
		dims = suppliedDims;
		noiseFunctions1D = new noiseFunction1D[octaves];
		noiseFunctions2D = new noiseFunction2D[octaves];
		createNoiseFunctions1D();
		createNoiseFunctions2D();
	}

	//interpolation function used for both 2D and 3D noise
	public float cosInterpolate(float min, float max,float x){

		float ft = x * 3.1415927f;
		float f = (1.0f - (float)Math.Cos(ft)) * 0.5f;

		return  min*(1-f) + max*f;

	}
		
	//functions required to 1D perlin noise
	public void createNoiseFunctions1D(){

		for(int i = 0; i<octaves; i++){

			noiseFunctions1D[i] = new noiseFunction1D(i);
		}

	}

	public float smoothedNoise1D(int x,int octave){


		return noiseFunctions1D[octave].get(x)/2  +  noiseFunctions1D[octave].get(x-1)/4  +  noiseFunctions1D[octave].get(x+1)/4;
	}
		

	public float interpolatedNoise1D(float x, int octave){

		int intX = (int)x;
		float fractX = x - intX;

		float v1 = smoothedNoise1D(intX, octave);
		float v2 = smoothedNoise1D(intX + 1,octave);

		return cosInterpolate(v1 , v2 , fractX);

	}

	public float perlinNoise1D(float x){

		float total = 0;
		float p = persistence;
		int n = octaves - 1;

		for(int i = 0; i<n; i++){

			float frequency = Mathf.Pow(2,i);
			float amplitude = Mathf.Pow(p,i);

			total = total + interpolatedNoise1D(x * frequency, i) * amplitude;
		}


		return total;

	}


	//functions required to 2D perlin noise
	public void createNoiseFunctions2D(){

		for(int i = 0; i<octaves; i++){

			noiseFunctions2D[i] = new noiseFunction2D(i);
		}

	}

	public float smoothedNoise2D(int x, int y, int octave){


		float corners = ( noiseFunctions2D[octave].get(x-1, y-1)+noiseFunctions2D[octave].get(x+1, y-1)+noiseFunctions2D[octave].get(x-1, y+1)+noiseFunctions2D[octave].get(x+1, y+1) ) / 16;
		float sides   = ( noiseFunctions2D[octave].get(x-1, y)  +noiseFunctions2D[octave].get(x+1, y)  +noiseFunctions2D[octave].get(x, y-1)  +noiseFunctions2D[octave].get(x, y+1) ) /  8;
		float center  =  noiseFunctions2D[octave].get(x, y) / 4;
		return corners + sides + center;

	}

	public float interpolatedNoise2D(float x, float y, int octave){

		int intX = (int)x;
		float fractX = x - intX;

		int intY = (int)y;
		float fractY = y - intY;

		float v1 = smoothedNoise2D(intX, intY, octave);
		float v2 = smoothedNoise2D(intX + 1,intY, octave);
		float v3 = smoothedNoise2D(intX, intY + 1, octave);
		float v4 = smoothedNoise2D(intX + 1, intY + 1, octave);

		float i1 = cosInterpolate(v1 , v2 , fractX);
		float i2 = cosInterpolate(v3 , v4 , fractX);

		return cosInterpolate(i1 , i2 , fractY);

	}

	public float perlinNoise2D(float x, float y){

		float total = 0;
		float p = persistence;
		int n = octaves - 1;

		for(int i = 0; i<n; i++){

			float frequency = Mathf.Pow(2,i);
			float amplitude = Mathf.Pow(p,i);

			total = total + interpolatedNoise2D(x * frequency, y * frequency, n) * amplitude;
		}
		return total;

	}


		
}
