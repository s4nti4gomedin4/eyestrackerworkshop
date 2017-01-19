using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Rigidbody))]
public class FollowUser : MonoBehaviour
{
	
	public  float speed = 0.5f;
	public ScapeGameController controller;

	private Transform userToFollow;
	private Rigidbody rb;

	private float TIME_TO_CHANGE_USER_FOLLOWING = 5f;
	private float timeFollowing=0;

	void Start ()
	{
		//Get rigid body component
		rb = GetComponent<Rigidbody> ();
		//Set user to follow null
		userToFollow = null;

	}

	void Update ()
	{
		if(timeFollowing>TIME_TO_CHANGE_USER_FOLLOWING||userToFollow==null){
			userToFollow = getUserToFollow ();	
			timeFollowing = 0;
			rb.velocity=Vector3.zero;
		}

		if (userToFollow != null) {
		
			transform.LookAt(userToFollow.position);
			rb.MovePosition(transform.position + transform.forward * Time.deltaTime*speed);
			timeFollowing += Time.deltaTime;

		} 


	}



	/// <summary>
	/// Gets the user to follow.
	/// </summary>
	/// <returns>The user to follow.</returns>
	public Transform getUserToFollow ()
	{
		//Get all users
		Transform users=controller.getActiveUsers ();
		//If users size is 0 return null
		if (users.childCount == 0) {
			return null;
		}
		//Select random user
		GameObject userToFollow = selectRandomObjectFromArray(users);
		return userToFollow.transform;
	}

	/// <summary>
	/// Selects the random object from array.
	/// </summary>
	/// <returns>The random object from array.</returns>
	/// <param name="objecs">Objecs.</param>
	public GameObject selectRandomObjectFromArray(Transform objecs){
		int randIndex = Random.Range (0, objecs.childCount);
		GameObject selectedObject = objecs.GetChild (randIndex).gameObject;
		return selectedObject;
	}
}
