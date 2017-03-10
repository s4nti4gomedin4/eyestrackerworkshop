using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.EyeTracking;

public class TrackingPosition : MonoBehaviour {

	/// <summary>
	/// The position tracking.
	/// </summary>
	public Vector2 positionTracking;

	/// <summary>
	/// Fixed the axis on position 0
	/// </summary>
	public bool FixedXPosition;
	public bool FixedYPosition;
	public bool FixedZPosition=true;

	/// <summary>
	/// The object to change position by position tracking.
	/// </summary>
	public GameObject positionObject;

	void Start () {
		EyeTracking.Initialize ();
		if (positionObject == null) {
			positionObject = gameObject;
		}

	}
	
	void Update () {
		UpdateEyeTracking ();
	}
	public void UpdateEyeTracking(){

		//Check if the user is looking the screen
		if (EyeTracking.GetGazePoint ().IsWithinScreenBounds) {
			
			//Get the position on world point
			Vector3 posTracking = Camera.main.ScreenToWorldPoint (EyeTracking.GetGazePoint ().Screen);

			// Fixed some axis position
			if(FixedXPosition)
			posTracking.x = 0;
			if(FixedYPosition)
			posTracking.y = 0;
			if(FixedZPosition)
			posTracking.z = 0;

			//Print position
			//print (string.Format ("Position: {0}", posTracking));

			//Set the position to object tracking position
			positionObject.transform.position =posTracking;

			//Print the object focused name
			if (EyeTracking.GetFocusedObject () != null) {
				print (string.Format ("Focus object {0}", EyeTracking.GetFocusedObject ().name));
			} 
		}

	}
}
