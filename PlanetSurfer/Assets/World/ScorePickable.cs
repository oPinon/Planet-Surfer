using UnityEngine;
using System.Collections;

public class ScorePickable : MonoBehaviour {

	public int ScoreIncrement = 10;

	void OnTriggerEnter2D(Collider2D other) {
		
		Player player = other.GetComponent<Player>();
		if(player != null) {
			player.incrementScore( ScoreIncrement );
			Destroy(this.gameObject);
		}
	}
}
