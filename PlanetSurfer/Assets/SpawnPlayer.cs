using UnityEngine;
using System.Collections;

public class SpawnPlayer : MonoBehaviour {

    public GameObject Player;
	public float SpawnHeight;

	// Use this for initialization
	void Start () {
	
		// Get the most centered planet
		Planet[] planets = GetComponentsInChildren<Planet>();
		float closestDist = float.PositiveInfinity;
		Planet closestPlanet = null;
		foreach(Planet p in planets) {
			float dist = (p.transform.position - this.transform.position).magnitude;
			if(dist < closestDist) {
				closestDist = dist;
				closestPlanet = p;
			}
		}
		if(closestPlanet==null) { Debug.LogError("Couldn't find any planet to spawn on" ); return; }
		else { // spawn player
			Vector3 pos = closestPlanet.transform.position;
			pos.y += closestPlanet.baseRadius + closestPlanet.radiusDiff + SpawnHeight;
			Player.transform.position = pos;
		}
	}
}
