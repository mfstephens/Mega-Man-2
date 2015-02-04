using UnityEngine;
using System.Collections;

public class SpikeWall : MonoBehaviour {

	public float speed = .8f;
	public float risingSpeed = 0.1f;
	public float shakeDuration = 1f;
	float max_fallbehind_dist = 4f;
	private GameObject mega_man;
	Camera main_cam;
	
	// Use this for initialization
	void Start () {
		main_cam = GameObject.Find ("Main Camera").camera;
		mega_man = GameObject.Find ("Mega Man");
	}
	
	// Update is called once per frame
	void Update () {
		if (mega_man.transform.position.x >= (transform.position.x + 1f)) {

				main_cam.GetComponent<FollowCam_Custom>().shake = shakeDuration;
		
			if (transform.position.y <= 0) {
				Vector3 temp = transform.position;
				temp.y += risingSpeed;
				transform.position = temp;
			}
		}
		if (transform.position.y >= -.01) {
			Vector3 temp2 = transform.position;
			if(transform.position.x < mega_man.transform.position.x - max_fallbehind_dist){
				temp2.x = mega_man.transform.position.x - max_fallbehind_dist;
				transform.position = temp2;
			} else {
				temp2.x += speed * Time.deltaTime;
				transform.position = temp2;
			}
		}
	}
}
