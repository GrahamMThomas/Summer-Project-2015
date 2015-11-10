using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[RequireComponent(typeof(CharacterController))]
public class Mover : MonoBehaviour
{
	public float maxHealth = 100;
	public float health;
	public UnityEngine.UI.Image hpBar;
	public UnityEngine.UI.Button retryButton;
	public bool dead = false;
	public Transform DeathBlood;
	Background bg;
	//Idle Animation
	Animation anim;
	public AnimationClip idle;
	public AnimationClip run;
	//Movement Oriented Variables
	float speed = 5.0F;
	public float rotateSpeed = 3.5F;
	float jumpSpeed = 5F;
	float gravity = 20.0F;
	private Vector3 moveDirection = Vector3.zero;
	//Upgrades
	public int bootsLevel = 1;
	public int jumpLevel = 1;

	void Start ()
	{
		//Initialize health speed and jump levels
		health = maxHealth;
		anim = gameObject.GetComponent<Animation> ();
		bg = GameObject.Find ("Background").GetComponent<Background> ();
		Load ();
		speed = speed + bootsLevel;
		jumpSpeed = jumpSpeed + jumpLevel;

	}

	void Update ()
	{
		if (!dead) {
			CharacterController controller = GetComponent<CharacterController> ();
			//If the player is on the ground allow for movement based on the arrow keys or on the wasd keys.
			if (controller.isGrounded) {
				moveDirection = new Vector3 (0, 0, Input.GetAxis ("Vertical"));
				moveDirection = transform.TransformDirection (moveDirection);
				moveDirection *= speed;
				if (Input.GetKey (KeyCode.Space)) {
					moveDirection.y = jumpSpeed;
				}
			//Weird bug where the character was getting stuck. So if you are in the air and
			//you press the space bar. The character will be barely moved at all but just enough to get him unstuck.
			//Weird workaround because I couldn't get it to work any other way.
			} else if (Input.GetKeyDown (KeyCode.Space)) {
				transform.position = new Vector3 (transform.position.x + 0.1f, transform.position.y, transform.position.z + 0.1f);
			}
			//Rotate the character and the camera using the rotate speed that can be changed in the esc menu.
			transform.Rotate (0, Input.GetAxis ("Horizontal") * rotateSpeed, 0);
			//Have to use the legacy animations because those are the ones that game with the model that I downloaded.
			if (Input.GetAxis ("Vertical") == 0) {
				anim.clip = idle;
				anim.Play ();
			} else {
				anim.clip = run;
				anim.Play ();
			}
			//Gravity is based on Time.
			moveDirection.y -= gravity * Time.deltaTime;
			controller.Move (moveDirection * Time.deltaTime);
			//Health Bar
			hpBar.fillAmount = (float)health / (float)maxHealth;
			//Kinda self explanatory
			if (health <= 0) {
				dead = true;
			}
		} else if(GameObject.Find ("dokebimusa")){
			StartCoroutine ("Death");
		}
	}

	IEnumerator Death ()
	{
		//Make the player disapear and make the weapon disapear as well.
		GameObject.Find ("dokebimusa").SetActive (false);
		if (GameObject.Find ("Bip01")) {
			GameObject.Find ("Bip01").SetActive(false);
		}
		//Enable the retry button which reloads the game.
		retryButton.gameObject.SetActive (true);
		//Create the really bad looking blood.
		var blood = Instantiate (DeathBlood);
		blood.SetParent (transform);
		blood.transform.localPosition = new Vector3 (0, 0.7f, 0);
		//Raise the blood so It isn't inside the floor
		yield return new WaitForSeconds (1f);
	}
	//This next method is something I had to google because It was really complicated.
	//Essentially, you are creating a file which holds the Serialized data of all of the variables that you chose to save.
	public void Save ()
	{
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + "/playerInfo.dat");
		PlayerData data = new PlayerData ();
		//All My variables
		data.health = health;
		data.money = bg.Money;
		data.zombiesLeft = bg.zombiesPerWave;
		data.waveNum = bg.waveNumber;
		data.isShopping = bg.nextWaveButton.IsActive ();
		data.posX = transform.position.x;
		data.posY = transform.position.y;
		data.posZ = transform.position.z;
		data.rotationY = transform.eulerAngles.y;
		data.waveString = GameObject.Find ("WaveText").GetComponent<Text> ().text;
		data.rotateSpeed = rotateSpeed;
		//Upgrades
		data.bootsLevel = bootsLevel;
		data.jumpLevel = jumpLevel;
		//End Variables
		bf.Serialize (file, data);
		file.Close ();
	}

	public void Reset ()
	{
		//Delete the save data because the player died. Hardcore mode activated.
		if (File.Exists (Application.persistentDataPath + "/playerInfo.dat")) {
			File.Delete (Application.persistentDataPath + "/playerInfo.dat");
		}
		Application.LoadLevel (0);
	}
	//Basically the opposite of the save function. Deserialize the data and load the data from the file into the variables.
	//The method is called every time you launch the game, however, unless the file exists, it will do nothing
	public void Load ()
	{
		print ("Loaded: " + Application.persistentDataPath + "/playerInfo.dat");
		if (File.Exists (Application.persistentDataPath + "/playerInfo.dat")) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
			PlayerData data = (PlayerData)bf.Deserialize (file);
			file.Close ();
			//All My Variables
			health = data.health;
			bg.Money = data.money;
			bg.zombiesPerWave = data.zombiesLeft;
			bg.waveNumber = data.waveNum;
			bg.nextWaveButton.gameObject.SetActive (data.isShopping);
			bg.house.SetActive (!data.isShopping);
			transform.position = new Vector3 (data.posX, data.posY, data.posZ);
			transform.Rotate (0, data.rotationY, 0);
			GameObject.Find ("WaveText").GetComponent<Text> ().text = data.waveString;
			rotateSpeed = data.rotateSpeed;
			//Upgrades
			bootsLevel = data.bootsLevel;
			jumpLevel = data.jumpLevel;
			//End Variables
		}
	}
	//You need to create a class so you can serialize it. So Any variable I needed, I first needed to put it here.
	[Serializable]
	public class PlayerData
	{
		public float health;
		public int money;
		//For game Progress
		public int zombiesLeft;
		public int waveNum;
		public bool isShopping;
		public string waveString;
		//For Character Position
		public float posX;
		public float posY;
		public float posZ;
		public float rotationY;
		public float rotateSpeed;
		//Upgrades
		public int bootsLevel = 1;
		public int jumpLevel = 1;
	}
}
