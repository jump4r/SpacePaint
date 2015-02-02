using UnityEngine;
using System.Collections;

public class PlanetScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		renderer.material.mainTexture = 
		new Texture2D(400,
		              400,
		              TextureFormat.RGB24, false);

		tex = ((Texture2D)renderer.material.mainTexture);
	}

	void PaintAtPoint(int x, int y, Color color)
	{
		Texture2D tex = ((Texture2D)renderer.material.mainTexture);
		tex.SetPixel(x, y, color);
		tex.Apply();

	}

	int radius = 3;

	Texture2D tex;
	bool edited = false;

	public void PaintAtUV(Vector2 uv , Color color)
	{



		int xCenter = (int)(uv.x * tex.width);
		int yCenter = (int)(uv.y * tex.height);

		for(int i = -radius; i < radius; i++)
		{
			for(int j = -radius; j < radius; j++)
			{
				if(Mathf.Sqrt(i*i+j*j) < radius)
				{
					tex.SetPixel((int)(uv.x * tex.width) + i, (int)(uv.y * tex.height) + j, color);
				}
			}
		}

		edited = true;
		//tex.SetPixel((int)(uv.x * tex.width), (int)(uv.y * tex.height), Color.red);
	}

	public void LateUpdate () 
	{
		if(edited)
		{
			tex.Apply();
		}
		edited = false;
	}

	// Update is called once per frame
	void Update () {
	}
}
