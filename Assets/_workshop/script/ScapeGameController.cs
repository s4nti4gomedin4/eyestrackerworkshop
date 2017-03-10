using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.EyeTracking;
using UnityEngine.UI;

public class ScapeGameController : MonoBehaviour
{
	/// <summary>
	/// Controll type
	/// </summary>
	public TypeControlls controllSelected;

	//Game panels
	public Transform panelUsers;
	public Transform panelPositions;
	public Transform panelFood;
	public GameObject PanelUi;

	//User prefab for every controllr
	public GameObject userTrackingWithRigiPrefab;
	public GameObject userControlMovement;

	//Food prefab
	public FoodObject foodObject;

	//Enemy 
	public GameObject enemy;

	//UI Objects
	public Text message;
	public Text scoreText;

	//Score values
	private int score;
	public int foodAmount = 10;
	private const int foodValue = 10;
	public  int penaltyFood ;
	public static int penaltyFoodWillbe=10;



	//Time values
	private float LIMIT_CONTROLL_LOCKED_TIME = 0.2f;
	private float timeControllLocked = 0;

	//Users alive
	private TrackingWithRigi[] usersTrackingWithRigi;
	private MoveWithControll[] usersMoveWithControll;

	//User selected index
	private int indexUser;

	//Game state
	private bool playing;

	//Penalty logic
	private EatFood lasObjectEatFood;


	//Suscribe to eatfood events

	void OnEnable(){
		EatFood.eat += EatHandler;
	}

	void OnDisable(){
		EatFood.eat -= EatHandler;
	}



	IEnumerator Start ()
	{
		//InitValues
		penaltyFoodWillbe = foodValue;
		penaltyFood = 0;
		PanelUi.SetActive (false);
		playing = false;
		score = 0;
		printScore ();
		enemy.transform.position = Vector3.zero;
		enemy.SetActive (true);
		indexUser = 0;

		//Check controlls type and create players
		if (controllSelected == TypeControlls.MoveWithEyes) {
			
			CreatePlayers (userTrackingWithRigiPrefab);
			yield return null;
			usersTrackingWithRigi = panelUsers.GetComponentsInChildren<TrackingWithRigi> ();

		} else if (controllSelected == TypeControlls.MoveWithControl) {
			
			CreatePlayers (userControlMovement);
			yield return null;
			usersMoveWithControll = panelUsers.GetComponentsInChildren<MoveWithControll> ();

		}

		//Start the game
		createFood ();
		playing = true;
	}

	void Update ()
	{
		penaltyFoodWillbe = foodValue;

		//Check game state
		if (!playing) {
			return;
		}

		//Check food remains
		if (panelFood.childCount == 0) {
			UserWin ();
			playing = false;
			return;
		}

		//Check users alive
		if (panelUsers.childCount == 0) {
			Userlose ();
			playing = false;
			return;
		}

	

		//Check controlls inputs
		if (controllSelected == TypeControlls.MoveWithEyes) {
			//Not finished yet
			CheckActionsForControllerMoveWithEyes ();
			
		} else if (controllSelected == TypeControlls.MoveWithControl) {
			
			CheckActionsForControllerMoveWithControl ();

		}

	}

	//Not finished yet
	public void CheckActionsForControllerMoveWithEyes(){
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
	}
	public void CheckActionsForControllerMoveWithControl(){
		if (EyeTracking.GetGazePoint ().IsWithinScreenBounds) {
			var pos = Camera.main.ScreenToWorldPoint (EyeTracking.GetGazePoint ().Screen);
			var objects = Physics.OverlapSphere (pos,5);
			for (int i = 0; i < objects.Length; i++) {
				if (objects [i].CompareTag ("Player")) {
					for (int j = 0; j < usersMoveWithControll.Length; j++) {
						if (usersMoveWithControll [j] != null) {
							usersMoveWithControll [j].beingUsed = usersMoveWithControll [j].gameObject == objects [i].gameObject;
							if (usersMoveWithControll [j].beingUsed) {
								if (lasObjectEatFood != null) {
									if (usersMoveWithControll [j].gameObject == lasObjectEatFood.gameObject) {

										int nextPenalty = foodValue - (penaltyFood + 1);

										penaltyFoodWillbe = Mathf.Clamp (nextPenalty, 0, foodValue);
									} else {
										print ("selected new foodeatobject");
										ScapeGameController.penaltyFoodWillbe=foodValue;
									}
								} else {
									ScapeGameController.penaltyFoodWillbe=foodValue;
								}

							}

						}

					}
					break;
				}
			}
		}
	}

	/// <summary>
	/// Creates the food on random positions from x range [-7,37 , 7.37] and z range [-3 , 3] with y = 0.
	/// </summary>
	public void createFood(){
		DestroyChilds (panelFood);
		for (int i = 0; i < foodAmount; i++) {
			var newFood = Instantiate (foodObject);
			Vector3 newPosition = Vector3.zero;
			newPosition.x = Random.Range (-7.37f, 7.37f);
			newPosition.z = Random.Range (-3, 3);
			newFood.transform.position = newPosition;
			newFood.transform.SetParent (panelFood);
		}
	}
	/// <summary>
	/// Creates players on every positions.
	/// </summary>
	/// <param name="objectUser">Object user.</param>
	public void CreatePlayers( GameObject objectUser){

		DestroyChilds (panelUsers);
		foreach(Transform positionUser in panelPositions) {
			var newUser = Instantiate (objectUser);
			newUser.transform.position = positionUser.position;
			newUser.transform.SetParent (panelUsers);
		}
	}

	/// <summary>
	/// Handle on eat event.
	/// </summary>
	/// <param name="objectEat">Object eat.</param>
	public void EatHandler(EatFood objectEat){
		if (lasObjectEatFood == objectEat) {
			penaltyFood++;
			penaltyFood=Mathf.Clamp (penaltyFood, 0, foodValue);
		}
		lasObjectEatFood = objectEat;
		int scoreByfood = foodValue - penaltyFood;
		score += scoreByfood * panelUsers.childCount;
		printScore ();
	}

	/// <summary>
	/// Prints the score.
	/// </summary>
	public void printScore(){
		int maxScore = foodValue * foodAmount * panelPositions.childCount;
		scoreText.text = string.Format ("Score: {0}/{1}",score,maxScore);
	}

	/// <summary>
	/// Destroies the childs from transfrom.
	/// </summary>
	/// <param name="panelToDestroyChilds">Panel to destroy childs.</param>
	public void DestroyChilds(Transform panelToDestroyChilds){
		foreach (Transform child in panelToDestroyChilds) {
			DestroyImmediate (child.gameObject);
		}
	}

	/// <summary>
	/// Gets the user panel.
	/// </summary>
	/// <returns>The user panel.</returns>
	public Transform GetUserPanel ()
	{
		
		return panelUsers;
	}

	/// <summary>
	/// Clears the game objects, like food, users and enemy.
	/// </summary>
	public void ClearGameObjects(){
		DestroyChilds (panelFood);
		DestroyChilds (panelUsers);
		enemy.SetActive (false);
	}

	/// <summary>
	/// User Won the game.
	/// </summary>
	public void UserWin(){
		print ("win");
		PanelUi.SetActive (true);
		message.text="You Win!";
		playing = false;
		ClearGameObjects ();
	}

	/// <summary>
	/// User lost the game
	/// </summary>
	public void Userlose(){
		print ("lose");
		PanelUi.SetActive (true);
		message.text="You Lose!";
		playing = false;
		ClearGameObjects ();
	}
	
	/// <summary>
	/// Raises the acept event.
	/// </summary>
	public void OnAcept(){
		
		StartCoroutine (Start ());
	}

	/// <summary>
	/// Type controlle.
	/// </summary>
	public enum TypeControlls
	{
		MoveWithEyes,
		MoveWithControl}

	;
}
