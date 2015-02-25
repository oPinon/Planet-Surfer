using UnityEngine;
using System.Collections;

public class SpawnPlayer : MonoBehaviour {

    public GameObject Player;
	public GameObject GoalArea;
	public float SpawnHeight;

	// Use this for initialization
	void Start () {
	
		// Get the most centered planet
		Planet[] planets = GetComponentsInChildren<Planet>();
		float closestDist = float.PositiveInfinity;
		float farthestDist = 0.0f;
		Planet closestPlanet = null;
		Planet farthestPlanet = null;
		foreach(Planet p in planets) {
			float dist = (p.transform.position - this.transform.position).magnitude;
			if(dist < closestDist) {
				closestDist = dist;
				closestPlanet = p;
			}
			if(dist >= farthestDist) {
				farthestDist = dist;
				farthestPlanet = p;
			}
		}
		if(closestPlanet==null) { Debug.LogError("Couldn't find any planet to spawn the player on" ); return; }
		else { // spawn player
			Vector3 pos = closestPlanet.transform.position;
			pos.y += closestPlanet.baseRadius + closestPlanet.radiusDiff + SpawnHeight;
			Player.transform.position = pos;
		}
		if(farthestPlanet==null) { Debug.LogError("Couldn't find any planet to spawn the GoalArea on" ); return; }
		else { // spawn the Goal Area
			Vector3 pos = farthestPlanet.transform.position;
			pos.y -=farthestPlanet.baseRadius + farthestPlanet.radiusDiff;
			pos.z = GoalArea.transform.position.z;
			GoalArea.transform.position = pos;
		}
	}
}
