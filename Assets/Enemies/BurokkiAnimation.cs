using UnityEngine;
using System.Collections;

public class BurokkiAnimation : MonoBehaviour {
	GameObject box1, box2, face, box3;
	public float rate = 1f;
	public float move_dist = .1f;
	float start_pos;
	bool g1_forward, g2_forward;
	// Use this for initialization
	void Start () {
		box1 = transform.FindChild ("box1").gameObject;
		box2 = transform.FindChild ("box2").gameObject;
		face = transform.FindChild ("face").gameObject;
		box3 = transform.FindChild ("box3").gameObject;
		g1_forward = false;
		g2_forward = true;
	
	}
	
	// Update is called once per frame
	void Update () {
		start_pos = transform.position.x;
		if (box1.transform.position.x <= start_pos - move_dist) g1_forward = false;
		if (box1.transform.position.x >= start_pos + move_dist) g1_forward = true;
		if (box2.transform.position.x <= start_pos - move_dist) g2_forward = false;
		if (box2.transform.position.x >= start_pos + move_dist) g2_forward = true;

		Vector3 temp1 = box1.transform.position;
		Vector3 temp2 = face.transform.position;
		if(g1_forward){
			temp1.x -= rate* Time.deltaTime;
			temp2.x -= rate* Time.deltaTime;
		} else {
			temp1.x += rate* Time.deltaTime;
			temp2.x += rate* Time.deltaTime;
		}
		box1.transform.position = temp1;
		face.transform.position = temp2;

		Vector3 temp3 = box2.transform.position;
		Vector3 temp4 = box3.transform.position;
		if(g2_forward){
			temp3.x -= rate* Time.deltaTime;
			temp4.x -= rate* Time.deltaTime;
		} else {
			temp3.x += rate* Time.deltaTime;
			temp4.x += rate* Time.deltaTime;
		}
		box2.transform.position = temp3;
		box3.transform.position = temp4;
	}
}
