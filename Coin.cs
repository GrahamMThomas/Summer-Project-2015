using UnityEngine;
using System.Collections;

public class Coin : MonoBehaviour
{
	public int value;
	public Transform pickup;
	bool killIt;
	Background bg;

	void Start ()
	{
		bg = GameObject.Find ("Background").GetComponent<Background> ();
		value = Random.Range ((bg.waveNumber * 10) + 30, (bg.waveNumber * 10) + 50);
	}

	void OnTriggerEnter (Collider colid)
	{
		if (colid.gameObject.tag == "Player") {
			var pickupEffect = Instantiate (pickup);
			pickupEffect.transform.localPosition = transform.position;
			bg.Money += value;
			Destroy (gameObject);
		}
	}
	// Update is called once per frame
	void Update ()
	{
		transform.Rotate (new Vector3 (0, 0, 1));
		if (transform.position.y < 1) {
			transform.position = (new Vector3 (transform.position.x, 1, transform.position.z));
		}
	}
}
