using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Orbit : MonoBehaviour {

	public GameObject orbiting;
	public float speed = 2f;

	float baseDistance = 0f;

	// Use this for initialization
	void Start () {
		baseDistance = (orbiting.transform.position - transform.position).magnitude;
	}

	float total = 0f;
	bool end = false;

	public Text blueMarble; 
	public Text sandyPearl;

	public PlanetScript planet;

	int winner = -1;

	// Update is called once per frame
	void Update () {

		if(!end)
		{
			total += -speed * Time.deltaTime;
			transform.RotateAround(orbiting.transform.position, Vector3.up, -speed * Time.deltaTime);
		}

		if(end)
		{
			Vector3 current = (orbiting.transform.position - transform.position);
			Vector3 target = baseDistance * current.normalized * 2.5f;

			transform.position += (current - target) * .6f * Time.deltaTime;

			if(winner == -1)
			{
				winner = planet.FindWinner();
				if(winner != 1)
				{
					sandyPearl.gameObject.SetActive(true);
				}
				else
				{
					blueMarble.gameObject.SetActive(true);
				}
			}


			if(sandyPearl.color.a < 1f)
			{
				sandyPearl.color += new Color(0,0,0,1f) * .3f * Time.deltaTime;
			}

			if(blueMarble.color.a < 1f)
			{
				blueMarble.color += new Color(0,0,0,1f) * .3f * Time.deltaTime;
			}

		}

		if(Mathf.Abs(total) > 370 || Input.GetKeyDown(KeyCode.Q))
		{
			end = true;
		}

	}
}
