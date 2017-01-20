using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Animator))]
public class MovementAnimations : MonoBehaviour {

	public string blendTreeVarName="speed";
	private Vector3 oldPosiotion;
	private Animator m_animator;
	private float speed;
	// Use this for initialization
	void Start () {
		oldPosiotion = transform.position;
		m_animator = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (oldPosiotion == transform.position) {
			speed = 0;
		}else if  (oldPosiotion != transform.position) {
			 speed = Vector3.Distance (oldPosiotion,transform.position)*10;
			oldPosiotion = transform.position;
		}

		m_animator.SetFloat (blendTreeVarName, speed);
	}
}
