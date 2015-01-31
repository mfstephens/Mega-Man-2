using UnityEngine;
using System.Collections;

public class Blaster : MonoBehaviour {
	private PE_Obj peo;
	public float	speed = 3, horzExtent;
	GameObject 		mega_man;
	// Use this for initialization

	void Awake(){
		horzExtent = (Camera.main.orthographicSize * Screen.width / Screen.height);
		peo = GetComponent<PE_Obj>();
		peo.grav = PE_GravType.none;
		mega_man = GameObject.Find ("Mega Man");
		Vector3 end_of_gun = mega_man.transform.position;		
		// mega man is facing backwards
		if (mega_man.transform.eulerAngles.y != 0) {
			end_of_gun.x += -.5f;
			speed = -speed;
		} else {
			end_of_gun.x += .5f;
		}
		transform.position = end_of_gun;
		peo.vel.x = speed;

	}

	void Start () {
	}

	
	// Update is called once per frame
	// Note that we use Update for input but FixedUpdate for physics. This is because Unity input is handled based on Update
	void FixedUpdate () {
		float cam_pos = GameObject.Find ("Main Camera").camera.transform.position.x;
		if (transform.position.x < cam_pos - horzExtent) {
			PhysEngine.objs.Remove(GetComponent<PE_Obj>());
			MegaMan.blasters.Remove(gameObject);
			Destroy (gameObject);
		}
		if (transform.position.x > cam_pos + horzExtent) {
			PhysEngine.objs.Remove(GetComponent<PE_Obj>());
			MegaMan.blasters.Remove(gameObject);
			Destroy (gameObject);
		}
	}
	

	void OnTriggerEnter(Collider other) {
		PE_Obj otherPEO = other.GetComponent<PE_Obj>();
		if (otherPEO == null) {
			if(other.GetComponent<MrBotHandler>() != null){
				other.GetComponent<MrBotHandler>().DecrementHP();
				PhysEngine.objs.Remove(this.GetComponent<PE_Obj>());
				MegaMan.blasters.Remove(gameObject);
				Destroy (gameObject);
			}
			return;
		}
		if (otherPEO.coll == PE_Collider.mole) {
			other.GetComponent<MoleHandler>().DecrementHP();
			PhysEngine.objs.Remove(this.GetComponent<PE_Obj>());
			MegaMan.blasters.Remove(gameObject);
			Destroy (gameObject);
			return;
		}
		else if (otherPEO.coll == PE_Collider.pierobot) {
			other.GetComponent<WheelHandler>().DecrementHP();
			PhysEngine.objs.Remove(this.GetComponent<PE_Obj>());
			MegaMan.blasters.Remove(gameObject);
			Destroy (gameObject);
			return;
			}

		else if (otherPEO.coll == PE_Collider.burokki){
			if(GetComponent<PE_Obj>() != null){
				PhysEngine.objs.Remove(this.GetComponent<PE_Obj>());
				MegaMan.blasters.Remove(gameObject);
				Destroy (gameObject);
			}
			return;
		}
		else if (otherPEO.coll == PE_Collider.burokkiface) {
			other.GetComponent<BurokkiHandler>().DecrementHP();
			if(GetComponent<PE_Obj>() != null){
				PhysEngine.objs.Remove(this.GetComponent<PE_Obj>());
				MegaMan.blasters.Remove(gameObject);
				Destroy (gameObject);
			}
			return;
		}
		else if (otherPEO.coll == PE_Collider.boss) {
			other.GetComponent<MetalMan>().DecrementHP();
			if(GetComponent<PE_Obj>() != null){
				PhysEngine.objs.Remove(this.GetComponent<PE_Obj>());
				MegaMan.blasters.Remove(gameObject);
				Destroy (gameObject);
			}
			return;
		}
	}



}

