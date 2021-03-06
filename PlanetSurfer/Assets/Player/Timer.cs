﻿using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour {

	public float MaxTime = 100; // in seconds
	private float _currentTime;
	private string _baseString;

	void Start()
	{
		if( !guiText )
		{
			Debug.Log( this.ToString() + " needs a GUIText component!");
			enabled = false;
			return;
		}
		_currentTime = 0;
		_baseString = guiText.text;
	}
	
	// Update is called once per frame
	void Update () {

		_currentTime += Time.deltaTime;
		guiText.text = _baseString + (int)(10*_currentTime)/10.0f + "s";
	}

	public float GetTime() { return _currentTime; }
}
