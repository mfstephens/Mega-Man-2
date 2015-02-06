using UnityEngine;
using System.Collections;

public class IceBall : MonoBehaviour {
	private PE_Obj peo;
	public float	horzExtent, velX, velY;
	public bool stomp;
	GameObject 	mega_man, custom_boss;
	public float rate = 1.8f, y_rate, x_rate, wait_time, wait_time_start;
	// Use this for initialization
	
	void Awake(){
		horzExtent = (Camera.main.orthographicSize * Screen.width / Screen.height);
		mega_man = GameObject.Find ("Mega Man");
		custom_boss = GameObject.Find ("Boss_Custom");
		wait_time = .8f;
		wait_time_start = Time.time;
		gameObject.renderer.material.color = new Color(1f, 1f, 1f, 0f);
		if(stomp){
			wait_time = 0f;
		}
	}
	
	void Start () {
	}
	
	void Update(){
		if (wait_time + wait_time_start > Time.time){
			if(custom_boss.transform == null){ Destroy(gameObject); return;}
			Vector3 end_of_face = custom_boss.transform.position;		
			// boss is facing forwards
			if (custom_boss.transform.eulerAngles.y != 0) {
				end_of_face.x -= .6f;
				end_of_face.y += .4f;
				if(mega_man.transform.position.y > 0f)transform.eulerAngles = new Vector3(0, 180, 90);
			} else {
				end_of_face.x += .6f;
				end_of_face.y += .4f;
				if(mega_man.transform.position.y > 0f)transform.eulerAngles = new Vector3(0, 0, 90);
				else transform.eulerAngles = new Vector3(0, 0, 40);
			}
			transform.position = end_of_face;
			
			float x_dist = mega_man.transform.position.x - end_of_face.x;
			float y_dist = mega_man.transform.position.y - end_of_face.y;
			float angle = (Mathf.Atan(y_dist / x_dist));
			if(mega_man.transform.position.x <= transform.position.x){
				y_rate = (-rate * Mathf.Sin(angle));
				x_rate = (-rate * Mathf.Cos(angle));
			}
			else if(mega_man.transform.position.x > transform.position.x){
				y_rate = (rate * Mathf.Sin(angle));
				x_rate = (rate * Mathf.Cos(angle));
			}
			return;
		}
		if(!stomp) gameObject.renderer.material.color = new Color(1f, 1f, 1f, 1f);
		Vector3 temp = transform.position;
		temp.y += y_rate * Time.deltaTime;
		temp.x += x_rate* Time.deltaTime;
		transform.position = temp;
		
		float cam_pos = GameObject.Find ("Main Camera").camera.transform.position.x;
		if (transform.position.x < cam_pos - horzExtent) {
			Custom_Boss_Handler.iceballs.Remove(gameObject);
			Destroy (gameObject);
		}
		if (transform.position.x > cam_pos + horzExtent) {
			Custom_Boss_Handler.iceballs.Remove(gameObject);
			Destroy (gameObject);
		}
	}
	
	
	void OnTriggerEnter(Collider other) {
		PE_Obj otherPEO = other.GetComponent<PE_Obj>();
		if (otherPEO == null) return; 
		if(otherPEO.coll == PE_Collider.megaman){
			Custom_Boss_Handler.iceballs.Remove(gameObject);
			Destroy (gameObject);
		}
		return;
		
	}
}
