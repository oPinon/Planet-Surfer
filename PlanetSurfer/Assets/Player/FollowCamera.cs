using UnityEngine;
using System.Collections;

public class FollowCamera : MonoBehaviour {

	public GameObject player;
	public AnimationCurve gravityCurve = new AnimationCurve(); // orthoSize function of gravity
	public float maxGravity, maxSize; // factors for the curve's x and y respectively

	private Player playerScript;
	private Camera _camera;
	private float _minSize;

	// Use this for initialization
	void Start () {

		Player script = player.GetComponent<Player>();
		if( !script ) { Debug.LogError( player.name + " has no script Player attached"); }
		else { this.playerScript = script; }

		_camera = this.GetComponent<Camera>();
		if( !_camera ) { Debug.LogError( this.name + " must be a Camera to use FollowCamera.cs" ); }
		_minSize = _camera.orthographicSize;
	}
	
	// Update is called once per frame
	void Update () {
	
		updatePosition( player.transform.position );
		updateRotation( playerScript.getGravity() );
		updateSize( playerScript.getGravity() );
	}

	void updatePosition( Vector3 pos ) {
		this.transform.position = new Vector3( pos.x, pos.y, this.transform.position.z );
	}

	void updateRotation( Vector2 gravity ) {
		Vector3 down = new Vector3(gravity.x, gravity.y);
		Quaternion rotation = new Quaternion();
		rotation.SetLookRotation( Vector3.forward, -down );
		this.transform.rotation = rotation;
	}

	void updateSize( Vector2 gravity ) {
		_camera.orthographicSize = _minSize + maxSize * gravityCurve.Evaluate( gravity.magnitude / maxGravity );
	}
}
