using UnityEngine;
using System.Collections;

public class SpikeWall : MonoBehaviour {

	public float speed = 0.01f;
	public float risingSpeed = 0.1f;
	private MegaMan mega_man;
	private float startingPos;
	
	// Use this for initialization
	void Start () {
		GameObject go = GameObject.Find ("Mega Man");
		mega_man = go.GetComponent<MegaMan> ();
		startingPos = this.transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
		if (mega_man.transform.position.x >= (transform.position.x + 1f)) {
			if (transform.position.y == startingPos) {
				GameObject.Find ("Main Camera").GetComponent<FollowCam>().shake = 10f;
			}
			if (transform.position.y <= 0) {
				Vector3 temp = transform.position;
				temp.y += risingSpeed;
				transform.position = temp;
			}
		}
		if (transform.position.y >= 0) {
			Vector3 temp2 = this.transform.position;
			temp2.x += speed;
			this.transform.position = temp2;
		}
	}
}
