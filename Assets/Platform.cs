using UnityEngine;
using System.Collections;

public enum PlatformType {
	forward,
	backward,
	still
}

public class Platform : MonoBehaviour {

	public PlatformType type = PlatformType.still;
	public float speed = .85f;

	// Use this for initialization
	void Start () {
		speed = .85f;
	}
	
	// Update is called once per frame
	void Update () {
			
	}
}
