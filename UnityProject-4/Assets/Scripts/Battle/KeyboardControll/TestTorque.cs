using UnityEngine;
using System.Collections;

public class TestTorque : MonoBehaviour {
	public Wheel[] wheels;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		foreach (Wheel wheel in wheels) {
			wheel.SetActualTorque (1);
		}
	}
}
