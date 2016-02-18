using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
	public GameObject target;
	public Vector3 shift;
	public float sensivity;
	float deg1 = 0, deg2 = 0, deg3 = 0;
	// Use this for initialization
	void Start () {
		shift = transform.position - target.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = target.transform.position + shift;

		float mouseShiftY = Input.mousePosition.y - Screen.height / 2;
		float mouseShiftX = Input.mousePosition.x - Screen.width / 2;


		Debug.Log (ToSphere (shift).x + " " + ToSphere (shift).y * Mathf.Rad2Deg + " " + ToSphere (shift).z * Mathf.Rad2Deg) ;

		transform.rotation = Quaternion.RotateTowards (transform.rotation, Quaternion.LookRotation (target.transform.position - transform.position), 180*Time.deltaTime);

		deg1 += sensivity * Mathf.Sign(mouseShiftX)* 
				mouseShiftX*mouseShiftX * Time.deltaTime * Mathf.Deg2Rad;
		deg2 += sensivity * Mathf.Sign(mouseShiftY)* 
			mouseShiftY*mouseShiftY * Time.deltaTime * Mathf.Deg2Rad;
		deg3 += Input.mouseScrollDelta.y;

		//Debug.Log (deg1);
		shift = ToDecart( new Vector3(40 + deg3, Mathf.Deg2Rad * 40 + deg2, deg1));
		//shift = ToDecart (ToSphere (shift) - new Vector3 (0, 0, sensivity * Mathf.Sign(mouseShift)* 
		//	mouseShift*mouseShift * Mathf.Deg2Rad * Time.deltaTime));
	}

	Vector3 ToSphere(Vector3 Decart){
		Vector3 Sphere = new Vector3 (shift.magnitude, Mathf.Acos (shift.y / shift.magnitude), Mathf.Atan (shift.x/shift.z));
		return Sphere;
	}
	Vector3 ToDecart(Vector3 Sphere){
		Vector3 Decart = new Vector3 (Sphere.x * Mathf.Sin(Sphere.y)*Mathf.Sin(Sphere.z), Sphere.x * Mathf.Cos(Sphere.y),  Sphere.x * Mathf.Sin(Sphere.y)*Mathf.Cos(Sphere.z));
		return Decart;
	}
}
