using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Shop : MonoBehaviour {
	Animator anim;
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
	}
	//If the player enters the shop range. The shopkeeper will bow and then allow you to enter
	//	the shop by pressing the E key.
	void OnTriggerEnter(Collider colid)
	{
		if (colid.gameObject.tag == "Player") {
			StartCoroutine("Bow");
			UnityEngine.UI.Text waveLetters = GameObject.Find ("WaveText").GetComponent<Text>();
			waveLetters.text = "Press E to Enter Shop";
		}
	}
	//If you press E while in range
	void OnTriggerStay(Collider colid)
	{
		//Load the new scene which is the shop.
		if (Input.GetKey (KeyCode.E)) {
			GameObject.Find ("Bob").GetComponent<Mover>().Save ();
			Application.LoadLevel(1);
		}
	}
	//If the player leaves the range, display no message so they player doesn't try to enter the shop.
	void OnTriggerExit(Collider colid)
	{
		if (colid.gameObject.tag == "Player") {
			UnityEngine.UI.Text waveLetters = GameObject.Find ("WaveText").GetComponent<Text>();
			waveLetters.text = "";
		}
	}
	//Make the shop keeper bow.
	IEnumerator Bow()
	{
		anim.SetBool ("Bow", true);
		yield return new WaitForSeconds(2f);
		anim.SetBool ("Bow", false);
	}
	// Update is called once per frame
	void Update () {
	
	}
}
