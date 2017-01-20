using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.EyeTracking;

[RequireComponent (typeof (Rigidbody))]
[RequireComponent (typeof (Collider))]
[RequireComponent (typeof (GazeAware))]
public class MoveWithControll : MonoBehaviour {



	public GameObject selected;
	public bool beingUsed; 
	public float speed=0.01f;
	void Start () {
	}
	
	void Update () {
		
		if (beingUsed) {
			float zAxis = Input.GetAxis ("Vertical");
			float xAxis = Input.GetAxis ("Horizontal");
			Vector3 newPosition = transform.position;

			newPosition.x += xAxis * speed;
			newPosition.z += zAxis * speed;
			transform.LookAt (newPosition);
			transform.position = newPosition;
			selected.SetActive (true);
		} else {
			selected.SetActive (false);
		}
		
	}
}
