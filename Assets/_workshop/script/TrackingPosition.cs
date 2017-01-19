using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.EyeTracking;

public class TrackingPosition : MonoBehaviour {

	public Vector2 positionTracking;
	public GameObject positionObject;
	// Use this for initialization
	void Start () {
		EyeTracking.Initialize ();
		UpdateEyeTracking ();
	}
	
	// Update is called once per frame
	void Update () {
		

	}
	public void UpdateEyeTracking(){
		if (EyeTracking.GetGazePoint ().IsWithinScreenBounds) {
			var pos = Camera.main.ScreenToWorldPoint (EyeTracking.GetGazePoint ().Screen);
		
			pos.y = 0;
			positionObject.transform.position =pos;
			if (EyeTracking.GetFocusedObject () != null) {
				print(EyeTracking.GetFocusedObject().name);
			}
		}

		Invoke ("UpdateEyeTracking", .1f);
	}
}
