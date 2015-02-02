using UnityEngine;
using System.Collections;

public class Orbit : MonoBehaviour {

	public GameObject orbiting;
	public float speed = 2f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.RotateAround(orbiting.transform.position, Vector3.up, speed * Time.deltaTime);
	}
}
