using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.EyeTracking;
using UnityEngine.UI;

public class ScapeGameController : MonoBehaviour
{
	public TypeControlle controllSelected;
	public Transform panelUsers;
	public Transform panelPositions;
	public Transform panelFood;
	public GameObject PanelUi;
	public GameObject userTrackingWithRigiPrefab;
	public GameObject userControlMovement;
	public GameObject foodObject;
	public GameObject enemy;
	public Text message;
	private int indexUser;
	public int FoodNumber=10;
	private float LIMIT_CONTROLL_LOCKED_TIME = 0.2f;
	private float timeControllLocked = 0;
	private TrackingWithRigi[] usersTrackingWithRigi;
	private MoveWithControll[] usersMoveWithControll;
	private bool playing;



	IEnumerator Start ()
	{
		
		PanelUi.SetActive (false);
		playing = false;
		enemy.transform.position = Vector3.zero;
		enemy.SetActive (true);
		indexUser = 0;
		if (controllSelected == TypeControlle.MoveWithEyes) {
			
			CreatePlayers (userTrackingWithRigiPrefab);
			yield return null;
			usersTrackingWithRigi = panelUsers.GetComponentsInChildren<TrackingWithRigi> ();

		} else if (controllSelected == TypeControlle.MoveWithControl) {
			
			CreatePlayers (userControlMovement);
			yield return null;
			usersMoveWithControll = panelUsers.GetComponentsInChildren<MoveWithControll> ();

		}

		createFood ();
		playing = true;
	}
	public void createFood(){
		DestroyChilds (panelFood);
		for (int i = 0; i < FoodNumber; i++) {
			var newFood = Instantiate (foodObject);
			Vector3 newPosition = Vector3.zero;
			newPosition.x = Random.Range (-7.37f, 7.37f);
			newPosition.z = Random.Range (-3, 3);
			newFood.transform.position = newPosition;
			newFood.transform.SetParent (panelFood);
		}
	}
	public void CreatePlayers( GameObject objectUser){

		DestroyChilds (panelUsers);
		foreach(Transform positionUser in panelPositions) {
			var newUser = Instantiate (objectUser);
			newUser.transform.position = positionUser.position;
			newUser.transform.SetParent (panelUsers);
		}
	}
	public void DestroyChilds(Transform panelToDestroyChilds){
		foreach (Transform child in panelToDestroyChilds) {
			DestroyImmediate (child.gameObject);
		}
	}


	public Transform GetUserPanel ()
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
	public void ClearGameObjects(){
		DestroyChilds (panelFood);
		DestroyChilds (panelUsers);
		enemy.SetActive (false);
	}

	public void UserWin(){
		print ("win");
		PanelUi.SetActive (true);
		message.text="You Win!";
		playing = false;
		ClearGameObjects ();
	}
	public void Userlose(){
		print ("lose");
		PanelUi.SetActive (true);
		message.text="You Lose!";
		playing = false;
		ClearGameObjects ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!playing) {
			return;
		}
		if (panelFood.childCount == 0) {
			UserWin ();
			playing = false;
			return;
		}
		if (panelUsers.childCount == 0) {
			Userlose ();
			playing = false;
			return;
		}

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
	public void OnAcept(){
		
		StartCoroutine (Start ());
	}

	public enum TypeControlle
	{
		MoveWithEyes,
		MoveWithControl}

	;
}
