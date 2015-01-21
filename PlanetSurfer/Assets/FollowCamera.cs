using UnityEngine;
using System.Collections;

public class FollowCamera : MonoBehaviour {

	public GameObject player;
	private Player playerScript;

	// Use this for initialization
	void Start () {
		Player script = player.GetComponent<Player>();
		if( !script ) { Debug.LogError( player.name + " has no script Player attached"); }
		else { this.playerScript = script; }
	}
	
	// Update is called once per frame
	void Update () {
	
		updatePosition( player.transform.position );
		updateRotation( playerScript.getGravity() );
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
}
