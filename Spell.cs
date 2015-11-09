using UnityEngine;
using System.Collections;

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
		if (enemy.tag == "Enemy") {
			enemy.GetComponent<Enemy>().Health -= 25;
			StartCoroutine("KillStuff");
		}
		else{
		}
	}
	IEnumerator KillStuff()
	{
		this.GetComponent<ParticleSystem> ().Stop ();
		Transform explosionObject = Instantiate (explosion);
		explosionObject.transform.position = this.transform.position;
		Destroy (this.gameObject);
		yield return new WaitForSeconds(0f);
	}
}
