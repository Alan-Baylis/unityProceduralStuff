﻿using UnityEngine;
using System.Collections;

public class createTrees : MonoBehaviour {

	public Color treeColor;

	public bool isForest;

	// Use this for initialization
	void Start () {
	
		if(isForest){
			forest();
		} else {
			singleTree();
		}


	}
	
	void singleTree(){



		tree newTree = new tree(16,2f,.9f,.01f,.8f,.8f,.8f,.08f,.5f);

		RaycastHit hit;

		Vector3 position = new Vector3(0,-5,5);
		float yPos;
		if (Physics.Raycast(position, -Vector3.up, out hit, 100.0f)){
			Debug.Log( hit.point.y);
			yPos =  hit.point.y;
		} else {
			yPos = 0.0f;
		}
			
		Vector3 newPos = new Vector3(position.x, yPos, position.z);

		GameObject createdTree = newTree.generate(position, treeColor);
		createdTree.transform.position = newPos;
		createdTree.name = "GeneratedTree";
	}

	void forest(){

		for(int i = 0; i<20; i++){

			int ns = (int)Random.Range(10f, 16f);
			float br = Random.Range(.5f, 4f);
			float rs = Random.Range(.8f, .9f);
			float mr = Random.Range(.01f, .09f);
			float bro = Random.Range(.5f, .9f);
			float sl = Random.Range(.5f, .9f);
			float t = Random.Range(.5f, .9f);
			float bp = Random.Range(.05f, .09f);
			float s = Random.Range(.3f, .9f);

			tree newTree = new tree(ns,br,rs,mr,bro,sl,t,bp,s);

			Vector3 newPos = new Vector3(Random.Range(-20f,20f),0, Random.Range(-20f,20f));

			GameObject createdTree = newTree.generate(Vector3.zero, treeColor);
			createdTree.transform.position = newPos;
			createdTree.name = "GeneratedTree";

		}

	}
}
