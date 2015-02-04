using UnityEngine;
using System.Collections;

public class MovingPillar : MonoBehaviour {

	public float speed = 0.1f;
	public float startY = -3.8f;
	public float endY = -1.8f;
	public PE_Dir direction;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 temp = transform.position;

		if (temp.y >= endY) {
			direction = PE_Dir.down;
		} else if (temp.y <= startY) {
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
