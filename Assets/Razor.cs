using UnityEngine;
using System.Collections;

public class Razor : MonoBehaviour {
	private PE_Obj peo;
	public float	speed = 1f, horzExtent, velX, velY;
	GameObject 		mega_man, metal_man;
	public float rate = 2f, y_rate, x_rate;
	// Use this for initialization
	
	void Awake(){
		horzExtent = (Camera.main.orthographicSize * Screen.width / Screen.height);
		mega_man = GameObject.Find ("Mega Man");
		metal_man = GameObject.Find ("Metal Man");
		Vector3 end_of_hand = metal_man.transform.position;		
		// metal man is facing forwards
		if (metal_man.transform.eulerAngles.y != 0) {
			end_of_hand.x += -.5f;
		} else {
			end_of_hand.x += .5f;
		}
		transform.position = end_of_hand;

		y_rate = ((mega_man.transform.position.y - end_of_hand.y) / 1.3f);
		x_rate = ((mega_man.transform.position.x - end_of_hand.x) / 1.3f);

	}
	
	void Start () {
	}

	void Update(){
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

