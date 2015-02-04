using UnityEngine;
using System.Collections;

public class Razor : MonoBehaviour {
	private PE_Obj peo;
	public float	horzExtent, velX, velY;
	GameObject 		mega_man, metal_man;
	public float rate = 4.5f, y_rate, x_rate, wait_time, wait_time_start;
	// Use this for initialization
	
	void Awake(){
		horzExtent = (Camera.main.orthographicSize * Screen.width / Screen.height);
		mega_man = GameObject.Find ("Mega Man");
		metal_man = GameObject.Find ("Metal Man");
		wait_time = .23f;
		wait_time_start = Time.time;
	}
	
	void Start () {
	}

	void Update(){
		if (wait_time + wait_time_start > Time.time){
			if(metal_man.transform == null){ Destroy(gameObject); return;}
			Vector3 end_of_hand = metal_man.transform.position;		
			// metal man is facing forwards
			if (metal_man.transform.eulerAngles.y != 0) {
				end_of_hand.x += -.25f;
				end_of_hand.y += .37f;
			} else {
				end_of_hand.x += .25f;
				end_of_hand.y += .37f;
			}
			transform.position = end_of_hand;
			
			float x_dist = mega_man.transform.position.x - end_of_hand.x;
			float y_dist = mega_man.transform.position.y - end_of_hand.y;
			float angle = (Mathf.Atan(y_dist / x_dist));
			if(mega_man.transform.position.x <= transform.position.x){
				transform.GetComponent<Spin_razor>().is_left = true;
				y_rate = (-rate * Mathf.Sin(angle));
				x_rate = (-rate * Mathf.Cos(angle));
			}
			else if(mega_man.transform.position.x > transform.position.x){
				transform.GetComponent<Spin_razor>().is_left = false;
				y_rate = (rate * Mathf.Sin(angle));
				x_rate = (rate * Mathf.Cos(angle));
			}
			return;
		}
		Vector3 temp = transform.position;
		temp.y += y_rate * Time.deltaTime;
		temp.x += x_rate* Time.deltaTime;
		transform.position = temp;
		
		float cam_pos = GameObject.Find ("Main Camera").camera.transform.position.x;
		if (transform.position.x < cam_pos - horzExtent) {
			MetalMan.razors.Remove(gameObject);
			Destroy (gameObject);
		}
		if (transform.position.x > cam_pos + horzExtent) {
			MetalMan.razors.Remove(gameObject);
			Destroy (gameObject);
		}
	}
		

	void OnTriggerEnter(Collider other) {
		PE_Obj otherPEO = other.GetComponent<PE_Obj>();
		if (otherPEO == null) return; 
		if(otherPEO.coll == PE_Collider.megaman){
			MetalMan.razors.Remove(gameObject);
			Destroy (gameObject);
		}
			return;
	
	}
}

