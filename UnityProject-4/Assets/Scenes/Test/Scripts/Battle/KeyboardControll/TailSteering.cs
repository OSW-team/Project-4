using UnityEngine;
using System.Collections;

public class TailSteering : MonoBehaviour {
    public float MaxSteerRange;
    public float SteerAngle;
    public float RotationSpeed;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetKey(KeyCode.LeftArrow)&&transform.localRotation.y<=MaxSteerRange)
        {
            var rot = new Quaternion(transform.localRotation.x, transform.localRotation.y+RotationSpeed, transform.localRotation.z, transform.localRotation.w);
            transform.localRotation = rot;
        }
        if (Input.GetKey(KeyCode.RightArrow) && transform.localRotation.y >= -MaxSteerRange)
        {
            var rot = new Quaternion(transform.localRotation.x, transform.localRotation.y - RotationSpeed, transform.localRotation.z, transform.localRotation.w);
            transform.localRotation = rot;
        }
        //if (transform.localEulerAngles.y > MaxSteerRange)
        //{
        //    var rot = new Vector3(transform.localEulerAngles.x, MaxSteerRange, transform.localEulerAngles.z);
        //    transform.localEulerAngles = rot;
        //}
        //if (transform.localEulerAngles.y < 360 - MaxSteerRange)
        //{
        //    var rot = new Vector3(transform.localEulerAngles.x, -MaxSteerRange, transform.localEulerAngles.z);
        //    transform.localEulerAngles = rot;
        //}
    }
}
