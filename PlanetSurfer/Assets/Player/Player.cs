using UnityEngine;
using System.Collections;

enum PlayerState { Playing, Rotating }

public class Player : MonoBehaviour {

	public GameObject Planets; // Parent object of the planets that affect the player
	public float _gravityFactor = 2f; // multiplies the gravity when the controller (spacebar) is used
	public AnimationCurve gravityCurve = new AnimationCurve(); // evolution of gravity for the distance
	public float maxGravityDist = 1f; // ratio between gravity's max distance and planet's radius
	public GameObject EnergyObject;
	public int EnergyCost = 50;
	public GameObject VelocityArrow;
	public GameObject TimerObject;

	private Vector2 _gravity;
	private Planet[] _planets;
	private Energy _energy;
	private PlayerState _state = PlayerState.Playing;
	private Vector2 _lastVelocity;
	private VelocityArrow _velocityArrow;
	private Timer _timer;

	// Use this for initialization
	void Start () {

		_planets = (Planet[]) Planets.GetComponentsInChildren<Planet>();
		Debug.Log ("Playing with " + _planets.Length + " planet(s)");

		_energy = EnergyObject.GetComponent<Energy>();
		if(_energy == null) { Debug.LogError("No attached score in " + EnergyObject.ToString()); }

		_velocityArrow = VelocityArrow.GetComponent<VelocityArrow>();
		if(_velocityArrow == null) { Debug.LogError("Not attached velocity arrow in " + VelocityArrow); }

		_timer = TimerObject.GetComponent<Timer>();
		if(_timer == null) { Debug.LogError("Not attached timer in " + TimerObject.ToString()); }

		computeGravity();
    }
	
	// Update is called once per frame
	void Update () {

		if(_state == PlayerState.Playing) {

			computeGravity();
			bool gravityPressed = Input.GetKey(KeyCode.Space) || Input.touchCount > 0;
			this.GetComponent<Rigidbody2D>().AddForce( ( gravityPressed ? _gravityFactor : 1) *_gravity);
			if(Input.GetKey(KeyCode.KeypadEnter) || Input.touchCount > 1) {
				if(_energy.getEnergy()-EnergyCost>=0) {
					startRotating();
					_energy.addEnergy(-EnergyCost);
				}
			}

		} else if (_state == PlayerState.Rotating) {

            if(Input.GetKey(KeyCode.Space) || Input.touchCount > 0 ) {
				_velocityArrow.gameObject.SetActive(false);
				this.GetComponent<Rigidbody2D>().WakeUp();
				float angle = _velocityArrow.Angle * 2 *Mathf.PI / 360.0f ;
				Vector2 newVelocity = _lastVelocity.magnitude * new Vector2( Mathf.Cos (angle), Mathf.Sin(angle));
				this.GetComponent<Rigidbody2D>().velocity = newVelocity;
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
		_lastVelocity = this.GetComponent<Rigidbody2D>().velocity;
		this.GetComponent<Rigidbody2D>().Sleep();
		_velocityArrow.gameObject.SetActive(true);
	}

	public Vector2 getGravity() {
		return _gravity;
	}

	public void incrementEnergy(int energyIncrement) {
		_energy.addEnergy(energyIncrement);
	}

	public float GetTime() { return _timer.GetTime(); }
}
