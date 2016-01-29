using UnityEngine;
using System.Collections;

public class AISimulation : MonoBehaviour {
	public float maxSpeed;
	public GameObject controller;
	public Transform checkpoint;
	void Update(){// просто даём машинке вектор движения
		Vector2 dir = new Vector2 (checkpoint.position.x - transform.position.x, checkpoint.position.z - transform.position.z);
		controller.GetComponent<SimpleController>(). Steering(dir.normalized * maxSpeed, maxSpeed);
	}
}
