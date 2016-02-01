using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class AdwancedCarController : SimpleController
{
	VehicleParent VP;//Ассетик
	public float currentSteer; //Для плавного поворота хвоста
	public float SteerSpeed;//Скорость поворота хвоста
	public override void Steering(Vector2 prefVelocity, float maxSpeed)//Это интерпретация вектора в руление (он передаётся из другого скрипта, где ИИ реализован)
	{
		Debug.DrawRay (transform.position + new Vector3 (0, 10, 0), new Vector3(prefVelocity.x, 0, prefVelocity.y));
		if (UnityEngine.Vector2.Angle(new UnityEngine.Vector2(transform.forward.x, transform.forward.z).normalized, new UnityEngine.Vector2(prefVelocity.x, prefVelocity.y).normalized) >= maxSteeringAngle)
		{
			steering = -Mathf.Sign(UnityEngine.Vector2.Dot(new UnityEngine.Vector2(transform.right.x, transform.right.z).normalized, new UnityEngine.Vector2(prefVelocity.x, prefVelocity.y).normalized));
		}
		else
		{
			steering = -Mathf.Sign(UnityEngine.Vector2.Dot(new UnityEngine.Vector2(transform.right.x, transform.right.z).normalized, new UnityEngine.Vector2(prefVelocity.x, prefVelocity.y).normalized)) * UnityEngine.Vector2.Angle(new UnityEngine.Vector2(transform.forward.x, transform.forward.z).normalized, new UnityEngine.Vector2(prefVelocity.x, prefVelocity.y).normalized) / maxSteeringAngle;
		}

		if (UnityEngine.Vector2.Angle(new UnityEngine.Vector2(transform.forward.x, transform.forward.z).normalized, new UnityEngine.Vector2(prefVelocity.x, prefVelocity.y).normalized) > 110)
		{
			motor = -maxMotorTorque * prefVelocity.magnitude /  maxSpeed;
		}
		else if (UnityEngine.Vector2.Angle(new UnityEngine.Vector2(transform.forward.x, transform.forward.z).normalized, new UnityEngine.Vector2(prefVelocity.x, prefVelocity.y).normalized) > 80)
		{
			motor = maxMotorTorque * prefVelocity.magnitude /  maxSpeed;
		}
		else
		{
			motor = maxMotorTorque / 2 * prefVelocity.magnitude /  maxSpeed;
		}
		if (rigBody != null && Vector3.Dot(rigBody.velocity,  transform.forward) < 0)
		{
			steering = -steering;
		}
	}

	void Start(){
		VP = GetComponentInParent<VehicleParent> ();//берем контроллер
		rigBody = GetComponent<Rigidbody> ();
		currentSteer = 0;
	}

	void FixedUpdate()
	{
		//rigBody.centerOfMass = massCenter.localPosition;  //Двигаем центр масс в нужную точку. От этого любим летать в космос
		if (Mathf.Abs (currentSteer + Mathf.Sign (steering) * SteerSpeed * Time.deltaTime) < steering * maxSteeringAngle) {// Рулим
			currentSteer = currentSteer + Mathf.Sign (steering) * SteerSpeed * Time.deltaTime;
		} else {
			currentSteer = currentSteer + Mathf.Sign(steering * maxSteeringAngle - currentSteer) * SteerSpeed * Time.deltaTime;
		}

		tail.transform.rotation = Quaternion.LookRotation (transform.forward * Mathf.Cos(currentSteer*Mathf.Deg2Rad) + transform.right * Mathf.Sin(currentSteer*Mathf.Deg2Rad), transform.up);//Вертим хвостом туда, куда рулим.
		if (motor > 0) {

			VP.SetAccel (motor / maxMotorTorque);//газуем
		} else {
			VP.SetBrake (-motor / maxMotorTorque);
		}
			
	}

}