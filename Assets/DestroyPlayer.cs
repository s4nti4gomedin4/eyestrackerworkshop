using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Rigidbody))]
[RequireComponent (typeof (Collider))]
public class DestroyPlayer : MonoBehaviour {

	void OnCollisionEnter(Collision col){
		print ("OnCollisionEnter");
		if (col.gameObject.CompareTag ("Player")) {
			Destroy (col.gameObject);
		}
	}
}
