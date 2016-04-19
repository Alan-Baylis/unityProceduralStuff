using UnityEngine;
using System.Collections;


public class turtleState {

	public Vector2 pos;
	public int angle;

	public turtleState(Vector2 givenPos, int givenAngle){

		pos = givenPos;
		angle = givenAngle;
	}
}
