using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class tree {

	//via http://www.wasabimole.com/procedural-tree/how-to-generate-a-procedural-tree-in-unity-3d/

	int Seed; // Random seed on which the generation is based
	int MaxNumVertices = 65000; // Maximum number of vertices for the tree mesh
	int NumberOfSides = 16; // Number of sides for tree
	float BaseRadius = 2f; // Base radius in meters
	float RadiusStep = 0.9f; // Controls how quickly radius decreases
	float MinimumRadius = 0.02f; // Minimum radius for the tree's smallest branches
	float BranchRoundness = 0.8f; // Controls how round branches are
	float SegmentLength = 0.5f; // Length of branch segments
	float Twisting = 20f; // How much branches twist
	float BranchProbability = 0.1f; // Branch probability
	float scale = .5f;

	float[] ringShape;
	GameObject newTree;
	Vector3 treePos;
	Quaternion startRot;
	Color treeColor;

	List<Vector3> vertexList; // Vertex list
	List<Vector2> uvList; // UV list
	List<int> triangleList; // Triangle list


	public tree(){

		newTree = new GameObject();
		vertexList = new List<Vector3>();
		uvList = new List<Vector2>();
		triangleList = new List<int>();
		startRot = newTree.transform.rotation;

	}
	
	public GameObject generate(Vector3 position, Color newColor){

		treeColor = newColor;
		setTreeRingShape();
		treePos = position;
		branch(new Quaternion(), position, -1, BaseRadius, 0f);
		return setTreeMesh();
	}

	public void setTreeRingShape(){

		ringShape = new float[NumberOfSides + 1];
		var k = (1f - BranchRoundness) * 0.5f;
		// Randomize the vertex offsets, according to BranchRoundness
		for (var n = 0; n < NumberOfSides; n++){
			ringShape[n] = 1f - (Random.value - 0.5f) * k;
		}	
		ringShape[NumberOfSides] = ringShape[0];
	}

	public void branch(Quaternion quaternion, Vector3 position, int lastRingVertexIndex, float radius, float texCoordV){

		Vector3 offset = Vector3.zero;
		Vector2 texCoord = new Vector2(0f, texCoordV);
		float textureStepU = 1f / NumberOfSides;
		float angInc = 2f * Mathf.PI * textureStepU;
		float ang = 0f;

		for (var n = 0; n <= NumberOfSides; n++, ang += angInc) 
		{
			float r = ringShape[n] * radius;
			offset.x = r * Mathf.Cos(ang); // Get X, Z vertex offsets
			offset.z = r * Mathf.Sin(ang);
			vertexList.Add(position + quaternion * offset); // Add Vertex position
			uvList.Add(texCoord); // Add UV coord
			texCoord.x += textureStepU;
		}

		if (lastRingVertexIndex >= 0) // After first base ring is added ...
		{
			// Create new branch segment quads, between last two vertex rings
			for (var currentRingVertexIndex = vertexList.Count - NumberOfSides - 1; currentRingVertexIndex < vertexList.Count - 1; currentRingVertexIndex++, lastRingVertexIndex++) 
			{
				triangleList.Add(lastRingVertexIndex + 1); // Triangle A
				triangleList.Add(lastRingVertexIndex);
				triangleList.Add(currentRingVertexIndex);
				triangleList.Add(currentRingVertexIndex); // Triangle B
				triangleList.Add(currentRingVertexIndex + 1);
				triangleList.Add(lastRingVertexIndex + 1);
			}
		}

		// Do we end current branch?
		radius *= RadiusStep;
		if (radius < MinimumRadius || vertexList.Count + NumberOfSides >= MaxNumVertices) // End branch if reached minimum radius, or ran out of vertices
		{
			// Create a cap for ending the branch
			vertexList.Add(position); // Add central vertex
			uvList.Add(texCoord + Vector2.one); // Twist UVs to get rings effect
			for (var n = vertexList.Count - NumberOfSides - 2; n < vertexList.Count - 2; n++) // Add cap
			{
				triangleList.Add(n);
				triangleList.Add(vertexList.Count - 1);
				triangleList.Add(n + 1);
			}
			return; 
		}

		// Continue current branch (randomizing the angle)
		texCoordV += 0.0625f * (SegmentLength + SegmentLength / radius);
		position += quaternion * new Vector3(0f, SegmentLength, 0f);
		newTree.transform.rotation = quaternion; 
		var x = (Random.value - 0.5f) * Twisting;
		var z = (Random.value - 0.5f) * Twisting;
		newTree.transform.Rotate(x, 0f, z);
		lastRingVertexIndex = vertexList.Count - NumberOfSides - 1;
		branch(newTree.transform.rotation, position, lastRingVertexIndex, radius, texCoordV); // Next segment

		// Do we branch?
		if (vertexList.Count + NumberOfSides >= MaxNumVertices || Random.value > BranchProbability) return;

		// Yes, add a new branch
		newTree.transform.rotation = quaternion;
		x = Random.value * 70f - 35f;
		x += x > 0 ? 10f : -10f;
		z = Random.value * 70f - 35f;
		z += z > 0 ? 10f : -10f;
		newTree.transform.Rotate(x, 0f, z);
		branch(newTree.transform.rotation, position, lastRingVertexIndex, radius, texCoordV);



	}

	public GameObject setTreeMesh(){

		// Get mesh or create one
		Mesh mesh = new Mesh();

		// Assign vertex data
		mesh.vertices = vertexList.ToArray();
		mesh.uv = uvList.ToArray();
		mesh.triangles = triangleList.ToArray();

		// Update mesh
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
		mesh.Optimize(); // Do not call this if we are going to change the mesh dynamically!

		newTree.AddComponent<MeshFilter>();
		newTree.GetComponent<MeshFilter>().mesh = mesh;
		newTree.AddComponent<MeshRenderer>();
		newTree.GetComponent<MeshRenderer>().material = new Material(Shader.Find("Diffuse"));
		newTree.GetComponent<MeshRenderer>().material.color = treeColor;

		newTree.transform.rotation = startRot;
		newTree.transform.localScale = new Vector3(scale,scale,scale);
		return newTree;
	}
}
