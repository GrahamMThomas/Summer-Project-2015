using UnityEngine;
using System.Collections;
public class LoadButton : MonoBehaviour {

public void onClick()
	{
		//Create a button in the shop so you can leave.
		GameObject.Find ("Shop Control").GetComponent<ShopScreen> ().LeaveShop ();
		Application.LoadLevel (0);
	}
	
}
