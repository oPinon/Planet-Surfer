using UnityEngine;
using System.Collections;

public class GoalArea : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other) {

		Player player = other.GetComponent<Player>();
		if(player != null) {

			Debug.Log ("Done in " + player.GetTime());
			Application.LoadLevel ("Menu");
		}
	}
}
