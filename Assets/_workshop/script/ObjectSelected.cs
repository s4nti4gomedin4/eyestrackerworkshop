using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.EyeTracking;

public class ObjectSelected : MonoBehaviour {
	/// <summary>
	/// The object renderer
	/// </summary>
	public Renderer renderer;
	/// <summary>
	/// The gazeaware object from tobbi sdk.
	/// </summary>
	private GazeAware gaze;

	void Start () {
		//Get the GazeAware Object
		gaze = GetComponent<GazeAware> ();
		renderer = GetComponent<Renderer> ();
	}
	
	void Update () {
		//Check if the object is selected and change color to red or white.
		if (gaze.HasGazeFocus) {
			renderer.material.color = Color.red;
		} else {
			renderer.material.color = Color.white;
		}
	}
}
