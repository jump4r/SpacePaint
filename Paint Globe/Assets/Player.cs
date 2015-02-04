using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public float planetRadius = 10f;
	public PlanetScript planet;

	public float speed = .5f;

	public Color color = Color.blue;

	public float acceleration = .3f;
	public float maxSpeed = .8f;
	public float friction = .3f;

	private int numPixelsPainted = 0;

	Vector3 velocity;

	// Use this for initialization
	void Start () {
	
	}

	public KeyCode keyUp;
	public KeyCode keyRight;
	public KeyCode keyLeft; 
	public KeyCode keyDown;

	public bool isSea = false;

	// Update is called once per frame
	void Update () {

		RaycastHit hit;
		Vector3 gravityDir = (planet.transform.position - this.transform.position).normalized;

		transform.up = -gravityDir;

		//Movement code

		Vector3 dir = new Vector3();

		if(Input.GetKey(keyRight))
		{
			dir += Camera.main.transform.right;
			//transform.position += Camera.main.transform.right * speed;
		}

		if(Input.GetKey(keyLeft))
		{
			dir -= Camera.main.transform.right;
			//transform.position -= Camera.main.transform.right * speed;
		}

		if(Input.GetKey(keyUp))
		{
			dir += Camera.main.transform.up;
			//transform.position += Camera.main.transform.up * speed;
		}

		if(Input.GetKey(keyDown))
		{
			dir -= Camera.main.transform.up;

			//transform.position -= Camera.main.transform.up * speed;
		}

		if (Input.GetKey (keyUp) || Input.GetKey (keyDown) || Input.GetKey (keyRight) || Input.GetKey (keyLeft))
		{
			if (!audio.isPlaying) 
			{
				audio.Play ();
			}
		}

		if (Input.GetKeyUp (keyUp) || Input.GetKeyUp (keyDown) || Input.GetKeyUp (keyRight) || Input.GetKeyUp (keyLeft))
		{
			audio.Pause ();
		}

		if(dir.magnitude == 0)
		{
			velocity -= velocity * friction;
		}

		velocity += dir.normalized * acceleration * Time.deltaTime * 60f;

		if(velocity.magnitude > maxSpeed)
		{
			velocity = velocity.normalized * maxSpeed;
		}

		transform.position += velocity * Time.deltaTime * 60f;
		//Debug.DrawLine(transform.position, transform.position + (planetCenter - this.transform.position), Color.red);

		if(Physics.Raycast(new Ray(this.transform.position - gravityDir * 1f, planet.transform.position - this.transform.position), out hit))
		{
			transform.position = hit.point + transform.up * .7f * renderer.bounds.size.y;
			planet.PaintAtUV(hit.textureCoord, color);
		}

		transform.LookAt(Camera.main.transform.position, -Vector3.up); 
	}
}
