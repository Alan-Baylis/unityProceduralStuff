using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class turtle {

	public int angle;
	public int numGen;
	public int dist;
	public int currentAngle = 90;
	public int currentGen = 1;
	public int currentPosx;
	public int currentPosy;

	public string axiom = "f";

	public List<turtleState> turtleStates = new List<turtleState>();

	public turtle() {

	}

	public virtual void setArgs(int inAngle, int inNumGen, int inDist){
		angle = inAngle;
		numGen = inNumGen;
		dist = inDist;

	}

	public virtual void run(){
		
		for(int i = 0; i<numGen; i++){

			axiom = processRule(axiom);

		}

		processSteps(axiom);
	}


	public virtual void processSteps(string axiom){

		for(int i = 0; i<axiom.Length; i++){

			switch(axiom[i].ToString()){

				case "f":
					for(int j = 0; j<dist; j++){
						moveFoward();
					}
					break;

				case "+":
					moveLeft();
					break;

				case "-":
					moveRight();
					break;

				case "[":
					push();
					break;

				case "]":
					pop();
					break;


			}
		}
			
	}

	public virtual string processRule(string rules){

		string newString = "";

		for(int i = 0; i<rules.Length; i++){

			switch(rules[i].ToString()){

				case "f":
					newString += "f[+f]f[-f][f]";
					break;
				default:
					newString += rules[i].ToString();
					break;
		
			}
		}
			
		return newString;
			
	}

	public virtual void moveFoward(){
		
		Debug.Log(1);
	}


	public virtual void moveLeft(){
		
		currentAngle -= angle;

	}

	public virtual void moveRight(){
		
		currentAngle += angle;

	}

	public virtual void push(){

	}

	public virtual void pop(){

	}
}
