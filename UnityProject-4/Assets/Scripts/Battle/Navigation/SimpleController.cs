using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class AxleInfo
{
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor;
    public bool steering;
}

[System.Serializable]
public class TrackInfo
{
    public List<WheelCollider> leftTrack;
    public List<WheelCollider> rightTrack;
}
public abstract class SimpleController : MonoBehaviour {
    public float maxMotorTorque;
    public float maxBreaksTorque;
    public float maxSteeringAngle;
	public float stopDistance;
    public Transform massCenter, tail;
    public float steering, motor;
    protected Rigidbody rigBody;
    public abstract void Steering(Vector2 prefVelocity, float maxSpeed);
    protected void Start()
    {
        rigBody = GetComponent<Rigidbody>();
    }
    
    
}
