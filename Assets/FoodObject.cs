using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodObject : MonoBehaviour {



	public TextMesh foodValue;
	public void update(){
		print ("ScapeGameController.penaltyFoodWillbe: "+ScapeGameController.penaltyFoodWillbe);
		foodValue.text = string.Format("{0}",ScapeGameController.penaltyFoodWillbe);
	}

}
