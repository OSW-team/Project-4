using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OldWCCOntroll : MonoBehaviour {
	public List<AxleInfo> axleInfos; 
	public float maxMotorTorque;
	public float maxSteeringAngle;
	public bool tank;
	public Transform tail;
	public float SteerSpeed;
	float currentSteer;

	// finds the corresponding visual wheel
	// correctly applies the transform
	public void ApplyLocalPositionToVisuals(WheelCollider collider)
	{
		if (collider.transform.childCount == 0) {
			return;
		}

		Transform visualWheel = collider.transform.GetChild(0);

		Vector3 position;
		Quaternion rotation;
		collider.GetWorldPose(out position, out rotation);

		visualWheel.transform.position = position;
		visualWheel.transform.rotation = rotation;
	}

	public void FixedUpdate()
	{
		if (tank) {
			float motor = maxMotorTorque;
			if (Input.GetKey (KeyCode.L)) {
				foreach (AxleInfo axleInfo in axleInfos) {
					if (Input.GetKey (KeyCode.I)) {
						axleInfo.leftWheel.motorTorque = motor;
						axleInfo.rightWheel.motorTorque = -motor;
					} else if (Input.GetKey (KeyCode.K)) {
						axleInfo.leftWheel.motorTorque = motor;
						axleInfo.rightWheel.motorTorque = -motor;
					} else {
						axleInfo.leftWheel.motorTorque = motor;
						axleInfo.rightWheel.motorTorque = -motor;
					}

					ApplyLocalPositionToVisuals (axleInfo.leftWheel);
					ApplyLocalPositionToVisuals (axleInfo.rightWheel);
				}
			} else if (Input.GetKey (KeyCode.J)) {
				foreach (AxleInfo axleInfo in axleInfos) {
					axleInfo.leftWheel.motorTorque = -motor;
					axleInfo.rightWheel.motorTorque = motor;

					ApplyLocalPositionToVisuals (axleInfo.leftWheel);
					ApplyLocalPositionToVisuals (axleInfo.rightWheel);
				}
			} else if (Input.GetKey (KeyCode.I)) {
				foreach (AxleInfo axleInfo in axleInfos) {
					axleInfo.leftWheel.motorTorque = motor;
					axleInfo.rightWheel.motorTorque = motor;


				}
			} else if (Input.GetKey (KeyCode.K)) {
				foreach (AxleInfo axleInfo in axleInfos) {
					axleInfo.leftWheel.motorTorque = -motor;
					axleInfo.rightWheel.motorTorque = -motor;

					ApplyLocalPositionToVisuals (axleInfo.leftWheel);
					ApplyLocalPositionToVisuals (axleInfo.rightWheel);
				}
			} else {
				foreach (AxleInfo axleInfo in axleInfos) {
					axleInfo.leftWheel.motorTorque = 0;
					axleInfo.rightWheel.motorTorque = 0;

					ApplyLocalPositionToVisuals (axleInfo.leftWheel);
					ApplyLocalPositionToVisuals (axleInfo.rightWheel);
				}
			}


		} else {
			float motor = maxMotorTorque * Input.GetAxis("Vertical");
			float steering = maxSteeringAngle * Input.GetAxis("Horizontal");

			if (Mathf.Abs (currentSteer + Mathf.Sign (steering) * SteerSpeed * Time.deltaTime) < steering * maxSteeringAngle) {// Рулим
				currentSteer = currentSteer + Mathf.Sign (steering) * SteerSpeed * Time.deltaTime;
			} else {
				currentSteer = currentSteer + Mathf.Sign(steering * maxSteeringAngle - currentSteer) * SteerSpeed * Time.deltaTime;
			}
			if(tail != null){
				tail.transform.rotation = Quaternion.LookRotation (transform.forward * Mathf.Cos(currentSteer*Mathf.Deg2Rad) + transform.right * Mathf.Sin(currentSteer*Mathf.Deg2Rad), transform.up);//Вертим хвостом туда, куда рулим.
			}


			foreach (AxleInfo axleInfo in axleInfos) {
				if (axleInfo.steering) {
					axleInfo.leftWheel.steerAngle = currentSteer;
					axleInfo.rightWheel.steerAngle = currentSteer;
				}
				if (axleInfo.motor) {
					axleInfo.leftWheel.motorTorque = motor;
					axleInfo.rightWheel.motorTorque = motor;
				}
				ApplyLocalPositionToVisuals(axleInfo.leftWheel);
				ApplyLocalPositionToVisuals(axleInfo.rightWheel);
			}
		}
	}
}