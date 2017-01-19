using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.EyeTracking;

public class TrackingWithRigi : MonoBehaviour {
	Rigidbody rb;
	public bool beingUsed;
	Renderer rend;
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
		rend = GetComponent<Renderer> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (!beingUsed) {
			rend.material.color = Color.red;
			return;
		}
		rend.material.color = Color.blue;
		if (EyeTracking.GetGazePoint ().IsWithinScreenBounds) {
			var pos = Camera.main.ScreenToWorldPoint (EyeTracking.GetGazePoint ().Screen);

			pos.y = 0.5f;
			rb.AddForce (pos-transform.position);
			rb.velocity = Vector3.ClampMagnitude (rb.velocity, 1f);
		}
	}
}
