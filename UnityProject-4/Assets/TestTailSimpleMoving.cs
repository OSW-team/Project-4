using UnityEngine;
using System.Collections;

public class TestTailSimpleMoving : MonoBehaviour {

	public float slowTurnSpeed = 10, speed = 0, angularSpeed = 20, axeleration = 5, power = 100, breaks = 100, friction = 0.01f, maxSpeed = 100;
	
	// Update is called once per frame
	void Update () {
		friction = power / (maxSpeed * maxSpeed);
		float gas = Input.GetAxis ("Vertical");
		float steer = Input.GetAxis ("Horizontal");
		Gas (gas);
		transform.position += speed * Time.deltaTime * transform.forward;
		transform.rotation = Quaternion.RotateTowards (transform.rotation,  Quaternion.LookRotation (Mathf.Sign(speed) * transform.right), Steering(steer) * Time.deltaTime);
		Friction (friction);
	}

	float Gas(float gas){
		if (gas > 0) {
			if (speed >= 0) {
				speed += gas * power * Time.deltaTime;
			} else {
				speed += gas * power * Time.deltaTime * breaks;
			}
		} else if (gas < 0) {
			if (speed >= 0) {
				speed += gas * power * Time.deltaTime * breaks;
			} else {
				speed += gas * power * Time.deltaTime;
			}
		}
		return speed;
	}

	float Steering(float steer){
		if (Mathf.Abs(speed) <  slowTurnSpeed) {
			return Mathf.Abs (speed) / slowTurnSpeed * angularSpeed * steer;
		} else {
			return  steer * angularSpeed;
		}
	}

	void Friction(float k){
		if (Mathf.Abs(speed) > 1) {
			speed -= Mathf.Sign (speed) * speed * speed * k;
		} else {
			speed -= speed * k;
		}
	}

}
