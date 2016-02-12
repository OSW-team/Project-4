using UnityEngine;
using System.Collections;

public class FitPosition : MonoBehaviour {
	public Transform parentPosition;
	// Update is called once per frame
	void Update () {
		transform.position = parentPosition.position;
	}
}
