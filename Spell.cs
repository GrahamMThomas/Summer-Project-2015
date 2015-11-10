using UnityEngine;
using System.Collections;
//NOTE: Some of the numbers have been hardcoded.
//	Will have to change if I am going to add another spell.
public class Spell : MonoBehaviour {
	public Transform explosion;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnTriggerEnter(Collider enemy)
	{
		//If the fireball projectile collides with an enemy, take away 25 hp.
		if (enemy.tag == "Enemy") {
			enemy.GetComponent<Enemy>().Health -= 25;
			StartCoroutine("KillStuff");
		}
		
		else{
		}
	}
	IEnumerator KillStuff()
	{
		//Once the projectile hits a mob. Stop it's movement and play the exploding animation. Then destroy it.
		this.GetComponent<ParticleSystem> ().Stop ();
		Transform explosionObject = Instantiate (explosion);
		explosionObject.transform.position = this.transform.position;
		Destroy (this.gameObject);
		yield return new WaitForSeconds(0f);
	}
}
