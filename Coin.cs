using UnityEngine;
using System.Collections;
//PURPOSE: Attach the script the the coin
public class Coin : MonoBehaviour
{
	public int value;
	public Transform pickup;
	bool killIt;
	Background bg;

	void Start ()
	{
		bg = GameObject.Find ("Background").GetComponent<Background> ();
		//Pick the value of the coin at random, scaling with wave number.
		value = Random.Range ((bg.waveNumber * 10) + 30, (bg.waveNumber * 10) + 50);
	}

	void OnTriggerEnter (Collider colid)
	{
		//When the player gets close.
		if (colid.gameObject.tag == "Player") {
			//Player the pickup animation.
			var pickupEffect = Instantiate (pickup);
			pickupEffect.transform.localPosition = transform.position;
			//Give the player the value of the coin.
			bg.Money += value;
			Destroy (gameObject);
		}
	}
	// Update is called once per frame
	void Update ()
	{
		//Rotate the coin to increase visibility
		transform.Rotate (new Vector3 (0, 0, 1));
		if (transform.position.y < 1) {
			transform.position = (new Vector3 (transform.position.x, 1, transform.position.z));
		}
	}
}
