using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlanetScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GenerateSphere();
		renderer.material.mainTexture = 
		new Texture2D(400,
		              400,
		              TextureFormat.RGB24, false);

		tex = ((Texture2D)renderer.material.mainTexture);


		Color[] colors = new Color[400 * 400];
		for(int i = 0; i < colors.Length; i++)
		{
			colors[i] = Color.white;
		}

		tex.SetPixels(colors);
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

	public Color earthColor;
	public Color seaColor;

	List<int> seaVerts = new List<int>();

	//Probably could be a shader
	public void OffsetMeshByTexture()
	{
		Color[] pixels = tex.GetPixels ();
		Mesh m = GetComponent<MeshFilter>().mesh;

		Vector2[] uvs = m.uv;
		Vector3[] verts = m.vertices;
		Vector3[] normals = m.normals;

		for(int i = 0; i < uvs.Length; i++)
		{
			Color color = tex.GetPixelBilinear(uvs[i].x, uvs[i].y);

			if(ColorDif(color, earthColor) < .1f)
			{

				if(!seaVerts.Contains(i))
				{
					verts[i] += normals[i] * .02f;
					seaVerts.Add(i);
					//seaVerts.Remove(i);
				}
			}
			if(ColorDif(color, seaColor) < .1f)
			{
				if(!seaVerts.Contains(i))
				{
					//seaVerts.Add(i);
				}
			}
		//	verts[i] += normals[i] * color.g * .002f;
		}

		m.vertices = verts;
		m.RecalculateNormals();
	}

	public float ColorDif(Color color1, Color color2)
	{
		float val = ((Mathf.Abs(color1.r - color2.r)) + (Mathf.Abs(color1.g - color2.g)) + (Mathf.Abs(color1.b - color2.b))) / 3f;
		return val;
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
		if(Input.GetKeyDown(KeyCode.P))
		{
			OffsetMeshByTexture();
		}

		OffsetMeshByTexture();
		//UpdateSeaVerts();
	}

	float offset = 0;

	void UpdateSeaVerts()
	{
		Mesh mesh = GetComponent<MeshFilter>().mesh;
		Vector3 [] verts = mesh.vertices;
		Vector3 [] normals = mesh.normals;

		offset += Time.deltaTime;
		for(int y = 0; y < seaVerts.Count; y++)
		{
			verts[seaVerts[y]] += Mathf.Sin(offset) * normals[seaVerts[y]] * .0005f;
		}

		mesh.vertices = verts;
	}


	void GenerateSphere()
	{
		MeshFilter filter = gameObject.AddComponent< MeshFilter >();
		Mesh mesh = filter.mesh;
		mesh.Clear();
		
		float radius = 1f;
		// Longitude |||
		int nbLong = 80;
		// Latitude ---
		int nbLat = 60;
		
		#region Vertices
		Vector3[] vertices = new Vector3[(nbLong+1) * nbLat + 2];
		float _pi = Mathf.PI;
		float _2pi = _pi * 2f;
		
		vertices[0] = Vector3.up * radius;
		for( int lat = 0; lat < nbLat; lat++ )
		{
			float a1 = _pi * (float)(lat+1) / (nbLat+1);
			float sin1 = Mathf.Sin(a1);
			float cos1 = Mathf.Cos(a1);
			
			for( int lon = 0; lon <= nbLong; lon++ )
			{
				float a2 = _2pi * (float)(lon == nbLong ? 0 : lon) / nbLong;
				float sin2 = Mathf.Sin(a2);
				float cos2 = Mathf.Cos(a2);
				
				vertices[ lon + lat * (nbLong + 1) + 1] = new Vector3( sin1 * cos2, cos1, sin1 * sin2 ) * radius;
			}
		}
		vertices[vertices.Length-1] = Vector3.up * -radius;
		#endregion
		
		#region Normales		
		Vector3[] normales = new Vector3[vertices.Length];
		for( int n = 0; n < vertices.Length; n++ )
			normales[n] = vertices[n].normalized;
		#endregion
		
		#region UVs
		Vector2[] uvs = new Vector2[vertices.Length];
		uvs[0] = Vector2.up;
		uvs[uvs.Length-1] = Vector2.zero;
		for( int lat = 0; lat < nbLat; lat++ )
			for( int lon = 0; lon <= nbLong; lon++ )
				uvs[lon + lat * (nbLong + 1) + 1] = new Vector2( (float)lon / nbLong, 1f - (float)(lat+1) / (nbLat+1) );
		#endregion
		
		#region Triangles
		int nbFaces = vertices.Length;
		int nbTriangles = nbFaces * 2;
		int nbIndexes = nbTriangles * 3;
		int[] triangles = new int[ nbIndexes ];
		
		//Top Cap
		int i = 0;
		for( int lon = 0; lon < nbLong; lon++ )
		{
			triangles[i++] = lon+2;
			triangles[i++] = lon+1;
			triangles[i++] = 0;
		}
		
		//Middle
		for( int lat = 0; lat < nbLat - 1; lat++ )
		{
			for( int lon = 0; lon < nbLong; lon++ )
			{
				int current = lon + lat * (nbLong + 1) + 1;
				int next = current + nbLong + 1;
				
				triangles[i++] = current;
				triangles[i++] = current + 1;
				triangles[i++] = next + 1;
				
				triangles[i++] = current;
				triangles[i++] = next + 1;
				triangles[i++] = next;
			}
		}
		
		//Bottom Cap
		for( int lon = 0; lon < nbLong; lon++ )
		{
			triangles[i++] = vertices.Length - 1;
			triangles[i++] = vertices.Length - (lon+2) - 1;
			triangles[i++] = vertices.Length - (lon+1) - 1;
		}
		#endregion
		
		mesh.vertices = vertices;
		mesh.normals = normales;
		mesh.uv = uvs;
		mesh.triangles = triangles;
		
		mesh.RecalculateBounds();
		mesh.Optimize();

		GetComponent<MeshCollider>().sharedMesh = mesh;
	}
}
