using UnityEngine;
using System.Collections;

public class VelocityArrow : MonoBehaviour {

	public float Angle = 0.0f;
	public float Speed = 0.5f;

	// Use this for initialization
	void Start () {
		this.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {

		Angle = (Angle + Speed*Time.deltaTime) % 360;
		this.transform.eulerAngles = new Vector3(0,0,Angle);
	}
}
