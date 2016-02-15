using UnityEngine;
using System.Collections;

public abstract class TranslateController : MonoBehaviour {
	public float stopDistance;
	public Transform massCenter, tail;
	public float steer, gas;
	public float slowTurnSpeed = 10, speed = 0, maxsteeringAngle = 20, power = 100, breaks = 100, friction = 0.01f, maxSpeed = 100;

	public abstract void Steering(Vector2 prefVelocity, float maxSpeed);

}
