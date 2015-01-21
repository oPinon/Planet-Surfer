using UnityEngine;
using System.Collections;
using UnityEditor;

[ExecuteInEditMode]
public class Planet : MonoBehaviour {

	public float mass = 10.0f;
	public float baseRadius = 10.0f;
	public float radiusDiff = 1.0f;
	public int meshResolution = 128;
	public int colliderResolution = 256;

	public void computeGeometry() {

		// Debug.Log("Computing geometry of " + this.name);

		// Computing mesh
		MeshFilter meshFilter = this.GetComponent<MeshFilter>();
		if(!meshFilter) { Debug.LogError( "Couldn't find meshFilter in planet " + this.name ); }
		else { createUVSphere( meshFilter.sharedMesh, meshResolution, 24 ); }
	
		// Computing EdgeCollider
		EdgeCollider2D collider = this.GetComponent<EdgeCollider2D>();
		if(!collider) { Debug.LogError( "Couldn't find edgeCollider2D in planet " + this.name ); }
		else { computeEdgeCollider(collider, colliderResolution); }
	}

	void Start () {

		computeGeometry();
	}
	
	// Update is called once per frame
	void Update () {

		if (!Application.isPlaying) { computeGeometry(); }

    }

	/* TODO: errors in spherical coordinates formula
	 * Computes a 2Dcollider alongs the planet's surface
	 */
	void computeEdgeCollider(EdgeCollider2D collider, int nbPoints) {

		Vector2[] points = new Vector2[nbPoints+1];
		for(int i=0; i<nbPoints; i++) {
			float theta = Mathf.PI/2 + 2*Mathf.PI * (float) i / nbPoints;
			float x = Mathf.Cos(theta);
			float y = -Mathf.Sin(theta);
			points[i] = new Vector2(x,y) * radius(-theta,Mathf.PI/2);
		}
		points[nbPoints] = points[0];
		collider.points = points;
	}
    
    // TODO: a weird seam is visible on one side of the sphere
	/*
	 * @returns a UVSphere mesh with a variable radius
	 * (using the @radius function)
	 * Phi = Latitude in radians [-Pi/2;Pi/2]
	 * Theta = Longitude in radians [0;2Pi]
	 * code based on http://wiki.unity3d.com/index.php/ProceduralPrimitives
	 * the sphere upper part is toward -z (Vector3.back)
	 */
	void createUVSphere( Mesh mesh, int nbLong, int nbLat ) {

		Vector3[] vertices = new Vector3[(nbLong+1) * nbLat + 2];

		#region Vertices
		vertices[0] = Vector3.forward * radius(0, Mathf.PI/2); // upper pole
		for( int lat = 0; lat < nbLat; lat++ ) {
			float phi = Mathf.PI * (float) (lat+1) / (nbLat+1); // latitude

			for( int lon=0; lon<=nbLong; lon++) {
				float theta = 2*Mathf.PI * (float) (lon == nbLong ? 0 : lon) / nbLong; // longitude
				float x = Mathf.Sin (phi)*Mathf.Cos(theta);
				float y = -Mathf.Sin (phi)*Mathf.Sin (theta);
                float z = Mathf.Cos(phi);
				vertices[lon+lat*(nbLong+1)+1] = new Vector3(x,y,z)*radius(theta, phi);
			}
		}
		vertices[vertices.Length-1] = Vector3.back * radius (0, -Mathf.PI/2);
		#endregion

		#region UVs
		Vector2[] uvs = new Vector2[vertices.Length];
		uvs[0] = Vector2.up;
		uvs[uvs.Length-1] = Vector2.zero;
		for(int lat=0;lat<nbLat;lat++) {
			for( int lon=0; lon <= nbLong; lon++) {
				float x = (float) lon / nbLong;
				float y = 1f - (float)(lat+1)/(nbLat+1);
				uvs[lon+lat*(nbLong+1)+1] = new Vector2(x,y);
			}
		}
		#endregion

		#region Triangles
		int nbFaces = vertices.Length;
		int nbTriangles = nbFaces*2;
		int nbIndexes = nbTriangles*3;
		int[] triangles = new int[nbIndexes];

		// Top Cap
		int i = 0;
		for(int lon=0; lon<nbLong; lon++) {
			triangles[i++] = lon+2;
			triangles[i++] = lon+1;
			triangles[i++] = 0;
		}
		// Middle
		for(int lat=0; lat<nbLat-1;lat++) {
			for(int lon=0; lon<nbLong; lon++) {

				int current = lon + lat*(nbLong+1)+1;
				int next = current+nbLong+1;

				triangles[i++] = current;
				triangles[i++] = current+1;
				triangles[i++] = next+1;

				triangles[i++] = current;
				triangles[i++] = next+1;
				triangles[i++] = next;
			}
		}

		// Bottom cap
		for( int lon=0; lon<nbLong; lon++) {
			triangles[i++] = vertices.Length - 1;
			triangles[i++] = vertices.Length - (lon+2) - 1;
			triangles[i++] = vertices.Length - (lon+1) - 1;
		}
		#endregion

		mesh.vertices = vertices;
		mesh.uv = uvs;
		mesh.triangles = triangles;

		mesh.RecalculateBounds();
		mesh.RecalculateNormals();
		mesh.Optimize();
	}

	/*
	 * @returns a radius given spherical coordinates
	 * coordinate system used :
	 * http://mathworld.wolfram.com/SphericalCoordinates.html
	 */
	float radius( float theta, float phi ) {

		float diff = Mathf.Cos(16*phi)*Mathf.Cos(42*theta)*Mathf.Sin (phi)*Mathf.Cos(17*theta);
		return baseRadius + radiusDiff*diff;
	}
}