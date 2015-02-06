using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public enum MenuSelection {
	done,
	useTank
}

public class StartMenu : MonoBehaviour {

	private bool isShowing;
	public GameObject menu;
	public MegaMan mm;
	public MegaMan_Custom mm2;
	public GameObject energyTankPrefab;
	private float scalingFactor = 17.0f;
	MenuSelection selectedItem;
	Stack<GameObject> tanks;

	// Use this for initialization
	void Start () {
		isShowing = false;
		menu.SetActive (isShowing);
		GameObject megaman = GameObject.Find ("Mega Man");
		if(Application.loadedLevel == 1){
			mm = megaman.GetComponent<MegaMan> ();
		} else if(Application.loadedLevel == 2){
			mm2 = megaman.GetComponent<MegaMan_Custom> ();
		}
		selectedItem = MenuSelection.done;
		tanks = new Stack<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Application.loadedLevel == 2){
			if (Input.GetKeyDown("return") || Input.GetKeyDown("enter")) {
				if (selectedItem == MenuSelection.useTank) {
					if (mm2.num_energy_tanks != 0) {
						mm2.health.GetComponent<HealthBar>().fill();
						mm2.num_energy_tanks -= 1;
						GameObject removed = tanks.Pop ();
						Destroy(removed);
					}
				} else {
					isShowing = !isShowing;
					menu.SetActive(isShowing);
				}
			} else if ((Input.GetKey("up") || Input.GetKey("w")) && (selectedItem != MenuSelection.done)) {
				GameObject cursor = GameObject.Find("Cursor");
				Vector3 temp = cursor.GetComponent<RectTransform>().position;
				temp.y += (Screen.height / scalingFactor);
				cursor.transform.position = temp;
				selectedItem = MenuSelection.done;
			} else if ((Input.GetKey("down") || Input.GetKey("s")) && (selectedItem != MenuSelection.useTank)) {
				GameObject cursor = GameObject.Find("Cursor");
				Vector3 temp = cursor.GetComponent<RectTransform>().position;
				temp.y -= (Screen.height / scalingFactor);
				cursor.transform.position = temp;
				selectedItem = MenuSelection.useTank;
			}
			if (isShowing) {
				Time.timeScale = 0.0001f; // DO NOT SET TO 0
				Text lives = GameObject.Find ("NumLives").GetComponent<Text>();
				lives.text = ": " + mm2.num_lives.ToString();
				while (tanks.Count != mm2.num_energy_tanks) {
					GameObject go = Instantiate(energyTankPrefab) as GameObject;
					go.transform.SetParent(menu.transform, false);
					tanks.Push(go);
				}
			} else {
				Time.timeScale = 1.0f;
			}
		}else{
		if (Input.GetKeyDown("return") || Input.GetKeyDown("enter")) {
			if (selectedItem == MenuSelection.useTank) {
				if (mm.num_energy_tanks != 0) {
					mm.health.GetComponent<HealthBar>().fill();
					mm.num_energy_tanks -= 1;
					GameObject removed = tanks.Pop ();
					Destroy(removed);
				}
			} else {
				isShowing = !isShowing;
				menu.SetActive(isShowing);
			}
		} else if ((Input.GetKey("up") || Input.GetKey("w")) && (selectedItem != MenuSelection.done)) {
			GameObject cursor = GameObject.Find("Cursor");
			Vector3 temp = cursor.GetComponent<RectTransform>().position;
			temp.y += (Screen.height / scalingFactor);
			cursor.transform.position = temp;
			selectedItem = MenuSelection.done;
		} else if ((Input.GetKey("down") || Input.GetKey("s")) && (selectedItem != MenuSelection.useTank)) {
			GameObject cursor = GameObject.Find("Cursor");
			Vector3 temp = cursor.GetComponent<RectTransform>().position;
			temp.y -= (Screen.height / scalingFactor);
			cursor.transform.position = temp;
			selectedItem = MenuSelection.useTank;
		}
		if (isShowing) {
			Time.timeScale = 0.0001f; // DO NOT SET TO 0
			Text lives = GameObject.Find ("NumLives").GetComponent<Text>();
			lives.text = ": " + mm.num_lives.ToString();
			while (tanks.Count != mm.num_energy_tanks) {
				GameObject go = Instantiate(energyTankPrefab) as GameObject;
				go.transform.SetParent(menu.transform, false);
				tanks.Push(go);
			}
		} else {
			Time.timeScale = 1.0f;
		}

	}
	}
}
