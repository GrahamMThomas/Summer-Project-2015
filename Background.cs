using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

//CURRENTLY WORKING:
public class Background : MonoBehaviour
{
	public GameObject Troll;
	public AudioClip waveComplete;
	public GameObject Bob;
	public GameObject house;
	public UnityEngine.UI.Text moneyText;
	public UnityEngine.UI.Image escapeMenu;
	public UnityEngine.UI.Text rotationText;
	public UnityEngine.UI.Text waveTextConstant;
	public UnityEngine.UI.Text zombiesLeftText;
	public UnityEngine.UI.Text HealthText;
	public UnityEngine.UI.Button nextWaveButton;
	public UnityEngine.UI.Text waveLetters;
	public List<GameObject> Spawners;
	public int Money = 0;
	public int timeToSpawn = 400;
	public int zombiesPerWave = 2;
	public int waveNumber = 1;
	int spawnTime;
	// Use this for initialization
	void Start ()
	{
		//Locate all the spawn points in the map
		Spawners = GameObject.FindGameObjectsWithTag ("Respawn").ToList ();
		waveTextConstant.text = "Wave 1";
		spawnTime = timeToSpawn - 1;
	}
	
	// Update is called once per frame
	void Update ()
	{
		//If a menu is pressed, display the menu
		if (Input.GetKeyDown (KeyCode.Escape)) {
			escapeMenu.gameObject.SetActive(!escapeMenu.gameObject.activeSelf);
		}
	}

	void FixedUpdate ()
	{
		//Display your current cash
		moneyText.text = "" + Money;
		//Creating the UI in the top right of the screen.
			//Player Health
		HealthText.text = Bob.GetComponent<Mover> ().health + "/" + Bob.GetComponent<Mover> ().maxHealth;
			//Zombies remaining text
		zombiesLeftText.text = "Remaining: " + (zombiesPerWave + GameObject.FindGameObjectsWithTag ("Enemy").Count () / 2);
		spawnTime = (spawnTime % timeToSpawn) + 1;
		//This is the rotation speed of the camera. When you press esc you will see this value.
		rotationText.text = "" + Mathf.Round (Bob.GetComponent<Mover> ().rotateSpeed * 100f) / 100f;
		//Work around to disable button highlighting after press.
		GameObject.Find ("EventSystem").GetComponent<UnityEngine.EventSystems.EventSystem> ().SetSelectedGameObject (null);
		//If it has been timeToSpawn seconds and there are still zombies remaining in the wave
		if (spawnTime == timeToSpawn && zombiesPerWave > 0) {
			//Pick a random spawn point and spawn the zombie
			int randNum = Random.Range (0, Spawners.Count);
			var newTroll = Instantiate (Troll);
			zombiesPerWave--;
			//Making sure the Enemies don't spawn ontop of the Player
			//Move the newTroll to the spawn point
			if (Mathf.Abs (Bob.transform.position.x - Spawners [randNum].transform.position.x) + 
				Mathf.Abs (Bob.transform.position.z - Spawners [randNum].transform.position.z) > 30) {
				newTroll.transform.position = Spawners [randNum].transform.position;
			} else if (Mathf.Abs (Bob.transform.position.x - Spawners [(randNum + 1) % Spawners.Count].transform.position.x) + 
				Mathf.Abs (Bob.transform.position.z - Spawners [(randNum + 1) % Spawners.Count].transform.position.z) > 30) {
				newTroll.transform.position = Spawners [(randNum + 1) % Spawners.Count].transform.position;
			} else {
				newTroll.transform.position = Spawners [(randNum + 2) % Spawners.Count].transform.position;
			}

		}
		//When there are no more zombies, display new wave text and allow the player to shop.
		if (zombiesPerWave <= 0 && GameObject.Find ("WaveText").GetComponent<Text> ().text == " " 
			&& GameObject.FindGameObjectsWithTag ("Enemy").Count () == 0) {
			print ("New Wave");
			StartCoroutine ("waveDisplay");
		}
	}

	public void Quit ()
	{
		Application.Quit ();
		//Must comment out line below in order to build game.
		//Line below is for Unity editor only.
		//UnityEditor.EditorApplication.isPlaying = false;
	}
	//These rotation methods are used for the buttons in the esc menu.
	public void rotationUp ()
	{
		Bob.GetComponent<Mover> ().rotateSpeed += 0.1f;
	}

	public void rotationDown ()
	{
		Bob.GetComponent<Mover> ().rotateSpeed -= 0.1f;
	}
	
	//Every time a wave ends
	IEnumerator waveDisplay ()
	{
		waveLetters.text = "Wave Complete!";
		GetComponent<AudioSource> ().clip = waveComplete;
		GetComponent<AudioSource> ().Play ();
		//Wait and then enabled the shop.
		yield return new WaitForSeconds (2f);
		house.SetActive (false);
		waveLetters.text = "";
		nextWaveButton.gameObject.SetActive (true);
	}
	//Buttons cannot use IEnumerators so I made this function
	public void DumbButton ()
	{
		StartCoroutine ("newWave");
	}

	//After the player hits the new wave button.
	IEnumerator newWave ()
	{
		//Disable the Button highlights
		GameObject.Find ("EventSystem").GetComponent<UnityEngine.EventSystems.EventSystem> ().SetSelectedGameObject (null);
		//Disable the next wave button
		nextWaveButton.gameObject.SetActive (false);
		//Reset the play position
		Bob.transform.position = new Vector3 (21, 2.25f, -15);
		//Hid the shop inside the house
		house.SetActive (true);
		waveNumber++;
		//Double the amount of zombies per wave
		zombiesPerWave = 2 * waveNumber;
		//Display The wave text for two seconds
		waveLetters.text = "Wave " + waveNumber;
		waveTextConstant.text = "Wave " + waveNumber;
		yield return new WaitForSeconds (2f);
		// This disable the wave text and start spawning.
		waveLetters.text = " ";
		spawnTime = timeToSpawn - 1;
	}
}
