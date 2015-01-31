using UnityEngine;
using System.Collections;

public class Spin : MonoBehaviour {
	public bool is_left = true;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if(is_left) transform.Rotate (Vector3.forward * Time.deltaTime * 110);
		if(!is_left) transform.Rotate (Vector3.back * Time.deltaTime * 110);
	}
}
