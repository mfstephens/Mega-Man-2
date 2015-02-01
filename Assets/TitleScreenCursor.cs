using UnityEngine;
using UnityEditor;
using System.Collections;

public enum levelType {
	classicLevel,
	customLevel
}

public class TitleScreenCursor : MonoBehaviour {

	public levelType type = levelType.classicLevel;
	private float scalingFactor = 15.0f;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if ((Input.GetKey("up") || Input.GetKey("w")) && (type != levelType.classicLevel)) {
			type = levelType.classicLevel;
			Vector3 temp = this.GetComponent<RectTransform>().position;
			temp.y += (Screen.height / scalingFactor);
			this.GetComponent<RectTransform>().position = temp;
		} else if ((Input.GetKey("down") || Input.GetKey("s")) && (type != levelType.customLevel)) {
			type = levelType.customLevel;
			Vector3 temp = this.GetComponent<RectTransform>().position;
			temp.y -= (Screen.height / scalingFactor);
			this.GetComponent<RectTransform>().position = temp;
		} else if (Input.GetKey ("enter") || Input.GetKey ("return")) {
			if (type == levelType.customLevel) {
				startCustomLevel();
			} else if (type == levelType.classicLevel) {
				startClassicLevel();
			}
		}
	}

	void startClassicLevel() {
		Application.LoadLevel (1);

	}

	void startCustomLevel() {
		Application.LoadLevel (2);
	}
}
