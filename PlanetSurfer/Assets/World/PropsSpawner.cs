using UnityEngine;
using System.Collections;

public class PropsSpawner : MonoBehaviour {

	public GameObject Props; // Objects to spawn
	public float Spacing = 0.1f; // minimum horizontal space between 2 objects
	public float Probability = 0.1f; // probability in [0;1] that an object spawns for each angle
	public float MinPhi = 0, MaxPhi = Mathf.PI;

	public void Spawn () {

		Planet planet = this.GetComponent<Planet>();
		if(planet==null) {
			Debug.LogError("Not attached to a planet" );
			return;
		}

		for(float phi = MinPhi; phi<MaxPhi; phi += Spacing/planet.baseRadius) {

			uint number = (uint) (planet.baseRadius * Mathf.Abs(Mathf.Sin (2*phi)) / Spacing);
			for(int i=0; i<number; i++) {
				if( Random.Range(0.0f,1.0f) > Probability ) { continue; }
				float theta = i*(2*Mathf.PI) / number;
				float r = planet.radius(theta,Mathf.PI/2);
				float x = r * Planet.xFromAngle(theta, phi);
				float y = r * Planet.yFromAngle(theta, phi);
				float z = r * Planet.zFromAngle(theta, phi);
				GameObject newObject = Instantiate(Props, transform.position, Props.transform.rotation) as GameObject;
				newObject.transform.position += new Vector3(x,y,z);
				newObject.transform.RotateAround(newObject.transform.position, Vector3.back, Mathf.Rad2Deg *theta - 90);
				//newObject.transform.RotateAround(newObject.transform.position, Vector3.right, Mathf.Rad2Deg * phi - 90);
            }
		}
	}
}
