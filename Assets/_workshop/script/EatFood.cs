using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent (typeof (Rigidbody))]
[RequireComponent (typeof (Collider))]
public class EatFood : MonoBehaviour {

	public  delegate void EatFoodEvent(EatFood objectEat);
	public static event EatFoodEvent eat;

	void OnCollisionEnter(Collision col){
		if (col.gameObject.CompareTag ("Food")) {
			Destroy (col.gameObject);
			if (eat != null) {
				eat (this);
			}
		}
	}
}
