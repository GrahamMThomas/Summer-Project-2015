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
		Spawners = GameObject.FindGameObjectsWithTag ("Respawn").ToList ();
		waveTextConstant.text = "Wave 1";
		spawnTime = timeToSpawn - 1;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.Escape)) {
			escapeMenu.gameObject.SetActive(!escapeMenu.gameObject.activeSelf);
		}
	}

	void FixedUpdate ()
	{
		moneyText.text = "" + Money;
		HealthText.text = Bob.GetComponent<Mover> ().health + "/" + Bob.GetComponent<Mover> ().maxHealth;
		zombiesLeftText.text = "Remaining: " + (zombiesPerWave + GameObject.FindGameObjectsWithTag ("Enemy").Count () / 2);
		spawnTime = (spawnTime % timeToSpawn) + 1;
		rotationText.text = "" + Mathf.Round (Bob.GetComponent<Mover> ().rotateSpeed * 100f) / 100f;
		GameObject.Find ("EventSystem").GetComponent<UnityEngine.EventSystems.EventSystem> ().SetSelectedGameObject (null);
		if (spawnTime == timeToSpawn && zombiesPerWave > 0) {
			int randNum = Random.Range (0, Spawners.Count);
			var newTroll = Instantiate (Troll);
			zombiesPerWave--;
			//Making sure the Enemies don't spawn ontop of the Player
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
		//UnityEditor.EditorApplication.isPlaying = false;
	}

	public void rotationUp ()
	{
		Bob.GetComponent<Mover> ().rotateSpeed += 0.1f;
	}

	public void rotationDown ()
	{
		Bob.GetComponent<Mover> ().rotateSpeed -= 0.1f;
	}

	IEnumerator waveDisplay ()
	{
		waveLetters.text = "Wave Complete!";
		GetComponent<AudioSource> ().clip = waveComplete;
		GetComponent<AudioSource> ().Play ();
		yield return new WaitForSeconds (2f);
		house.SetActive (false);
		waveLetters.text = "";
		nextWaveButton.gameObject.SetActive (true);
	}

	public void DumbButton ()
	{
		StartCoroutine ("newWave");
	}

	IEnumerator newWave ()
	{
		GameObject.Find ("EventSystem").GetComponent<UnityEngine.EventSystems.EventSystem> ().SetSelectedGameObject (null);
		nextWaveButton.gameObject.SetActive (false);
		Bob.transform.position = new Vector3 (21, 2.25f, -15);
		house.SetActive (true);
		waveNumber++;
		zombiesPerWave = 2 * waveNumber;
		waveLetters.text = "Wave " + waveNumber;
		waveTextConstant.text = "Wave " + waveNumber;
		yield return new WaitForSeconds (2f);
		waveLetters.text = " ";
		spawnTime = timeToSpawn - 1;
	}
}
