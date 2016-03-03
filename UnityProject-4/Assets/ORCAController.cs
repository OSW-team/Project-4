using UnityEngine;
using System.Collections;

public class ORCAController : TranslateController {
	public bool tank = false;
	public float rope, centerRadius = 1, turnRadius = 18;
	BoxMovement move;
	Rigidbody rigBody;
	public float  TailSpeed, radius;
	public Vector3 prefVel;
	private RVO.Agent agent;
	bool inAgent = true;
	StateMachine machine;

	// Use this for initialization
	void Start () {
		machine = GetComponent<StateMachine> ();
		move = GetComponent<BoxMovement> ();
		force = power / Time.fixedDeltaTime;
		rigBody = GetComponentInParent<Rigidbody> ();
	}

	public void AgentChose(int i){
		agent = RVO.Simulator.Instance.agents_ [i];
		Debug.Log (agent);
		//radius = 3;
	}

	void Update(){
		if (Time.fixedDeltaTime > 0) {
			force = power / Time.fixedDeltaTime;
		} 
		if (!move._InAir) {
			Gas (gas);
			agent.maxSpeed_ = maxSpeed;
			transform.rotation = Quaternion.RotateTowards (transform.rotation,  Quaternion.LookRotation (Mathf.Sign(speed) * transform.right), SpeedSteer(steer) * Time.deltaTime);

		} else {
			agent.maxSpeed_ = 0;
		}
		Vector3 onCenter = VectorConvert(agent.position_) -  FromV3toV2 (transform.position);

		if (onCenter.sqrMagnitude > agent.radius_ * agent.radius_*rope*rope) {
			inAgent = false;
		} 
		if (onCenter.sqrMagnitude < 0.01f){
			inAgent = true;
		}

		if (inAgent) {
			Steering (ToV2(VectorConvert (agent.velocity_)) + ToV2(onCenter), agent.maxSpeed_);
			power = maxSpeed * 9.7f;
		} else {
			Steering (ToV2(FromV3toV2( onCenter).normalized*maxSpeed), agent.maxSpeed_);
			power = maxSpeed * 15;
		}
		if (inAgent) {
			agent.radius_ = radius + RadiusCalculation (Vector3.Angle (FromV3toV2 (transform.forward), VectorConvert (agent.velocity_)));
		} else {
			agent.position_ = VectorConvert (transform.position);
			//agent.radius_ = (FromV3toV2 (transform.position) - VectorConvert (agent.position_)).magnitude;
		}

		/*if (onCenter.sqrMagnitude > agent.radius_ * agent.radius_) {
			transform.position += onCenter.normalized * (onCenter.magnitude - agent.radius_);
		}*/
	}

	public float RadiusCalculation(float angle){
		return angle / 180.0f*turnRadius*rigBody.velocity.magnitude/maxSpeed;
	}

	public override void Steering(Vector2 prefVelocity, float maxSpeed){
		Debug.DrawRay (transform.position, new Vector3 (prefVelocity.x, 0, prefVelocity.y), Color.green);
		if (Vector3.Dot (FromV3toV2 (transform.right), new Vector3(prefVelocity.x, 0, prefVelocity.y)) < 0) {
			steer = -1;
		} else {
			steer = 1;
		}
		gas = Mathf.Sqrt(agent.velocity_.x_ * agent.velocity_.x_ + agent.velocity_.y_* agent.velocity_.y_) / agent.maxSpeed_;
		if(Vector2.Angle(ToV2( transform.forward), prefVelocity) > 160){
			if (tank) {
				gas *= -1;
			} else {
				gas = 0;
			}
			if (!inAgent) {
				steer = 0;
			}
			//steer *= -1;
		}
		if (Vector2.Angle (ToV2 (transform.forward), prefVelocity) > 90) {
			agent.maxSpeed_ = maxSpeed;
		}
	}


	float SpeedSteer(float steer){
		if (rigBody!= null && rigBody.velocity.sqrMagnitude <  slowTurnSpeed*slowTurnSpeed) {
			return rigBody.velocity.magnitude / slowTurnSpeed * maxsteeringAngle * steer;
		} else {
			return  steer * maxsteeringAngle;
		}
	}

	void Gas(float gas){
		if (gas > 0) {
			if (Vector3.Dot (rigBody.velocity, transform.forward) >= 0) {
				rigBody.AddForce (transform.forward * gas * 100 * force);
				//speed += gas * power * Time.deltaTime;
			} else {
				rigBody.AddForce (transform.forward * gas * 100 * force );
				//speed += gas * power * Time.deltaTime * breaks;
			}
		} else if (gas < 0) {
			if (Vector3.Dot (rigBody.velocity, transform.forward) >= 0) {
				rigBody.AddForce (transform.forward * gas * 100 * force );
				//speed += gas * power * Time.deltaTime * breaks;
			} else {
				rigBody.AddForce (transform.forward * gas * 100 * force);
				//speed += gas * power * Time.deltaTime;
			}
		}
	}
	public Vector2 ToV2(Vector3 vect){
		return new Vector2 (vect.x, vect.z);
	}

	public Vector3 FromV3toV2(Vector3 dir){
		return new Vector3 (dir.x, 0, dir.z);
	}

	public Vector3 VectorConvert(RVO.Vector2 vector){
		return new Vector3 (vector.x_, 0, vector.y_);
	}
	public RVO.Vector2 VectorConvert (Vector3 vector){
		RVO.Vector2 _vector = new RVO.Vector2 ();
		_vector.x_ = vector.x;
		_vector.y_ = vector.z;
		return _vector;
	}


}
