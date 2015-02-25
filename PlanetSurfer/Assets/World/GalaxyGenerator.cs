using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GalaxyGenerator : MonoBehaviour {

	public GameObject PlanetAsset;
	public int Number;
	public float MinRadius, MaxRadius;
	public float MinDist;

	// Use this for initialization
	void Awake () {

		List<Planet> planets;
		planets = new List<Planet>();
		float xRange = Mathf.Sqrt(Number)*(MinRadius+MaxRadius);

		if(MinRadius<=0) { Debug.LogError("Min radius must be > 0 for " + this.name); return; }

		int i = 0;
		int attempts = 0;
		float r = Random.Range(MinRadius,MaxRadius);
		while( i < Number ) {
			attempts++;
			float x = Random.Range(-xRange,xRange);
			float y = Random.Range(-xRange,xRange);
			bool spawnable = true;
			foreach(Planet p in planets) {
				float actualDist2 = (new Vector3(x,y,0)-p.transform.position).sqrMagnitude;
				float dist = r + MinDist + p.baseRadius + p.radiusDiff;
				if(actualDist2<dist*dist) { spawnable = false; }
			}
			if(!spawnable) { continue; }
			else {
				Planet script = createPlanet(x,y,r);
				planets.Add(script);
				r = Random.Range(MinRadius,MaxRadius);
				i++;
			}
		}
		Debug.Log ((100.0f*Number/attempts)+"% successful spawns");
	}

	Planet createPlanet(float x, float y, float r) {

		GameObject newPlanet = Instantiate(PlanetAsset) as GameObject;
		newPlanet.transform.parent = this.transform;
		newPlanet.transform.position = new Vector3(x,y,0);
		Planet script  = newPlanet.GetComponent<Planet>();
		if(script==null) { Debug.LogError("PlanetAsset "+PlanetAsset.name+" has no planet generator script" ); return null; }
		script.baseRadius = r;
		return script;
	}
}
