using UnityEngine;
using System.Collections;

public class HoverFanera : MonoBehaviour {
	public float angularSpeed, speed, springspeed, suspensionLength, suspensionMin, jumpSpeed;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		RaycastHit[] hit = new RaycastHit[4];
		Ray rayRF, rayRB, rayLF, rayLB;
		rayRF = new Ray(
			transform.position 
			+ transform.forward*(transform.localScale.x / 2)
			+ transform.right*(transform.localScale.z / 2),
			-Vector3.up);
		rayLF = new Ray(
			transform.position 
			+ transform.forward*(transform.localScale.x / 2) 
			- transform.right*(transform.localScale.z / 2),
			-Vector3.up);
		rayRB = new Ray(
			transform.position
			- transform.forward*(transform.localScale.x / 2)
			+ transform.right*(transform.localScale.z / 2),
			-Vector3.up);
		rayLB = new Ray(
			transform.position 
			- transform.forward*(transform.localScale.x / 2) 
			- transform.right*(transform.localScale.z / 2),
			-Vector3.up);

		Debug.DrawRay (rayRF.origin, rayRF.direction * 20, Color.black);
		Debug.DrawRay (rayLF.origin, rayLF.direction * 20, Color.black);
		Debug.DrawRay (rayRB.origin, rayRB.direction * 20, Color.black);



		if (!Physics.Raycast (rayRF, out hit [0])) {
			hit [0].distance = -100;
			transform.position += Vector3.up * jumpSpeed * Time.deltaTime;
		}
		if (!Physics.Raycast(rayRB, out hit[1]) ) {
			hit [1].distance = -100;
			transform.position +=Vector3.up * jumpSpeed * Time.deltaTime;
		}
		if (!Physics.Raycast(rayLF, out hit[2]) ) {
			hit [1].distance = -100;
			transform.position +=Vector3.up * jumpSpeed * Time.deltaTime;
		}
		if (!Physics.Raycast(rayLB, out hit[3])) {
			hit [1].distance = -100;
			transform.position += Vector3.up *jumpSpeed * Time.deltaTime;
		}
		Vector3 upward = new Vector3();
		float maxDist = Mathf.Max (hit [0].distance, hit [1].distance, hit [2].distance, hit [3].distance);
		float minDist = Mathf.Min (hit [0].distance, hit [1].distance, hit [2].distance, hit [3].distance);
		if (maxDist == hit [0].distance) {
			upward = Vector3.Cross (hit [1].point - hit [3].point, hit [2].point - hit [3].point).normalized;
		}
		else if (maxDist == hit [1].distance) {
			upward = Vector3.Cross (hit [0].point - hit [2].point, hit [3].point - hit [2].point).normalized;
		}
		else if (maxDist == hit [2].distance) {
			upward = Vector3.Cross (hit [0].point - hit [1].point, hit [3].point - hit [1].point).normalized;
		}
		else {
			upward = Vector3.Cross (hit [1].point - hit [0].point, hit [2].point - hit [0].point).normalized;
		}


		if(Vector3.Dot(upward, transform.up) < 0 ){
			upward *= -1;
		}


		Vector3 r = Vector3.Cross (upward, transform.forward);
		Vector3 r1 = Vector3.Cross (r, upward);
		Vector3 r2 = Vector3.Cross (r1, r);

		Debug.DrawRay (transform.position, r * 20,Color.red);
		Debug.DrawRay (transform.position, r1 * 20,Color.blue);
		Debug.DrawRay (transform.position, r2 * 20, Color.green);


		//transform.position += transform.forward * Input.GetAxis ("Vertical") * speed * Time.deltaTime;
		transform.rotation = Quaternion.RotateTowards (transform.rotation, Quaternion.LookRotation (r1, r2), springspeed*Time.deltaTime);
		//transform.rotation = Quaternion.RotateTowards (transform.rotation, Quaternion.LookRotation (transform.right, transform.up), angularSpeed * Time.deltaTime * Input.GetAxis ("Horizontal"));
		if (minDist > suspensionLength) {
			transform.position += Physics.gravity * Time.deltaTime;
		}
		if (minDist < suspensionMin) {
			transform.position += transform.up * jumpSpeed * Time.deltaTime;
		}
	}
}
