using UnityEngine;
using System.Collections;

public class FitRotation : MonoBehaviour {
	public Transform[] bridges;
	public float angularSpeed;
	
	// Update is called once per frame
	void Update () {
		Vector3 result = Vector3.zero;
		foreach (Transform bridge in bridges) {
			result += bridge.up;
		}
		transform.rotation = Quaternion.RotateTowards (transform.rotation, Quaternion.LookRotation (Vector3.Cross (transform.right, result), result), angularSpeed*Time.deltaTime);
	
	}
}
