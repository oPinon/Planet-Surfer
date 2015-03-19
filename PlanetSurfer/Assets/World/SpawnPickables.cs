using UnityEngine;
using System.Collections;

public class SpawnPickables : MonoBehaviour {

	public GameObject Pickable; // Object to spawn
	public float Height = 0.07f; // Height at which it is spawned from ground
	public float Spacing = 0.1f; // minimum horizontal space between 2 objects
	public float Probability = 0.1f; // probability in [0;1] that an object spawns for each angle

	public void Spawn () {
		Planet planet = this.GetComponent<Planet>();
		if(planet==null) {
			Debug.LogError("Not attached to a planet" );
			return;
		}
		int number = (int) (planet.baseRadius / Spacing);
		bool[] spawns = new bool[number];
		for(int i=0; i<spawns.Length; i++) {
			spawns[i] = Random.Range(0.0f,1.0f) < Probability;
		}

		for(int i=0; i<spawns.Length; i++) {
			if(spawns[i]) {
				float theta = i*(2*Mathf.PI) / spawns.Length;
				float r = planet.radius(theta,Mathf.PI/2) + Height;
				float x = r*Planet.xFromAngle(theta);
				float y = r*Planet.yFromAngle(theta);
				GameObject newObject = Instantiate(Pickable, transform.position, transform.rotation) as GameObject;
				newObject.transform.position += new Vector3(x,y,0);
			}
		}
	}
}
