using UnityEngine;
using System.Collections;

public class TranslateCarController : TranslateController {
	public float slowTurnSpeed = 10, speed = 0, maxsteeringAngle = 20, power = 100, breaks = 100, friction = 0.01f, maxSpeed = 100;

	void Update(){
		
		Gas (gas);
		SpeedSteer (steer);
		transform.position += speed * Time.deltaTime * transform.forward;
		transform.rotation = Quaternion.RotateTowards (transform.rotation,  Quaternion.LookRotation (Mathf.Sign(speed) * transform.right), SpeedSteer(steer) * Time.deltaTime);
		Friction (friction);
	}


	public override void Steering(Vector2 prefVelocity, float maxSpeed){
		
		if (UnityEngine.Vector2.Angle(new UnityEngine.Vector2(transform.forward.x, transform.forward.z).normalized, new UnityEngine.Vector2(prefVelocity.x, prefVelocity.y).normalized) >= maxsteeringAngle)
		{
			steer =  Mathf.Sign(UnityEngine.Vector2.Dot(new UnityEngine.Vector2(transform.right.x, transform.right.z).normalized, new UnityEngine.Vector2(prefVelocity.x, prefVelocity.y).normalized));
		}
		else
		{
			steer = Mathf.Sign(UnityEngine.Vector2.Dot(new UnityEngine.Vector2(transform.right.x, transform.right.z).normalized, new UnityEngine.Vector2(prefVelocity.x, prefVelocity.y).normalized)) * UnityEngine.Vector2.Angle(new UnityEngine.Vector2(transform.forward.x, transform.forward.z).normalized, new UnityEngine.Vector2(prefVelocity.x, prefVelocity.y).normalized) / maxsteeringAngle;
		}
		steer = Mathf.Sign (speed) * steer;
		if (UnityEngine.Vector2.Angle(new UnityEngine.Vector2(transform.forward.x, transform.forward.z).normalized, new UnityEngine.Vector2(prefVelocity.x, prefVelocity.y).normalized) > 110)
		{
			gas = -1 * Mathf.Sqrt(prefVelocity.x * prefVelocity.x + prefVelocity.y * prefVelocity.y) /  maxSpeed;
		}
		else if (UnityEngine.Vector2.Angle(new UnityEngine.Vector2(transform.forward.x, transform.forward.z).normalized, new UnityEngine.Vector2(prefVelocity.x, prefVelocity.y).normalized) > 80)
		{
			gas = 1 * Mathf.Sqrt(prefVelocity.x * prefVelocity.x + prefVelocity.y * prefVelocity.y) /  maxSpeed;
		}
		else
		{
			gas = (1.0f / 2.0f) * Mathf.Sqrt(prefVelocity.x * prefVelocity.x + prefVelocity.y * prefVelocity.y) /  maxSpeed;
		}
	}

	void Start(){
		friction = power / (maxSpeed * maxSpeed);
		//slowTurnSpeed = maxSpeed / 5;
		//text = GameObject.Find ("Log").GetComponent<guiText>();

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

	float SpeedSteer(float steer){
		if (Mathf.Abs(speed) <  slowTurnSpeed) {
			return Mathf.Abs (speed) / slowTurnSpeed * maxsteeringAngle * steer;
		} else {
			return  steer * maxsteeringAngle;
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
