using UnityEngine;
using System.Collections;

public class MovingPillar : MonoBehaviour {

	public float speed = 0.1f;
	private float startY;
	private float endY;
	public float distance = 5.0f;
	PE_Dir direction = PE_Dir.down;

	// Use this for initialization
	void Start () {
		startY = transform.position.y;
		endY = startY + distance;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 temp = transform.position;

		if (temp.y >= endY) {
			print ("down");
			direction = PE_Dir.down;
		} else if (temp.y <= startY) {
			print ("up");
			direction = PE_Dir.up;
		}

		if (direction == PE_Dir.down) {
			temp.y -= speed;
		} else if (direction == PE_Dir.up) {
			temp.y += speed;
		}

		transform.position = temp;
	}
}
