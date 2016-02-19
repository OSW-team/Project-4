using UnityEngine;
using System.Collections;

public class PositionMoving : MonoBehaviour {
	public float speed;
	// Use this for initialization
	
	// Update is called once per frame
	void Update () {
		transform.position  += transform.forward * Input.GetAxis ("Vertical") * Time.deltaTime * speed;
	}
}
