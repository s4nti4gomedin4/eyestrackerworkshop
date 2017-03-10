using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.EyeTracking;

public class FireByEyeTracking : MonoBehaviour {

	/// <summary>
	/// Object to attack
	/// </summary>
	private  GameObject m_Target;

	/// <summary>
	/// Ship controller
	/// </summary>
	private  Done_PlayerController m_ship;

	// Use this for initialization
	void Start () {
		EyeTracking.Initialize ();
		m_ship = GetComponent<Done_PlayerController> ();
	}
	
	// Update is called once per frame
	void Update () {
		m_Target = EyeTracking.GetFocusedObject ();

		if (m_Target != null) {
			//Get ship position
			Vector3 actualPos = m_ship.transform.position;

			//Change ship position to target position
			actualPos.x = m_Target.transform.position.x;
			m_ship.transform.position = actualPos;

			//Call fire from ship controller
			m_ship.Fire ();

			//Print target name
			print (string.Format("Shooting to ",m_Target.name));
		}
	}
}
