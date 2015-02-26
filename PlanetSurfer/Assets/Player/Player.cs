using UnityEngine;
using System.Collections;

enum PlayerState { Playing, Rotating }

public class Player : MonoBehaviour {

	public GameObject Planets; // Parent object of the planets that affect the player
	public float _gravityFactor = 2f; // multiplies the gravity when the controller (spacebar) is used
	public AnimationCurve gravityCurve = new AnimationCurve(); // evolution of gravity for the distance
	public float maxGravityDist = 1f; // ratio between gravity's max distance and planet's radius	public GameObject Score;
	public GameObject VelocityArrow;

	private Vector2 _gravity;
	private Planet[] _planets;
	private Score _score;
	private PlayerState _state = PlayerState.Playing;
	private Vector2 _lastVelocity;
	private VelocityArrow _velocityArrow;

	// Use this for initialization
	void Start () {

		_planets = (Planet[]) Planets.GetComponentsInChildren<Planet>();
		Debug.Log ("Playing with " + _planets.Length + " planet(s)");
		_score = Score.GetComponent<Score>();
		if(_score == null) { Debug.LogError("No attached score in " + Score); }
		_velocityArrow = VelocityArrow.GetComponent<VelocityArrow>();
		if(_velocityArrow == null) { Debug.LogError("Not attached velocity arrow in " + VelocityArrow); }

		computeGravity();
    }
	
	// Update is called once per frame
	void Update () {

		if(_state == PlayerState.Playing) {

			computeGravity();
			bool gravityPressed = Input.GetKey(KeyCode.Space) || Input.touchCount > 0;
			this.rigidbody2D.AddForce( ( gravityPressed ? _gravityFactor : 1) *_gravity);
			if(Input.GetKey(KeyCode.KeypadEnter) || Input.touchCount > 1) { startRotating(); }

		} else if (_state == PlayerState.Rotating) {

			if(Input.GetKey(KeyCode.Space)) {
				_velocityArrow.gameObject.SetActive(false);
				this.rigidbody2D.WakeUp();
				float angle = _velocityArrow.Angle * 2 *Mathf.PI / 360.0f ;
				Vector2 newVelocity = _lastVelocity.magnitude * new Vector2( Mathf.Cos (angle), Mathf.Sin(angle));
				this.rigidbody2D.velocity = newVelocity;
				_state = PlayerState.Playing;
			}

		}
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

	private void startRotating() {
		_state = PlayerState.Rotating;
		_lastVelocity = this.rigidbody2D.velocity;
		this.rigidbody2D.Sleep();
		_velocityArrow.gameObject.SetActive(true);
	}

	public Vector2 getGravity() {
		return _gravity;
	}

	public void incrementScore(int scoreIncrement) {
		_score.addScore(scoreIncrement);
	}
}
