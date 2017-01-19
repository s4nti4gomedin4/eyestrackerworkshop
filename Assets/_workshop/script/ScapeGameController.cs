using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.EyeTracking;

public class ScapeGameController : MonoBehaviour
{

	public Transform panelUsers;
	public Transform panelPositions;


	float LIMIT_CONTROLL_LOCKED_TIME = 0.2f;
	float timeControllLocked = 0;

	public enum TypeControlle
	{
MoveWithEyes,
MoveWithControl}

	;

	public TypeControlle controllSelected;

	public GameObject userTrackingWithRigiPrefab;
	public GameObject userControllMovement;

	private int indexUser;
	private TrackingWithRigi[] usersTrackingWithRigi;
	private MoveWithControll[] usersMoveWithControll;
	private List<GameObject> activeUsers;




	IEnumerator Start ()
	{
		indexUser = 0;
		activeUsers = new List<GameObject> ();
		if (controllSelected == TypeControlle.MoveWithEyes) {
			
			foreach(Transform positionUser in panelPositions) {
				var newUser = Instantiate (userTrackingWithRigiPrefab);
				newUser.transform.position = positionUser.position;
				newUser.transform.SetParent (panelUsers);
			}
			yield return null;
			usersTrackingWithRigi = panelUsers.GetComponentsInChildren<TrackingWithRigi> ();
		} else if (controllSelected == TypeControlle.MoveWithControl) {
			foreach(Transform positionUser in panelPositions) {
				var newUser = Instantiate (userControllMovement);
				newUser.transform.position = positionUser.position;
				newUser.transform.SetParent (panelUsers);

			}
			yield return null;
			usersMoveWithControll = panelUsers.GetComponentsInChildren<MoveWithControll> ();

		}
	
	}


	public Transform getActiveUsers ()
	{
		/*if (activeUsers.Count == 0) {
			if (controllSelected == TypeControlle.MoveWithEyes) {
				
				if (usersTrackingWithRigi != null) {
					for (int i = 0; i < usersTrackingWithRigi.Length; i++) {
						if (usersTrackingWithRigi [i].gameObject.activeInHierarchy) {
							activeUsers.Add (usersTrackingWithRigi [i].gameObject);
						}

					}
				}

			} else if (controllSelected == TypeControlle.MoveWithControl) {
				if (usersMoveWithControll != null) {
					for (int i = 0; i < usersMoveWithControll.Length; i++) {
						if (usersMoveWithControll [i].gameObject.activeInHierarchy) {
							activeUsers.Add (usersMoveWithControll [i].gameObject);
						}

					}
				}
			}
		}*/	
		return panelUsers;
	}
	
	// Update is called once per frame
	void Update ()
	{
		

		//If move the controll

			
		if (controllSelected == TypeControlle.MoveWithEyes) {
			if (Input.GetAxis ("Vertical") != 0) {
				timeControllLocked += Time.deltaTime;
				if (timeControllLocked < LIMIT_CONTROLL_LOCKED_TIME) {
					return;
				}
				timeControllLocked = 0;
				if (Input.GetAxis ("Vertical") > 0) {
					print ("Up");
					indexUser++;
					if (indexUser >= usersTrackingWithRigi.Length) {
						indexUser = 0;
					}

				} else {
					print ("Down");
					indexUser--;
					if (indexUser < 0) {
						indexUser = usersTrackingWithRigi.Length - 1;
					}
				}
				for (int i = 0; i < usersTrackingWithRigi.Length; i++) {
					usersTrackingWithRigi [i].beingUsed = i == indexUser;
				}
			}
		} else if (controllSelected == TypeControlle.MoveWithControl) {
			if (EyeTracking.GetGazePoint ().IsWithinScreenBounds) {
				var pos = Camera.main.ScreenToWorldPoint (EyeTracking.GetGazePoint ().Screen);
				var objects = Physics.OverlapSphere (pos,5);
				for (int i = 0; i < objects.Length; i++) {
					if (objects [i].CompareTag ("Player")) {
						for (int j = 0; j < usersMoveWithControll.Length; j++) {
							if(usersMoveWithControll [j]!=null)
								usersMoveWithControll [j].beingUsed = usersMoveWithControll [j].gameObject == objects [i].gameObject;
						}
						break;
					}
				}
			}

		
			 
		}
		




		
	}
}
