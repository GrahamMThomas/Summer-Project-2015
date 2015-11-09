using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class Caster : MonoBehaviour
{
	//Shockwave when you press enter to cast
	public Transform castParticle;
	//Particles to use fireball
	public Transform fireBall;
	//Fireball specs
	float fireBallLiveTime = 2;
	int fireBallSpeed = 8;
	//Whether a cast is currently happening.
	public bool Casting = false;
	//The text object about casting.
	UnityEngine.UI.Text spName;
	//This of numbers used in the current spell cast
	List<int> currSpellnums = new List<int> ();
	//List of all the icons being currently use by the current spell
	public List<GameObject> currSpellIcon = new List<GameObject> ();
	//Every Image located at the bottom
	public List<GameObject> images = new List<GameObject> ();
	
	void Start ()
	{
		//Initializing spells attributes
		spName = GameObject.Find ("SpellName").GetComponent<Text> ();
		//This is initializing the images list
		for (int i = 0; i < 18; i++) {
			GameObject currentImage = GameObject.Find ("Image (" + i + ")");
			images.Add (currentImage);
			currentImage.SetActive (false);
			currentImage = GameObject.Find ("Mid (" + i + ")");
			images.Add (currentImage);
			currentImage.SetActive (false);
			currentImage = GameObject.Find ("Cast (" + i + ")");
			images.Add (currentImage);
			currentImage.SetActive (false);
		}
	}

	void Update ()
	{		
		//List the Spells unlocked here
		if (!GameObject.Find ("Bob").GetComponent<Mover> ().dead) {
			if ((Input.GetKey (KeyCode.Alpha1) || Input.GetKey (KeyCode.Keypad1)) && Casting == false) {
				//Set up the spell name and difficulty and Color here. Attributes are all the top. Damage in the spell script
				CastSetup ("FireBall", 3, Color.Lerp (Color.red, Color.yellow, 0.5f));
				GameObject.Find ("TextBack").GetComponent<AudioSource> ().Play ();
			}
			//Checking to see if you pressed a button to cast a spell
			if (Casting == true && currSpellnums.Count >= 0) {
				if (currSpellnums.Count >= 1) {
					if (currSpellnums [0] == 9 && Input.GetKeyDown (KeyCode.Keypad9)) {
						currSpellIcon [0].GetComponent<Image> ().color = Color.grey;
						currSpellIcon [0].GetComponent<AudioSource> ().Play ();
						currSpellnums.RemoveAt (0);
						currSpellIcon.RemoveAt (0);
					} else if (currSpellnums [0] == 8 && Input.GetKeyDown (KeyCode.Keypad8)) {
						currSpellIcon [0].GetComponent<Image> ().color = Color.grey;
						currSpellIcon [0].GetComponent<AudioSource> ().Play ();
						currSpellnums.RemoveAt (0);
						currSpellIcon.RemoveAt (0);
					} else if (currSpellnums [0] == 7 && Input.GetKeyDown (KeyCode.Keypad7)) {
						currSpellIcon [0].GetComponent<Image> ().color = Color.grey;
						currSpellIcon [0].GetComponent<AudioSource> ().Play ();
						currSpellnums.RemoveAt (0);
						currSpellIcon.RemoveAt (0);
					} else if (currSpellnums [0] == 6 && Input.GetKeyDown (KeyCode.Keypad6)) {
						currSpellIcon [0].GetComponent<Image> ().color = Color.grey;
						currSpellIcon [0].GetComponent<AudioSource> ().Play ();
						currSpellnums.RemoveAt (0);
						currSpellIcon.RemoveAt (0);
					} else if (currSpellnums [0] == 5 && Input.GetKeyDown (KeyCode.Keypad5)) {
						currSpellIcon [0].GetComponent<Image> ().color = Color.grey;
						currSpellIcon [0].GetComponent<AudioSource> ().Play ();
						currSpellnums.RemoveAt (0);
						currSpellIcon.RemoveAt (0);
					} else if (currSpellnums [0] == 4 && Input.GetKeyDown (KeyCode.Keypad4)) {
						currSpellIcon [0].GetComponent<Image> ().color = Color.grey;
						currSpellIcon [0].GetComponent<AudioSource> ().Play ();
						currSpellnums.RemoveAt (0);
						currSpellIcon.RemoveAt (0);
					} else if (currSpellnums [0] == 3 && Input.GetKeyDown (KeyCode.Keypad3)) {
						currSpellIcon [0].GetComponent<Image> ().color = Color.grey;
						currSpellIcon [0].GetComponent<AudioSource> ().Play ();
						currSpellnums.RemoveAt (0);
						currSpellIcon.RemoveAt (0);
					} else if (currSpellnums [0] == 2 && Input.GetKeyDown (KeyCode.Keypad2)) {
						currSpellIcon [0].GetComponent<Image> ().color = Color.grey;
						currSpellIcon [0].GetComponent<AudioSource> ().Play ();
						currSpellnums.RemoveAt (0);
						currSpellIcon.RemoveAt (0);
					} else if (currSpellnums [0] == 1 && Input.GetKeyDown (KeyCode.Keypad1)) {
						currSpellIcon [0].GetComponent<Image> ().color = Color.grey;
						currSpellIcon [0].GetComponent<AudioSource> ().Play ();
						currSpellnums.RemoveAt (0);
						currSpellIcon.RemoveAt (0);
					}
					//If all Nodes are passed, press enter to cast the spell
				} else if (Casting == true && Input.GetKeyDown (KeyCode.KeypadEnter)) {
					currSpellIcon [0].GetComponent<Image> ().color = Color.grey;
					GameObject.Find ("SpellName").GetComponent<AudioSource> ().Play ();       
					var castCircle = Instantiate (castParticle);
					//The next to lines put the Cast Particle between the character and the camera
					//creating the illusion that it is inside the UI.
					castCircle.SetParent (GameObject.Find ("ParticleLayer").transform);
					castCircle.transform.position = GameObject.Find ("ParticleLayer").transform.position;
					currSpellIcon.RemoveAt (0);
					Cast (spName.text);
					ResetCast ();
				}
			}	
		}
	}
	//Reset cast is used to set the List back to their empty positions as well and the images
	void ResetCast ()
	{
		foreach (GameObject thing in images) {
			thing.GetComponent<Image> ().color = Color.white;
			thing.SetActive (false);
		}
		currSpellIcon.Clear ();
		currSpellnums.Clear ();
		Casting = false;
		StartCoroutine ("SpellNameChange");
	}
	//Used to show the word cast after you successfully casted a spell
	IEnumerator SpellNameChange ()
	{
		spName.text = "Cast!";
		spName.color = Color.white;
		yield return new WaitForSeconds (.5f);
		if (spName.text == "Cast!") {
			spName.text = "";
		}
	}
	//Use to instantiate the fireball particle and correct the position and rotation.
	void Cast (string spellName)
	{
		if (spellName == "FireBall") {
			//ballOFire is the fireball spawned by this method.
			Transform ballOFire = Instantiate (fireBall);
			ballOFire.GetComponent<Rigidbody> ().velocity = transform.TransformDirection (Vector3.forward * fireBallSpeed);
			ballOFire.transform.position = new Vector3 (this.transform.position.x, transform.localPosition.y + 2, this.transform.position.z);
			ballOFire.eulerAngles = new Vector3 (0, GameObject.Find ("Bob").transform.eulerAngles.y + 90, 0);
			StartCoroutine ("SpellExpire", ballOFire);

		}
	}
	//Used to destory fireball after time lived was allowed
	IEnumerator SpellExpire (Transform spell)
	{
		yield return new WaitForSeconds (fireBallLiveTime);
		if (spell) {
			Destroy (spell.gameObject);
		}
	}
	//Creates images and casting combo for the spell
	void CastSetup (string name, int castNum, Color spColor)
	{
		spName.text = name;
		spName.color = spColor;
		Casting = true;
		for (int i = 0; i <= castNum; i++) {
			
			if (i != 0) {
				i = (i * 3);
			}
			
			int keyNum = Random.Range (1, 10);
			if (keyNum != 5 && i / 3 != castNum) {
				images [i].SetActive (true);
				currSpellIcon.Add (images [i]);
				if (keyNum == 1) {
					images [i].transform.rotation = Quaternion.Euler (0, 0, 225);
				} else if (keyNum == 2) {
					images [i].transform.rotation = Quaternion.Euler (0, 0, 270);
				} else if (keyNum == 3) {
					images [i].transform.rotation = Quaternion.Euler (0, 0, 315);
				} else if (keyNum == 4) {
					images [i].transform.rotation = Quaternion.Euler (0, 0, 180);
				} else if (keyNum == 6) {
					images [i].transform.rotation = Quaternion.Euler (0, 0, 0);
				} else if (keyNum == 7) {
					images [i].transform.rotation = Quaternion.Euler (0, 0, 135);
				} else if (keyNum == 8) {
					images [i].transform.rotation = Quaternion.Euler (0, 0, 90);
				} else if (keyNum == 9) {
					images [i].transform.rotation = Quaternion.Euler (0, 0, 45);
				}
				currSpellnums.Add (keyNum);
			} else if (keyNum == 5 && i / 3 != castNum) {
				images [i + 1].SetActive (true);
				currSpellIcon.Add (images [i + 1]);
				currSpellnums.Add (keyNum);
			} else {
				images [i + 2].SetActive (true);
				currSpellIcon.Add (images [i + 2]);
			}
			i = i / 3;
		}
	}

}