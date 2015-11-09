using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class ShopScreen : MonoBehaviour
{
	//All UI Transforms
	public UnityEngine.UI.Text BootsText;
	public UnityEngine.UI.Text BootsPrice;
	public UnityEngine.UI.Text JumpText;
	public UnityEngine.UI.Text JumpPrice;
	public UnityEngine.UI.Text MoneyText;
	public BinaryFormatter bf = new BinaryFormatter ();
	FileStream file;
	Mover.PlayerData data;
	int bootsPrice;

	// Use this for initialization
	void Start ()
	{
		{
			if (File.Exists (Application.persistentDataPath + "/playerInfo.dat")) {
				BinaryFormatter bf = new BinaryFormatter ();
				file = File.Open (Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
				data = (Mover.PlayerData)bf.Deserialize (file);
				file.Close ();
			} else {
				BinaryFormatter bf = new BinaryFormatter ();
				print ("File not found, Loading Test.dat");
				file = File.Open (Application.persistentDataPath + "/Test.dat", FileMode.Open);
				data = (Mover.PlayerData)bf.Deserialize (file);
				file.Close ();
			}
		}
		//Upgrades
		BootsPrice.text = "" + data.bootsLevel * 50;
		JumpPrice.text = "" + data.jumpLevel * 50;
	}

	public void UpgradeBoots ()
	{
		if (data.money >= int.Parse (BootsPrice.text)) {
			data.bootsLevel++;
			data.money -= int.Parse (BootsPrice.text);
			BootsPrice.text = "" + (int.Parse (BootsPrice.text) + 50);
		}
	}

	public void UpgradeJump ()
	{
		if (data.money >= int.Parse (JumpPrice.text)) {
			data.jumpLevel++;
			data.money -= int.Parse(JumpPrice.text);
			JumpPrice.text = "" + (int.Parse (JumpPrice.text) + 50);
		}
	}

	public void giveMoney()
	{
		data.money += 10;
	}

	// Update is called once per frame
	void Update ()
	{
		//UI object Updates
		MoneyText.text = "" + data.money;
		BootsText.text = "Boots Lv." + data.bootsLevel;
		JumpText.text = "Jump Lv." + data.jumpLevel;
		GameObject.Find ("EventSystem").GetComponent<UnityEngine.EventSystems.EventSystem> ().SetSelectedGameObject (null);
	}

	public void LeaveShop ()
	{
		if (File.Exists (Application.persistentDataPath + "/playerInfo.dat")) {
			file = File.Open (Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
		} else { // Using this to test the shop without having to play the game first.
			file = File.Open (Application.persistentDataPath + "/Test.dat", FileMode.Open);
		}
		bf.Serialize (file, data);
		file.Close ();
	}
}
