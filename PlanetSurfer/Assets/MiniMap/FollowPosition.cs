using UnityEngine;
using System.Collections;

public class FollowPosition : MonoBehaviour {

	public GameObject ToFollow;
	private Vector3 _offset;

	// Use this for initialization
	void Start () {
		_offset = this.transform.position;
	}
	
	// Update is called once per frame
	void Update () {

		this.transform.position = ToFollow.transform.position + _offset;
	}
}
