using UnityEngine;
using System.Collections;

public class ItemObj : MonoBehaviour {
	public Item_Obj item_type;
	GameObject health_bar;
	float horzExtent, vertExtent;
	Camera main_cam;
	float life_time, time_spawned, start_refill_time, end_refill_time, missing_hp;
	bool entered, being_used;


	// Use this for initialization
	void Start () {
		health_bar = GameObject.Find ("Health Bar");
		horzExtent = (Camera.main.orthographicSize * Screen.width / Screen.height);
		vertExtent = (Camera.main.orthographicSize * Screen.height/ Screen.width);
		main_cam = GameObject.Find ("Main Camera").camera;
		life_time = 4.5f;
		time_spawned = Time.time;
		end_refill_time = .225f;
		entered = being_used = false;
	}

	// Update is called once per frame
	void Update () {
		//Destroy if out of view
		float cam_posX = main_cam.transform.position.x;
		float cam_posY = main_cam.transform.position.y;
	
		if (item_type != Item_Obj.OneUp && item_type != Item_Obj.EnergyTank && item_type != Item_Obj.CustomWeapon) {
			if (transform.position.x < cam_posX - horzExtent) {
				PhysEngine.objs.Remove(GetComponent<PE_Obj>());
				DestroyImmediate(gameObject);
			}
			else if (transform.position.x > cam_posX + horzExtent) {
				PhysEngine.objs.Remove(GetComponent<PE_Obj>());
				DestroyImmediate(gameObject);
			}
			else if (transform.position.y < cam_posY - vertExtent) {
				PhysEngine.objs.Remove(GetComponent<PE_Obj>());
				DestroyImmediate(gameObject);
			}
			else if (transform.position.y > cam_posY + vertExtent) {
				PhysEngine.objs.Remove(GetComponent<PE_Obj>());
				DestroyImmediate(gameObject);
			}	
			else if(time_spawned + life_time < Time.time && !being_used) {
				PhysEngine.objs.Remove(GetComponent<PE_Obj>());
				DestroyImmediate (gameObject);
			}
		}
	}

	void OnTriggerEnter(Collider other){
		OnTriggerStay (other);

	}

	void OnTriggerStay(Collider other){
		MegaMan_Custom mmc = other.GetComponent <MegaMan_Custom> ();
		if(mmc != null){
			if(item_type == Item_Obj.CustomWeapon) {
				mmc.currentWeapon = WeaponType.CustomWeapon;
				PhysEngine.objs.Remove(this.GetComponent<PE_Obj>());
				Destroy(gameObject);
			}
			if(item_type == Item_Obj.EnergyTank){
				mmc.num_energy_tanks++;
				PhysEngine.objs.Remove(this.GetComponent<PE_Obj>());
				Destroy (gameObject);
			}

		}
		MegaMan mm = other.GetComponent<MegaMan> ();
		if(mm != null){
			being_used = true;
			if(item_type == Item_Obj.EnergyCapsule){
				float hp = health_bar.GetComponent<HealthBar>().healthUnits.Count;
				if(hp == 28 && !entered) {
					PhysEngine.objs.Remove(GetComponent<PE_Obj>());
					Destroy (gameObject);
					return;
				}
				else if(!entered){ 
						if(hp > 20) missing_hp = 28f - hp;
						else missing_hp = 8f;
						waiting_for_refill();
						start_refill_time = Time.time;
						end_refill_time *= missing_hp;
						entered = true;
						Destroy(gameObject.GetComponent<ItemFlash>());
						Color temp  = gameObject.renderer.material.color;
						temp.a = 0f;
						gameObject.renderer.material.color = temp;
				}
				if(start_refill_time + end_refill_time > Time.time){
					mm.no_movement = true;
					mm.GetComponent<PE_Obj>().still = true;
					mm.GetComponent<Animator>().SetFloat ("speed", 0f);
				} else { 
					mm.GetComponent<PE_Obj>().still = false;
					mm.no_movement = false;
					PhysEngine.objs.Remove(this.GetComponent<PE_Obj>());
					Destroy (gameObject);
				}
			}
			if(item_type == Item_Obj.EnergyPellet){
				float hp = health_bar.GetComponent<HealthBar>().healthUnits.Count;
				if(hp == 28 && !entered) {
					PhysEngine.objs.Remove(GetComponent<PE_Obj>());
					Destroy (gameObject);
					return;
				}
				else if(!entered){ 
					if(hp > 26) missing_hp = 1f;
					else missing_hp = 2f;
					waiting_for_refill();
					start_refill_time = Time.time;
					end_refill_time *= missing_hp;
					entered = true;
					Destroy(gameObject.GetComponent<ItemFlash>());
					Color temp  = gameObject.renderer.material.color;
					temp.a = 0f;
					gameObject.renderer.material.color = temp;
				}
				if(start_refill_time + end_refill_time > Time.time){
					mm.GetComponent<PE_Obj>().still = true;
					mm.no_movement = true;
					mm.GetComponent<Animator>().SetFloat ("speed", 0f);
				} else { 
					mm.GetComponent<PE_Obj>().still = false;
					mm.no_movement = false;
					PhysEngine.objs.Remove(this.GetComponent<PE_Obj>());
					Destroy (gameObject);
				}
			}

			if(item_type == Item_Obj.EnergyTank){
				mm.num_energy_tanks++;
				PhysEngine.objs.Remove(this.GetComponent<PE_Obj>());
				Destroy (gameObject);
			}
			if(item_type == Item_Obj.OneUp){
				mm.num_lives++;
				PhysEngine.objs.Remove(this.GetComponent<PE_Obj>());
				Destroy (gameObject);
			}
			if(item_type == Item_Obj.WeaponEnergyCapsule){
				// if other weapon equipped restore energy
				// must implement energy bar for weapon if we decide to do this
				PhysEngine.objs.Remove(this.GetComponent<PE_Obj>());
				Destroy (gameObject);

			}
			if(item_type == Item_Obj.WeaponEnergyPellet){
				// if other weapon equipped restore energy
				// must implement energy bar for weapon if we decide to do this
				PhysEngine.objs.Remove(this.GetComponent<PE_Obj>());
				Destroy (gameObject);
			}
		}
		return;
	}
	void waiting_for_refill(){
		StartCoroutine(refill());
	}

	IEnumerator refill() {
		for(; missing_hp > 0; missing_hp--) {
			health_bar.GetComponent<HealthBar>().increaseByOne();
			yield return new WaitForSeconds(0.225f);
		}
		while( missing_hp <= 0) yield return null;
	}
	
}
