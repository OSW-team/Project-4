using UnityEngine;
using System.Collections;

public class RaycastMove: MonoBehaviour {
	public float wheelDistance, angularSpeed, fallSpeed, sprignLength, sprign;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		RaycastHit[] hit = new RaycastHit[4];
		Ray rayRF, rayRB, rayLF, rayLB;
		rayRF = new Ray(transform.position + new Vector3(transform.localScale.x / 2, 0,  + transform.localScale.z / 2),-Vector3.up);
		rayLF = new Ray(transform.position + new Vector3(transform.localScale.x / 2, 0, - transform.localScale.z / 2),-Vector3.up);
		rayRB = new Ray(transform.position + new Vector3(-transform.localScale.x / 2, 0, + transform.localScale.z / 2),-Vector3.up);
		rayLB = new Ray(transform.position + new Vector3(-transform.localScale.x / 2, 0, - transform.localScale.z / 2),-Vector3.up);

		if (Physics.Raycast(rayRF, out hit[0]) 
			&& Physics.Raycast(rayRB, out hit[1]) 
			&& Physics.Raycast(rayLF, out hit[2]) 
			&& Physics.Raycast(rayLB, out hit[3])) {

			float Min = Mathf.Min (hit [0].distance, hit [1].distance, hit [2].distance, hit [3].distance);

			Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(Vector3.Cross (hit [0].point - hit [1].point, hit [2].point - hit [1].point)), angularSpeed);
			/*if (Min > wheelDistance) {
				transform.position += Mathf.Min(1, Mathf.Abs(Min/sprignLength - 1)) * fallSpeed * Time.deltaTime * -Vector3.up;
			} else if (Min < wheelDistance) {
				transform.position += Mathf.Min(1, Mathf.Abs(Min/sprignLength - 1)) * sprign * Time.deltaTime * Vector3.up;
			}*/
			//float Min = Mathf.Min(hit [0].point.magnitude, hit [1].point.magnitude, hit [2].point.magnitude, hit [3].point.magnitude);
			//Mathf.Atan (Mathf.Sin (Min));
			//Rigidbody R = GetComponent<Rigidbody> ();
			//Debug.Log (hit [0].distance + " " + hit [1].distance + " " + hit [2].distance + " " + hit [3].distance);
		}
	
	}
}
