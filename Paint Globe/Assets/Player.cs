using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public float planetRadius = 10f;
	public Vector3 planetCenter;
	public PlanetScript planet;

	public float speed = .5f;

	public Color color = Color.blue;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		RaycastHit hit;
		Vector3 gravityDir = (planetCenter - this.transform.position).normalized;

		transform.up = -gravityDir;


		if(Input.GetKey(KeyCode.D))
		{
			transform.position += Camera.main.transform.right * speed;
		}

		if(Input.GetKey(KeyCode.A))
		{
			transform.position -= Camera.main.transform.right * speed;
		}

		if(Input.GetKey(KeyCode.W))
		{
			transform.position += Camera.main.transform.up * speed;
		}

		if(Input.GetKey(KeyCode.S))
		{
			transform.position -= Camera.main.transform.up * speed;
		}

		//Debug.DrawLine(transform.position, transform.position + (planetCenter - this.transform.position), Color.red);

		if(Physics.Raycast(new Ray(this.transform.position - gravityDir * 1f, planetCenter - this.transform.position), out hit))
		{
			transform.position = hit.point + transform.up * .5f * renderer.bounds.size.y;
			planet.PaintAtUV(hit.textureCoord, color);
		}
	}
}
