using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Shop : MonoBehaviour {
	Animator anim;
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
	}
	void OnTriggerEnter(Collider colid)
	{
		if (colid.gameObject.tag == "Player") {
			StartCoroutine("Bow");
			UnityEngine.UI.Text waveLetters = GameObject.Find ("WaveText").GetComponent<Text>();
			waveLetters.text = "Press E to Enter Shop";
		}
	}
	void OnTriggerStay(Collider colid)
	{
		if (Input.GetKey (KeyCode.E)) {
			GameObject.Find ("Bob").GetComponent<Mover>().Save ();
			Application.LoadLevel(1);
		}
	}
	void OnTriggerExit(Collider colid)
	{
		if (colid.gameObject.tag == "Player") {
			UnityEngine.UI.Text waveLetters = GameObject.Find ("WaveText").GetComponent<Text>();
			waveLetters.text = "";
		}
	}
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
