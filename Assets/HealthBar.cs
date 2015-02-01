using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HealthBar : MonoBehaviour {

	public GameObject healthbarUnitPrefab;
	public Stack<GameObject> healthUnits;
	public int totalHealthUnits = 28;
	public float distanceBetweenUnits = 3f;
	public float topUnitPosition = -43.5f;

	// Use this for initialization
	void Start () {
		healthUnits = new Stack<GameObject>();
		for (int i = 0; i < totalHealthUnits; ++i) {
			increaseByOne();
		}
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void increaseByOne() {
		if (healthUnits.Count >= totalHealthUnits) {
			return;
				}

		GameObject hb = GameObject.Find ("Health Bar");

		// create and add a new health unit to bar
		GameObject go = Instantiate(healthbarUnitPrefab) as GameObject;
		go.transform.SetParent(hb.transform, false);


		Vector2 temp = go.GetComponent<RectTransform> ().anchoredPosition;
		temp.y = topUnitPosition + distanceBetweenUnits;
		topUnitPosition = temp.y;
		go.GetComponent<RectTransform> ().anchoredPosition = temp;
		healthUnits.Push(go);
	}

	public void decreaseByOne() {
		if (healthUnits.Count > 0) {
			GameObject removed = healthUnits.Pop();
			topUnitPosition = removed.GetComponent<RectTransform> ().anchoredPosition.y - distanceBetweenUnits;
			Destroy(removed);
		}
	}

	public void increaseByAmount(int amount) {
		for (int i = 0; i < amount; ++i) {
			increaseByOne();
		}
	}

	public void decreaseByAmount(int amount) {
		for (int i = 0; i < amount; ++i) {
			decreaseByOne();
		}
	}

	public void empty() {
		while (healthUnits.Count != 0) {
			decreaseByOne();
		}
	}

	public void fill() {
		for (int i = 0; i < totalHealthUnits; ++i) {
			increaseByOne();
		}
	}
}
