using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	private float gameTimer = 100f;
	public Text timerText;

	private bool gameOver = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		string t = "Timer: " + gameTimer + " seconds";
		timerText.text = t;
		gameTimer -= Time.deltaTime;

		if (gameTimer < 0) {
			EndGame ();
			gameTimer = 100f;
		}
	}

	private void EndGame () {
		Debug.Log ("End Game");
		GameObject.Find ("Planet").GetComponent<PlanetScript> ().CalculateWinner ();
	}
}
