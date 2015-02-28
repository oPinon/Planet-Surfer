using UnityEngine;
using System.Collections;

public class EnergyPickable : MonoBehaviour {

	public int EnergyIncrement = 20;

	void OnTriggerEnter2D(Collider2D other) {
		
		Player player = other.GetComponent<Player>();
		if(player != null) {
			player.incrementEnergy( EnergyIncrement );
			Destroy(this.gameObject);
		}
	}
}
