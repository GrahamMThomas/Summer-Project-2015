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
		//If the saved data is found, deserialize it so it can be used to upgrade the players attributes.
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
		//Boots and jump price is the level x 50
		BootsPrice.text = "" + data.bootsLevel * 50;
		JumpPrice.text = "" + data.jumpLevel * 50;
	}
	//Method is applied to a button.
	public void UpgradeBoots ()
	{
		//Parse the string to the price and then if you have the money, it allows you to buy it.
		if (data.money >= int.Parse (BootsPrice.text)) {
			data.bootsLevel++;
			data.money -= int.Parse (BootsPrice.text);
			BootsPrice.text = "" + (int.Parse (BootsPrice.text) + 50);
		}
	}
	//Similar to boots
	public void UpgradeJump ()
	{
		//Parse the string to the price and then if you have the money, it allows you to buy it.
		if (data.money >= int.Parse (JumpPrice.text)) {
			data.jumpLevel++;
			data.money -= int.Parse(JumpPrice.text);
			JumpPrice.text = "" + (int.Parse (JumpPrice.text) + 50);
		}
	}
	//This was a hidden button I used for text purposes.
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
		//If they press the leave shop button. Reserialize the data so I could be loaded.
		if (File.Exists (Application.persistentDataPath + "/playerInfo.dat")) {
			file = File.Open (Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
		} else { // Using this to test the shop without having to play the game first.
			file = File.Open (Application.persistentDataPath + "/Test.dat", FileMode.Open);
		}
		bf.Serialize (file, data);
		file.Close ();
	}
}
