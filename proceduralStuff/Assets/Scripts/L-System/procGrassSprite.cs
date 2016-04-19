using UnityEngine;
using System.Collections;

public class procGrassSprite : MonoBehaviour {

	public int texWidth = 400;
	public int texHeight = 400;
	public int angle = 45;
	public int numGen = 3;
	public int dist = 4;
	public Color color;

	private Texture2D texture;
	private turtleTexture turtle;
	// Use this for initialization
	void Start () {
		Debug.Log(1);
		texture = new Texture2D(texWidth, texHeight, TextureFormat.RGBA32, false);
		turtle = new turtleTexture();
		turtle.setArgs(angle, numGen,dist);
		turtle.setTexture(texWidth,texHeight,texture, color);
		turtle.run();
		Texture2D outputTex = turtle.getTex();
		outputTex.Apply();
		GetComponent<Renderer>().material.mainTexture = outputTex;

	}
}
