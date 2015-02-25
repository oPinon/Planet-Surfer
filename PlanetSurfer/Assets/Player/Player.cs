using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public GameObject Planets; // Parent object of the planets that affect the player
	public float _gravityFactor = 2f; // multiplies the gravity when the controller (spacebar) is used
	public AnimationCurve gravityCurve = new AnimationCurve(); // evolution of gravity for the distance
	public float maxGravityDist = 1f; // ratio between gravity's max distance and planet's radius	public GameObject Score;

	private Vector2 _gravity;
	private Planet[] _planets;
	private Score _score;

	// Use this for initialization
	void Start () {

		_planets = (Planet[]) Planets.GetComponentsInChildren<Planet>();
		Debug.Log ("Playing with " + _planets.Length + " planet(s)");
		_score = Score.GetComponent<Score>();
		if(_score == null) { Debug.LogError("Not attached score in " + Score); }

		computeGravity();
    }
	
	// Update is called once per frame
	void Update () {

		computeGravity();
		this.rigidbody2D.AddForce( ( Input.GetKey(KeyCode.Space) ? _gravityFactor : 1) *_gravity);
    }

	void computeGravity() {
		Vector2 newGravity = new Vector2();
		foreach(Planet p in _planets) {
			Vector3 posDiff = p.transform.position - this.transform.position;
			float distance = posDiff.magnitude;
			float factor = p.mass * gravityCurve.Evaluate((distance-p.baseRadius)/(maxGravityDist*p.baseRadius));
			newGravity += new Vector2(posDiff.x, posDiff.y)/distance * factor;
		}
		_gravity = newGravity;
	}

	public Vector2 getGravity() {
		return _gravity;
	}

	public void incrementScore(int scoreIncrement) {
		_score.addScore(scoreIncrement);
	}
}
