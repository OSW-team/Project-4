using UnityEngine;
using System.Collections;

public class AdwancedTankController : SimpleController{

	NavMeshAgent agent;
	VehicleParent VP;
	public GearboxTransmission[] transMissions;

	public override void Steering(Vector2 prefVelocity, float maxSpeed)//Это интерпретация вектора в руление (он передаётся из другого скрипта, где ИИ реализован)
	{
		//Debug.DrawRay (transform.position + new Vector3 (0, 10, 0), new Vector3(prefVelocity.x, 0, prefVelocity.y));
		if (UnityEngine.Vector2.Angle(new UnityEngine.Vector2(transform.forward.x, transform.forward.z), new UnityEngine.Vector2(prefVelocity.x, prefVelocity.y)) >= maxSteeringAngle)
		{
			steering = -Mathf.Sign(UnityEngine.Vector2.Dot(new UnityEngine.Vector2(transform.right.x, transform.right.z), new UnityEngine.Vector2(prefVelocity.x, prefVelocity.y)));
		}
		else
		{
			steering = -Mathf.Sign(UnityEngine.Vector2.Dot(new UnityEngine.Vector2(transform.right.x, transform.right.z), new UnityEngine.Vector2(prefVelocity.x, prefVelocity.y))) * UnityEngine.Vector2.Angle(new UnityEngine.Vector2(transform.forward.x, transform.forward.z), new UnityEngine.Vector2(prefVelocity.x, prefVelocity.y)) / maxSteeringAngle;
		}

		if (UnityEngine.Vector2.Angle(new UnityEngine.Vector2(transform.forward.x, transform.forward.z), new UnityEngine.Vector2(prefVelocity.x, prefVelocity.y)) > 175)
		{
			motor = -maxMotorTorque * prefVelocity.magnitude /  maxSpeed;
		}
		else if (UnityEngine.Vector2.Angle(new UnityEngine.Vector2(transform.forward.x, transform.forward.z), new UnityEngine.Vector2(prefVelocity.x, prefVelocity.y)) > 80)
		{
			motor = maxMotorTorque */*0.8f */ prefVelocity.magnitude /  maxSpeed;
		}
		else
		{
			motor = maxMotorTorque * prefVelocity.magnitude /  maxSpeed;
		}
		if (rigBody != null && Vector3.Dot(rigBody.velocity,  transform.forward) < 0)
		{
			steering = -steering;
		}
	}

	void Start(){
		agent = GetComponentInChildren<NavMeshAgent> ();
		VP = GetComponentInParent<VehicleParent> ();//берем контроллер
		rigBody = GetComponent<Rigidbody> ();
	}

	void FixedUpdate()
	{
		agent.transform.localPosition = new Vector3 (0, agent.transform.localPosition.y, 0);

		if ((transform.position - agent.destination).sqrMagnitude < stopDistance * stopDistance) {
			VP.SetAccel (0);//газуем
			VP.SetBrake (0);
		} else if (steering < 0.1 && steering > -0.1) {
			transMissions [0].currentGear = 1;
			transMissions [1].currentGear = 1;
			VP.SetAccel (motor / maxMotorTorque);//газуем
			VP.SetBrake (0);
			//Debug.Log ("forward "+transMissions [0].currentGear + " " +transMissions [1].currentGear);

		} else if (motor < 0) {
			transMissions [0].currentGear = 0;
			transMissions [1].currentGear = 0;
			VP.SetAccel (0);
			VP.SetBrake (-motor / maxMotorTorque);
			//Debug.Log ("backward"+transMissions [0].currentGear + " " +transMissions [1].currentGear);

		} else if (steering > 0) {
			VP.SetAccel (motor / maxMotorTorque);//газуем
			VP.SetBrake (0);
			transMissions [0].currentGear = 1;
			transMissions [1].currentGear = 0;
			//Debug.Log ("left"+transMissions [0].currentGear + " " +transMissions [1].currentGear);

		} else {
			VP.SetAccel (motor / maxMotorTorque);//газуем
			VP.SetBrake (0);
			transMissions [0].currentGear = 0;
			transMissions [1].currentGear = 1;
			//Debug.Log ("right"+transMissions [0].currentGear + " " +transMissions [1].currentGear);
		}
		/*
		if (motor > 0) {

			VP.SetAccel (motor / maxMotorTorque);//газуем
			VP.SetBrake (0);
		} else {
			VP.SetAccel (0);
			VP.SetBrake (-motor / maxMotorTorque);
		}*/

	}
}
