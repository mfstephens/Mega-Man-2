using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HealthBar : MonoBehaviour {

	public GameObject healthbarUnitPrefab;
	public Stack<GameObject> healthUnits;
	public int numHealthUnits = 28;
	public float distanceBetweenUnits = 3f;

	// Use this for initialization
	void Start () {
		healthUnits = new Stack<GameObject>();
		for (int i = 0; i < numHealthUnits; ++i) {
			increaseByOne();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown("o")) {
			decreaseByOne();
		} else if (Input.GetKeyDown("i")) {
			increaseByOne();
		}
	}

	public void increaseByOne() {
		if (healthUnits.Count >= numHealthUnits) return;

		GameObject hb = GameObject.Find ("Health Bar");

		// Get the distance from top health unit to bottom of bar
		float offset = -(hb.GetComponent<RectTransform>().sizeDelta.y / 4.0f) + (distanceBetweenUnits * healthUnits.Count);

		// create and add a new health unit to bar
		GameObject go = Instantiate(healthbarUnitPrefab) as GameObject;
		go.transform.SetParent(hb.transform);
		Vector3 temp = go.transform.position;
		temp = this.transform.position;
		temp.y += offset;
		go.transform.position = temp;
		healthUnits.Push(go);
	}

	public void decreaseByOne() {
		if (healthUnits.Count > 0) {
			GameObject removed = healthUnits.Pop();
			Destroy(removed);
		}
	}
}
