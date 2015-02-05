using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StartMenu : MonoBehaviour {

	private bool isShowing;
	public GameObject menu;
	public MegaMan mm;
	public GameObject energyTankPrefab;

	// Use this for initialization
	void Start () {
		isShowing = false;
		menu.SetActive (isShowing);
		GameObject megaman = GameObject.Find ("Mega Man");
		mm = megaman.GetComponent<MegaMan> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown("return") || Input.GetKeyDown("enter")) {
			isShowing = !isShowing;
			menu.SetActive(isShowing);
		}
		if (isShowing) {
			Text lives = GameObject.Find ("NumLives").GetComponent<Text>();
			lives.text = mm.num_lives.ToString();
			for (int i = 0; i < mm.num_energy_tanks; ++i) {
				Instantiate(energyTankPrefab);
			}
		} else {
		}

	}
}
