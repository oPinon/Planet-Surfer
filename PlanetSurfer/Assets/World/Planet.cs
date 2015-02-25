using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

//[ExecuteInEditMode]
public class Planet : MonoBehaviour {

	public struct Coeff { 
		public float factor;
		public int period;
		public Coeff(float factor, int period) { this.factor=factor; this.period=period; }
	}

	public float mass = 10.0f;
	public float baseRadius = 10.0f;
	public float radiusDiff = 1.0f;
	public int angularMeshResolution = 128;
	public int angularColliderResolution = 256;
	public float Steepness = 0.1f;
	public List<Coeff> surfaceCoeffs = new List<Coeff>();

	static int toOdd( int value ) { return value + 1 - (value%2); }

	public void computeGeometry() {

		// Computing mesh
		MeshFilter meshFilter = this.GetComponent<MeshFilter>();
		if(!meshFilter) { Debug.LogError( "Couldn't find meshFilter in planet " + this.name ); }
		else {
            if (!meshFilter.sharedMesh) { meshFilter.sharedMesh = new Mesh(); }
			int meshResolution = (int) Mathf.Max(8,baseRadius*angularMeshResolution);
			createUVSphere( meshFilter.sharedMesh, meshResolution, toOdd ( (int) Mathf.Sqrt(meshResolution)) );
        }
	
		// Computing EdgeCollider
		EdgeCollider2D collider = this.GetComponent<EdgeCollider2D>();
		if(!collider) { Debug.LogError( "Couldn't find edgeCollider2D in planet " + this.name ); }
		else { computeEdgeCollider(collider, (int) Mathf.Max(8,baseRadius*angularColliderResolution)); }
	}

	void generate() {

		int basePeriod = (int) (32*Steepness*baseRadius);
		surfaceCoeffs.Clear();
		for(int i=0; i<3; i++) {
			float factor = Random.Range(0f,1f);
			int period = (int) Random.Range(basePeriod/2f,basePeriod*1.5f);
			surfaceCoeffs.Add(new Coeff(factor,period));
		}
		computeGeometry();
	}

	void Start () {

		generate();
	}
	
	// Update is called once per frame
	void Update () {

		//if (!Application.isPlaying) { generate(); }

    }

	/*
	 * Computes a 2Dcollider alongs the planet's surface
	 */
	void computeEdgeCollider(EdgeCollider2D collider, int nbPoints) {

		Vector2[] points = new Vector2[nbPoints+1];
		for(int i=0; i<nbPoints; i++) {
			float theta = Mathf.PI/2 + 2*Mathf.PI * (float) i / nbPoints;
			float x = xFromAngle(theta,Mathf.PI/2);
			float y = yFromAngle(theta,Mathf.PI/2);
			points[i] = new Vector2(x,y) * radius(theta,Mathf.PI/2);
		}
		points[nbPoints] = points[0];
		collider.points = points;
	}
    
    // TODO: a weird seam is visible on one side of the sphere
	/*
	 * @returns a UVSphere mesh with a variable radius
	 * (using the @radius function)
	 * Phi = Latitude in radians [0;Pi]
	 * Theta = Longitude in radians [0;2Pi]
	 * code based on http://wiki.unity3d.com/index.php/ProceduralPrimitives
	 * the sphere upper part is toward -z (Vector3.back)
	 */
	void createUVSphere( Mesh mesh, int nbLong, int nbLat ) {

		Vector3[] vertices = new Vector3[(nbLong+1) * nbLat + 2];

		#region Vertices
		// top pole
		float x0 = xFromAngle(0,0);
		float y0 = yFromAngle(0,0);
		float z0 = zFromAngle(0,0);
		vertices[0] = new Vector3(x0,y0,z0) * radius(0, 0);
		for( int lat = 0; lat < nbLat; lat++ ) {
			float phi = Mathf.PI * (float) (lat+1) / (nbLat+1); // latitude

			for( int lon=0; lon<=nbLong; lon++) {
				float theta = 2*Mathf.PI * (float) (lon == nbLong ? 0 : lon) / nbLong; // longitude
				float x = xFromAngle(theta,phi);
				float y = yFromAngle(theta,phi);
                float z = zFromAngle(theta,phi);
				vertices[lon+lat*(nbLong+1)+1] = new Vector3(x,y,z)*radius(theta, phi);
			}
		}
		// lower pole
		float x1 = xFromAngle(0,Mathf.PI);
		float y1 = yFromAngle(0,Mathf.PI);
		float z1 = zFromAngle(0,Mathf.PI);
		vertices[vertices.Length-1] = new Vector3(x1,y1,z1) * radius (0, Mathf.PI);
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

        mesh.Clear();

		mesh.vertices = vertices;
		mesh.uv = uvs;
		mesh.triangles = triangles;

		mesh.RecalculateBounds();
		mesh.RecalculateNormals();
		mesh.Optimize();
	}

    void createIcoSphere(Mesh mesh, int recursionLevel)
    {
        Mesh icosphere = IcoSphere.Create(recursionLevel);

        // TODO: change radius

        mesh.Clear();
        mesh.vertices = icosphere.vertices;
		mesh.triangles = icosphere.triangles;
        mesh.uv = icosphere.uv;
        mesh.normals = icosphere.normals;

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.Optimize();
    }

	void createFlatMesh(Mesh mesh, int nbPoints) {

		Vector3[] points = new Vector3[nbPoints+2];
		int[] triangles = new int[3*nbPoints];
		for(int i=0; i<nbPoints; i++) {

			float theta = Mathf.PI/2 + 2*Mathf.PI * (float) i / nbPoints;
			float x = xFromAngle(theta,Mathf.PI/2);
			float y = yFromAngle(theta,Mathf.PI/2);

			points[i] = new Vector3(x,y,0) * radius(theta,Mathf.PI/2);

			int nextVertice = (i+1)%nbPoints;
			triangles[3*i+0] = i;
			triangles[3*i+1] = nextVertice;
			triangles[3*i+2] = nbPoints+1;
		}
		points[nbPoints] = points[0];
		points[nbPoints+1] = Vector3.zero; // last point is (0,0,0)

		mesh.Clear();
		mesh.vertices = points;
		mesh.triangles = triangles;

		mesh.RecalculateBounds();
		mesh.RecalculateNormals();
	}

	/*
	 * @returns a radius given spherical coordinates
	 * coordinate system used :
	 * http://mathworld.wolfram.com/SphericalCoordinates.html
	 * Phi = Latitude in radians [0;Pi]
	 * Theta = Longitude in radians [0;2Pi]
	 */
	float radius( float theta, float phi ) {

		float diff = 0f;
		foreach(Coeff c in surfaceCoeffs) { diff += c.factor*Mathf.Cos(c.period*theta); }
		diff *= Mathf.Max( 0, -Mathf.Sin(3*phi) ); // equator ( playing part )
		return baseRadius + radiusDiff*diff;
	}

	static float xFromAngle( float theta, float phi ) { return -Mathf.Cos(theta)*Mathf.Sin(phi); }
	static float yFromAngle( float theta, float phi ) { return Mathf.Sin(theta)*Mathf.Sin(phi); }
	static float zFromAngle( float theta, float phi ) { return Mathf.Cos(phi); }
}