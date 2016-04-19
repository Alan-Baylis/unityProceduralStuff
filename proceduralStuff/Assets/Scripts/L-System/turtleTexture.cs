using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class turtleTexture : turtle {


	public int texWidth;
	public int texHeight;
	public Texture2D texture;
	public Color color;

	public turtleTexture()
		: base()
	{


	}
		
	public void setTexture( int inTexWidth, int inTexHeight, Texture2D inTex, Color inColor){
		texWidth = inTexWidth;
		texHeight = inTexHeight;
		currentPosx = texWidth/2;
		currentPosy = 0;
		texture = inTex;
		color = inColor;

	}
		
	public override void run(){

		Color alpha = Color.clear;

		Color[] fillPixels = new Color[texWidth * texHeight];

		for (int i = 0; i < fillPixels.Length; i++)
		{
			fillPixels[i] = alpha;
		}

		texture.SetPixels(fillPixels);


		for(int i = 0; i<numGen; i++){

			axiom = processRule(axiom);

		}
		processSteps(axiom);

	}

	public override void moveFoward(){
		
		if(currentPosy<texHeight){
			Vector2 addVec = Vector2FromAngle(currentAngle);

			currentPosx += Mathf.RoundToInt(addVec.x);
			currentPosy += Mathf.RoundToInt(addVec.y);
		}
		draw();
	}


	public override void push(){

		turtleState newState = new turtleState(new Vector2(currentPosx,currentPosy), currentAngle);

		turtleStates.Add(newState);
	}

	public override void pop(){

		if(turtleStates.Count > 0){

			turtleState newState = turtleStates[turtleStates.Count-1];
			turtleStates.RemoveAt(turtleStates.Count-1);

			Vector2 newPos = newState.pos;
			currentAngle = newState.angle;

			currentPosx = (int)newPos.x;
			currentPosy = (int)newPos.y;
		} else {
			currentPosx = texWidth/2;
			currentPosy = 0;
		}
	}

	void draw(){


		if(currentPosy>1){
			if(currentAngle == 90){
				texture.SetPixel(currentPosx-1, currentPosy, color*.8f);
			} else if (currentAngle == 45){
				texture.SetPixel(currentPosx-1, currentPosy-1, color*.8f);
			} else if (currentAngle == 135){
				texture.SetPixel(currentPosx-1, currentPosy+1, color*.8f);
			} else if (currentAngle == 0 || currentAngle == 180){
				texture.SetPixel(currentPosx, currentPosy-1, color*.8f);
			}

		}
		texture.SetPixel(currentPosx, currentPosy, color);

		if(currentPosy<texHeight-1){

			if(currentAngle == 90){
				texture.SetPixel(currentPosx+1, currentPosy, color*.8f);
			} else if (currentAngle == 45){
				texture.SetPixel(currentPosx+1, currentPosy+1, color*.8f);
			} else if (currentAngle == 135) {
				texture.SetPixel(currentPosx+1, currentPosy-1, color*.8f);
			} else if (currentAngle == 0 || currentAngle == 180){
				texture.SetPixel(currentPosx, currentPosy-1, color*.8f);
			}

		}
	}

	public Vector2 Vector2FromAngle(float a)
	{
		float rad = a *Mathf.Deg2Rad;
		Vector2 newVec = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
		return newVec;

	}

	public Texture2D getTex(){
		
		return texture;
	}
}
