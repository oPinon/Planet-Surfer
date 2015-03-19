using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Energy : MonoBehaviour {

	public float Width = 100, Height = 10;
	public float Margin = 5;
	public Texture Background, Foreground;

	private int _energy = 0;

	public void addEnergy( int energy ) {
		_energy += energy;
		_energy = Mathf.Min( _energy, 100 );
	}
	
	void OnGUI() {

		float x = this.transform.position.x * Screen.width;
		float y = (1-this.transform.position.y) * Screen.height;
		string message = (_energy >= 50) ? "Ready !" : _energy + "%";

		GUI.DrawTexture( new Rect( x, y, Width, Height), Background );
		GUI.DrawTexture( new Rect( x+Margin, y+Margin, (_energy*0.01f)*(Width-2*Margin), Height-2*Margin), Foreground );
		GUI.Label( new Rect( x+Width+2*Margin, y+Margin, Width, Height), message );
	}

	public int getEnergy() {
		return _energy;
	}
}
