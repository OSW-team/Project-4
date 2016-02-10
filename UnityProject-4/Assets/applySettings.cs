using UnityEngine;
using System.Collections;

public class applySettings : MonoBehaviour {
	public Transform everybodey;
	public GameObject prefab;

	void Start(){
		Apply ();
	}


	public void Apply(){
		TestTailSimpleMoving sample = prefab.GetComponent<TestTailSimpleMoving> ();
		foreach (TestTailSimpleMoving unit in everybodey.GetComponentsInChildren<TestTailSimpleMoving>()) {
			unit.maxSpeed = prefab.GetComponent<TestTailSimpleMoving> ().maxSpeed;
			unit.friction = sample.friction;
			unit.breaks = sample.breaks;
			unit.angularSpeed = sample.angularSpeed;
			unit.power = sample.power;

		}
	}
}
