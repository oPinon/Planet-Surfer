using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public GameObject Planets;
	public float _gravityFactor = 2f;

	private Vector2 _gravity;
	private Planet[] _planets;

	// Use this for initialization
	void Start () {

		_planets = (Planet[]) Planets.GetComponentsInChildren<Planet>();
		Debug.Log ("Playing with " + _planets.Length + " planet(s)");

		computeGravity();
    }
	
	// Update is called once per frame
	void Update () {

		computeGravity();
		this.rigidbody2D.AddForce( ( Input.GetKey(KeyCode.Space) ? _gravityFactor : 1) *_gravity);
    }

	// TODO : gravity evolution law
	void computeGravity() {
		Vector2 newGravity = new Vector2();
		foreach(Planet p in _planets) {
			Vector3 posDiff = p.transform.position - this.transform.position;
			newGravity += new Vector2(posDiff.x, posDiff.y) * p.mass;
		}
		_gravity = newGravity;
	}

	public Vector2 getGravity() {
		return _gravity;
	}
}
