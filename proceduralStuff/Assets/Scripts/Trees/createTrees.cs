using UnityEngine;
using System.Collections;

public class createTrees : MonoBehaviour {

	public Color treeColor;
	// Use this for initialization
	void Start () {
	
		tree newTree = new tree();

		RaycastHit hit;

		Vector3 position = new Vector3(-2,0,3);
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
	
	// Update is called once per frame
	void Update () {
	
	}
}
