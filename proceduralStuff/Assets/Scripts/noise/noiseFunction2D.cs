using System;
using System.Threading;
using UnityEngine;

public class noiseFunction2D{

	float[] values = new float[3];
	System.Random fixRandX;

	public noiseFunction2D(int seed){

		fixRandX = new System.Random( seed );
		values[0] = ((float)fixRandX.NextDouble() * 100.0f);
		values[1] = ((float)fixRandX.NextDouble() * 100.0f);
		values[2] = ((float)fixRandX.NextDouble() * 10000.0f);

	}

	public float get(int x, int y){

		float val = Mathf.Sin(Vector2.Dot(new Vector2(x,y),new Vector2(values[0],values[1])))* values[2];
		int intVal = (int)val;
		float fractVal = val - intVal;

		return fractVal;
	}




}
