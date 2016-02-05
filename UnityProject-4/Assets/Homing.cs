using UnityEngine;
using System.Collections;

public class Homing : MonoBehaviour {
	public Transform target;
	public float power, acceleration,angularSpeed, startforce = 0, wings;
	Rigidbody rigBody;

	// Use this for initialization
	void Start () {
		rigBody = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
		transform.rotation = Quaternion.RotateTowards (transform.rotation, Quaternion.LookRotation (target.position - transform.position), angularSpeed * Time.deltaTime);
		startforce = Mathf.Min (startforce + acceleration * Time.deltaTime, power);
		rigBody.velocity = Vector3.zero;
		rigBody.AddForce (transform.forward * startforce);
	
		//Debug.DrawRay (transform.position,transform.forward * startforce);
	}
}
