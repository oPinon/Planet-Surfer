using UnityEngine;
using System.Collections;

public class Score : MonoBehaviour {

	private int _score = 0;
	private string _baseString;

	// Use this for initialization
	void Start () {
		if(!guiText) {
			Debug.LogError("Score is not a GUIText" );
			enabled = false;
			return;
		}
		_baseString = guiText.text;
	}
	
	// Update is called once per frame
	void Update () {
		guiText.text = _baseString + _score;
	}

	public void addScore( int score ) {
		_score += score;
	}
}
