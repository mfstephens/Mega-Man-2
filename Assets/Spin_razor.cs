﻿using UnityEngine;
using System.Collections;

public class Spin_razor : MonoBehaviour {
	public bool is_left = true;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if(is_left) transform.Rotate (Vector3.forward * Time.deltaTime * 720);
		if(!is_left) transform.Rotate (Vector3.back * Time.deltaTime * 720);
	}
}
