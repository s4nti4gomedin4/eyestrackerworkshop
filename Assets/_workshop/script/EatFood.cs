using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent (typeof (Rigidbody))]
[RequireComponent (typeof (Collider))]
public class EatFood : MonoBehaviour {

	void OnCollisionEnter(Collision col){
		if (col.gameObject.CompareTag ("Food")) {
			Destroy (col.gameObject);
		}
	}
}
