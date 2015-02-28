using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour {

	public float speed;

	private float _angle = 0;

	// Update is called once per frame
	void Update () {
	
		_angle += speed*Time.deltaTime;

		this.transform.eulerAngles = new Vector3(0,0,_angle);
	}
}
