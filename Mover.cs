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
			if (controller.isGrounded) {
				moveDirection = new Vector3 (0, 0, Input.GetAxis ("Vertical"));
				moveDirection = transform.TransformDirection (moveDirection);
				moveDirection *= speed;
				if (Input.GetKey (KeyCode.Space)) {
					moveDirection.y = jumpSpeed;
				}
					
			} else if (Input.GetKeyDown (KeyCode.Space)) {
				transform.position = new Vector3 (transform.position.x + 0.1f, transform.position.y, transform.position.z + 0.1f);
			}
			transform.Rotate (0, Input.GetAxis ("Horizontal") * rotateSpeed, 0);
			if (Input.GetAxis ("Vertical") == 0) {
				anim.clip = idle;
				anim.Play ();
			} else {
				anim.clip = run;
				anim.Play ();
			}
			moveDirection.y -= gravity * Time.deltaTime;
			controller.Move (moveDirection * Time.deltaTime);
			//Health Bar
			hpBar.fillAmount = (float)health / (float)maxHealth;
			if (health <= 0) {
				dead = true;
			}
		} else if(GameObject.Find ("dokebimusa")){
			StartCoroutine ("Death");
		}
	}

	IEnumerator Death ()
	{
		GameObject.Find ("dokebimusa").SetActive (false);
		if (GameObject.Find ("Bip01")) {
			GameObject.Find ("Bip01").SetActive(false);
		}
		retryButton.gameObject.SetActive (true);
		var blood = Instantiate (DeathBlood);
		blood.SetParent (transform);
		blood.transform.localPosition = new Vector3 (0, 0.7f, 0);
		yield return new WaitForSeconds (1f);
	}
	
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
		if (File.Exists (Application.persistentDataPath + "/playerInfo.dat")) {
			File.Delete (Application.persistentDataPath + "/playerInfo.dat");
		}
		Application.LoadLevel (0);
	}

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