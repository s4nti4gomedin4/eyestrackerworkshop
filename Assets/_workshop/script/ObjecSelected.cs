using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.EyeTracking;

public class ObjecSelected : MonoBehaviour {

	private  MeshRenderer renderer;
	private GazeAware gaze;
	// Use this for initialization
	void Start () {
		gaze = GetComponent<GazeAware> ();
		renderer = GetComponent<MeshRenderer> ();
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
