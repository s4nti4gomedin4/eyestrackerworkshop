using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.EyeTracking;

public class ObjecSelected : MonoBehaviour {

	public Renderer renderer;
	GazeAware gaze;
	// Use this for initialization
	void Start () {
		gaze = GetComponent<GazeAware> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (gaze.HasGazeFocus) {
			renderer.material.color = Color.red;
		} else {
			renderer.material.color = Color.white;
		}
	}
}
