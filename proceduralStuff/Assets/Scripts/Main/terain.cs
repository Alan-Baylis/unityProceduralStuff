using UnityEngine;
using System.Collections;

public class terain : MonoBehaviour {

	public int detail = 9;
	public float roughness = 0.01f;
	public float zScale = .1f;
	public float stepSize = 1f;
	public bool showTex;

	void Start () {

		Renderer rend;
		rend = GetComponent<Renderer>();
		diamondSquare terrainTex = new diamondSquare(detail, roughness, zScale, stepSize);
		Texture2D tex = terrainTex.generateTex();

		if(showTex){
			
			rend.material.mainTexture = tex;

		} else {

			Mesh newMesh = terrainTex.generateMesh();
			GetComponent<MeshFilter>().mesh = newMesh;
			GetComponent<MeshCollider>().sharedMesh = newMesh;

		}
			
	}

}
