using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.EyeTracking;

public class DonePlayerTrackingLogic : MonoBehaviour {


	public GameObject m_Target;
	public Done_PlayerController m_ship;

	// Use this for initialization
	void Start () {
		EyeTracking.Initialize ();
	}
	
	// Update is called once per frame
	void Update () {
		m_Target = EyeTracking.GetFocusedObject ();

		if (m_Target != null) {
			print ("target found");
			Vector3 actualPos = m_ship.transform.position;
			actualPos.x = m_Target.transform.position.x;
			m_ship.transform.position = actualPos;
			m_ship.Fire ();
		}
	}
}
