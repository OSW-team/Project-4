using UnityEngine;
using System.Collections;

public class Homing : MonoBehaviour {
	public Transform target;
	public float power, acceleration,angularSpeed, startforce = 0, wings, force;
	Rigidbody rigBody;

	// Use this for initialization
	void Start () {
		rigBody = GetComponent<Rigidbody> ();
		force = power / Time.fixedDeltaTime;
	}
	
	// Update is called once per frame
	void Update () {
		if (target != null) {
			transform.rotation = Quaternion.RotateTowards (transform.rotation, Quaternion.LookRotation (target.position - transform.position), angularSpeed * Time.deltaTime);
		} else {
			transform.rotation = Quaternion.RotateTowards (transform.rotation, (Random.rotation), angularSpeed * Time.deltaTime);
		}
			startforce = Mathf.Min (startforce + acceleration * Time.deltaTime, force);
			rigBody.velocity = Vector3.zero;
			rigBody.AddForce (transform.forward * startforce);
		
		//Debug.DrawRay (transform.position,transform.forward * startforce);
	}
}
