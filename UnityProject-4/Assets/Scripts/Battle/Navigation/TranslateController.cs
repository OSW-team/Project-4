using UnityEngine;
using System.Collections;

public abstract class TranslateController : MonoBehaviour {
	public float stopDistance;
	public Transform massCenter, tail;
	public float steer, gas;
	public abstract void Steering(Vector2 prefVelocity, float maxSpeed);

}
