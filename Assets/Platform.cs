using UnityEngine;
using System.Collections;

public enum PlatformType {
	forward,
	backward,
	still
}

public class Platform : MonoBehaviour {

	public PlatformType type = PlatformType.still;
	public float speed = 1.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
			
	}
}
