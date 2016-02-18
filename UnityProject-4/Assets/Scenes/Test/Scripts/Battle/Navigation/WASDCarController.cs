using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WASDCarController : SimpleController {

	public List<AxleInfo> axleInfos;
	public float steeringstep = 1f;

	void Update(){
		rigBody.centerOfMass = massCenter.transform.localPosition;
		Steering (Vector3.zero, 0);
	}
	public override void Steering(Vector2 prefVelocity, float maxSpeed)
	{	
		
		if (Input.GetKey (KeyCode.W)) {
			foreach (AxleInfo axleInfo in axleInfos) {
				if (axleInfo.motor) {
					axleInfo.leftWheel.brakeTorque = 0;
					axleInfo.rightWheel.brakeTorque = 0;
					axleInfo.leftWheel.motorTorque = maxMotorTorque;
					axleInfo.rightWheel.motorTorque = maxMotorTorque;
				}
			}
		} else if (Input.GetKey (KeyCode.S)) {
			if (Vector3.Dot (rigBody.velocity, transform.forward) > 0) {
				foreach (AxleInfo axleInfo in axleInfos) {
					if (axleInfo.motor) {
						axleInfo.leftWheel.brakeTorque = maxBreaksTorque;
						axleInfo.rightWheel.brakeTorque = maxBreaksTorque;
					}
				}
			} else {
				foreach (AxleInfo axleInfo in axleInfos) {
					if (axleInfo.motor) {
						axleInfo.leftWheel.brakeTorque = 0;
						axleInfo.rightWheel.brakeTorque = 0;
						axleInfo.leftWheel.motorTorque = -maxMotorTorque / 2;
						axleInfo.rightWheel.motorTorque = -maxMotorTorque / 2;
					}
				}
			}
		} else {
			foreach (AxleInfo axleInfo in axleInfos) {
				if (axleInfo.motor) {
					axleInfo.leftWheel.brakeTorque = 0;
					axleInfo.rightWheel.brakeTorque = 0;
					axleInfo.leftWheel.motorTorque = 0;
					axleInfo.rightWheel.motorTorque = 0;
				}
			}
			
		}
		if (Input.GetKey (KeyCode.A)) {
			foreach (AxleInfo axleInfo in axleInfos) {
				if (axleInfo.steering) {
					axleInfo.leftWheel.steerAngle = Mathf.Min (axleInfo.leftWheel.steerAngle + steeringstep * Time.deltaTime, maxSteeringAngle);
					axleInfo.rightWheel.steerAngle = Mathf.Min (axleInfo.rightWheel.steerAngle + steeringstep * Time.deltaTime, maxSteeringAngle);
				}
			}
		} else if (Input.GetKey (KeyCode.D)) {
			foreach (AxleInfo axleInfo in axleInfos) {
				if (axleInfo.steering) {
					axleInfo.leftWheel.steerAngle = Mathf.Max (axleInfo.leftWheel.steerAngle - steeringstep * Time.deltaTime, -maxSteeringAngle);
					axleInfo.rightWheel.steerAngle = Mathf.Max (axleInfo.rightWheel.steerAngle - steeringstep * Time.deltaTime, -maxSteeringAngle);

				}
			}
		} else {
			foreach (AxleInfo axleInfo in axleInfos) {
				if (axleInfo.steering) {
					if (axleInfo.leftWheel.steerAngle < 0) {
						axleInfo.leftWheel.steerAngle = Mathf.Min (axleInfo.leftWheel.steerAngle + steeringstep * Time.deltaTime, 0);
						axleInfo.rightWheel.steerAngle = Mathf.Min (axleInfo.leftWheel.steerAngle + steeringstep * Time.deltaTime, 0);
					} else {
						axleInfo.leftWheel.steerAngle = Mathf.Max (axleInfo.leftWheel.steerAngle - steeringstep * Time.deltaTime, 0);
						axleInfo.rightWheel.steerAngle = Mathf.Max (axleInfo.leftWheel.steerAngle - steeringstep * Time.deltaTime, 0);

					}

				}
			}
		}

	}
}
