using System;
using System.Threading;
using UnityEngine;

public class noiseFunction1D {

	float[] values;
	int size = 100000;

	public noiseFunction1D(int seed){

		System.Random fixRand = new System.Random( seed );
		values = new float[size];
		for(int i = 0; i<size; i++){
			values[i] = ((float)fixRand.NextDouble() * 2.0f) - 1.0f;
		}

	}

	public float get(int x){

		return values[x];
	}

}
