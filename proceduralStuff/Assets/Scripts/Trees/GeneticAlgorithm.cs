using UnityEngine;
using System.Collections;

public class GeneticAlgorithm : MonoBehaviour {

	public int populationSize = 20;
	// Use this for initialization
	void Start () {
	
		forest();
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	void forest(){

		for(int i = 0; i<populationSize; i++){

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

			Color treeColor = new Color(Random.value, Random.value, Random.value);

			GameObject createdTree = newTree.generate(Vector3.zero, treeColor);
			createdTree.transform.position = newPos;
			createdTree.name = "GeneratedTree";

		}

	}

}
