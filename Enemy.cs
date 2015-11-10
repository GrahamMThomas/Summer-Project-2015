using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
	public Transform target;
	public Transform Coin;
	public float speed = 6f;
	public int damage = 10;
	public static int MaxHealth;
	public Transform hpWords;
	public Transform hpBar;
	public Transform hitEffect;
	public AnimationClip att;
	public AnimationClip death;
	public AnimationClip run;
	public int Health;
	//Fix the death animation
	bool dying = false;
	int fix = 0;
	Animation anim;
	NavMeshAgent navMap;

	void Start ()
	{
		anim = GetComponent<Animation> ();
		navMap = gameObject.GetComponent<NavMeshAgent> ();
		MaxHealth = 100;
		Health = MaxHealth;
	}

	void Update ()
	{
		//Health Bar Stuff
		hpWords.GetComponent<Text> ().text = "" + Health;
		hpBar.GetComponent<Image> ().fillAmount = (float)Health / (float)MaxHealth;
		if (Health <= 0 && !dying) {
			StartCoroutine ("trollDeath");
		}
		if (this) {
			//Nav Mesh Stuff
			//Target the player and start walking toward them.
			ChangeTarget (GameObject.FindGameObjectWithTag ("Player").transform);
			//navMap.speed = speed;
			navMap.SetDestination (target.position);
			//Troll was attacking when it spawned, fix it.
			//Weird work around
			fix++;
			if (navMap.remainingDistance < 4 && fix > 5 && anim.clip.name == "Run" && !dying) {
				StartCoroutine ("trollAttack");
			}
		}
	}

	IEnumerator trollDeath ()
	{
		//When the troll runs out of health.
		dying = true;
		navMap.Stop ();
		//Change the animation so it looks like it's dying and play a noise.
		anim.clip = death;
		anim.Play ();
		//Drop a coin.
		var coinOb = Instantiate (Coin);
		//Place the coin in the correct position
		coinOb.transform.localPosition = new Vector3 (transform.position.x, transform.position.y + 3,
		                                              transform.position.z);
		//Throw the coin upward before it flys downward (Looks Cooler)
		coinOb.gameObject.GetComponent<Rigidbody> ().AddForce (new Vector3 (0, 300, 0));
		GetComponent<AudioSource> ().Play ();
		GetComponent<BoxCollider> ().enabled = false;
		yield return new WaitForSeconds (1.3f);
		//After the animation is over. Destroy it.
		Destroy (this.gameObject);
	}

	IEnumerator trollAttack ()
	{
		//Stop moving and begin the attack animation.
		navMap.Stop ();
		anim.clip = att;
		anim.Play ();
		yield return new WaitForSeconds (.9f);
		//Don't start attacking if it is dying.
		if (!dying) {
			//Play the animation
			var hit = Instantiate (hitEffect);
			hit.SetParent (transform);
			hit.transform.localPosition = new Vector3 (-0.01f, 0.125f, 0.19f);
			//Weird that I had to put this here twice.
			if (!dying) {
				yield return new WaitForSeconds (0.6f);
				//Start the movement again.
				navMap.Resume ();
				anim.clip = run;
				anim.Play ();
			}
		}
	}

	void ChangeTarget (Transform newtarget)
	{
		target = newtarget;
	}
}


