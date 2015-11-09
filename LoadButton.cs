using UnityEngine;
using System.Collections;
public class LoadButton : MonoBehaviour {

public void onClick()
	{
		GameObject.Find ("Shop Control").GetComponent<ShopScreen> ().LeaveShop ();
		Application.LoadLevel (0);
	}
	
}
