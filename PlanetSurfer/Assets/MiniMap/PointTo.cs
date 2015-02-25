using UnityEngine;
using System.Collections;

public class PointTo : MonoBehaviour {

	public GameObject Target;
	
	// Update is called once per frame
	void Update () {

		Vector3 posDiff = Target.transform.position - this.transform.position;
		Quaternion newRotation = new Quaternion();
		newRotation.SetLookRotation( Vector3.forward, posDiff );
		this.transform.rotation = newRotation;
	}
}
