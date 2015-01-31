using UnityEngine;
using System.Collections;

public class PieroBot_flip : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void flip(){
		Vector3 temp = transform.eulerAngles;
		if (temp.y == 0)temp.y = 180;
		else temp.y = 0;

		transform.eulerAngles = temp;

	}
}
