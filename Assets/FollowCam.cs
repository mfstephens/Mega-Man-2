using UnityEngine;
using System.Collections;

public class FollowCam : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

		// Get the position of mega man
		Vector3 mega_man_pos = GameObject.Find ("Mega Man").transform.position;
		
		// return if camera tries to follow Mega Man to the left of his starting position
		if (mega_man_pos.x < 0.001) return;
		
		// Set camera's x value to Mega Man's x value
		Vector3 cam_temp = this.transform.position;
		cam_temp.x = mega_man_pos.x;
		this.transform.position = cam_temp;

	}
}
