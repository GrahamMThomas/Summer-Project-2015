using UnityEngine;
using System.Collections;

public class EnemyHit : MonoBehaviour {
	//Creating this Variable to stop lasting hitboxes
	public int timeSensitive = 0;
	//Damages the Player
	void Update()
	{
		timeSensitive++;
	}
	void OnTriggerEnter(Collider colid)
	{
		if (colid.tag == "Player" && timeSensitive < 3 && !colid.GetComponent<Mover>().dead) {
			GetComponent<AudioSource>().Play ();
			colid.GetComponent<Mover>().health -= this.GetComponentInParent<Enemy>().damage;
		}
	}
}
