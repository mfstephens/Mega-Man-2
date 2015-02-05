using UnityEngine;
using System.Collections;

public class RandomItemDrop : MonoBehaviour {
	float random, energyCapsule, energyPellet, weaponEnergyCapsule, weaponEnergyPellet;
	GameObject temp;
	public bool should_drop = true;
	// Use this for initialization
	void Start () {
		random = Random.Range (0, 100);
		energyPellet = 25; // 25% chance drop (0-25)
		weaponEnergyPellet = 50; // 25% chance drop (25-50)
		weaponEnergyCapsule = 65; // 15% chance drop (50-65)
		energyCapsule = 80; // 15% chance drop (65-80)
		// drop nothing = 20% chance
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnDestroy(){
		if(!should_drop) return;
		Vector3 set_location = transform.position;
		if(random <= energyPellet) temp = Instantiate((GameObject)Resources.Load("EnergyPelletPrefab")) as GameObject;
		else if(random <= weaponEnergyPellet && random > energyPellet) temp = Instantiate((GameObject)Resources.Load("WeaponEnergyPelletPrefab")) as GameObject;
		else if(random <= weaponEnergyCapsule && random > weaponEnergyPellet) temp = Instantiate((GameObject)Resources.Load("WeaponEnergyCapsulePrefab")) as GameObject;
		else if(random <= energyCapsule && random > weaponEnergyCapsule) temp = Instantiate((GameObject)Resources.Load("EnergyCapsulePrefab")) as GameObject;
		// else temp = null;
		if(random < 80) temp.transform.position = set_location;
	}
}
