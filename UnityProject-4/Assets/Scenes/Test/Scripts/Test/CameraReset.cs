using UnityEngine;
using System.Collections;

public class CameraReset : MonoBehaviour {
	Vector3 start;
	void Start(){
		start = transform.position;
	}

	public void CameraResetPosition(){
		transform.position = start;
	}
}
