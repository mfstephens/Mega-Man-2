using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HealthBarBoss : MonoBehaviour {
	
	public GameObject healthbarUnitPrefabBoss;
	public Stack<GameObject> hpUnits;
	public int totalHealthUnits = 28;
	public float distanceBetweenUnits = 3f;
	public float topUnitPosition = -43.5f;
	
	// Use this for initialization
	void start () {
		hpUnits = new Stack<GameObject>();
		for (int i = 0; i < totalHealthUnits; ++i) {
			increaseByOne();
		}
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	public void increaseByOne() {
		if (hpUnits.Count >= totalHealthUnits) {
			return;
		}
		
		GameObject hb = GameObject.Find ("Health Bar Boss");
		
		// create and add a new health unit to bar
		GameObject go = Instantiate(healthbarUnitPrefabBoss) as GameObject;
		go.transform.SetParent(hb.transform, false);
		
		
		Vector2 temp = go.GetComponent<RectTransform> ().anchoredPosition;
		temp.y = topUnitPosition + distanceBetweenUnits;
		topUnitPosition = temp.y;
		go.GetComponent<RectTransform> ().anchoredPosition = temp;
		hpUnits.Push(go);
	}
	
	public void decreaseByOne() {
		if (hpUnits.Count > 0) {
			GameObject removed = hpUnits.Pop();
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
		while (hpUnits.Count != 0) {
			decreaseByOne();
		}
	}
	
	public void fill() {
		for (int i = 0; i < totalHealthUnits; ++i) {
			increaseByOne();
		}
	}
}
