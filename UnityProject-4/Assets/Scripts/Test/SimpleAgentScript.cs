using UnityEngine;
using System.Collections;
using System;

public class SimpleAgentScript : MonoBehaviour {
    public Transform Target;
    public float Speed;
    public float TurnRadius;
    public float Acceleration;
    private NavMeshAgent _agent;
    public float delta = 0.1f;
	public float displacementTolerance;
	public float side;
	Vector3 circleCenter = Vector3.zero;

	Vector3 miniWaypoint;
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();

    }

    void FixedUpdate()
    {
		side = Mathf.Sign (Vector3.Dot (transform.right, _agent.steeringTarget - transform.position));
        _agent.SetDestination(Target.position);
        if (_agent.hasPath)
        {
            var angle = Vector3.Angle(transform.forward, _agent.steeringTarget - transform.position);
            if(angle<1f)
            {
                //Move towards
                MoveForward();
                Debug.Log("Move towards");
            }
            else
            {
                //Turn
				Debug.Log("Turn angle " + angle);
				Turn();
            }
			Debug.DrawLine (transform.position, _agent.steeringTarget, Color.black);
			Debug.DrawRay (transform.position, transform.forward, Color.blue);
        }
    }

	private void Turn()
    {
		
		//Debug.Log (miniWaypoint);
		if (miniWaypoint != Vector3.zero && (miniWaypoint - transform.position).sqrMagnitude > displacementTolerance * displacementTolerance) {
			Vector3 newForward = Vector3.Cross (Vector3.up, circleCenter - transform.position);

			Debug.DrawRay (transform.position, newForward * 10, Color.green);
			Debug.DrawRay (transform.position, Vector3.up * 10, Color.red);
			Debug.DrawRay (transform.position, (circleCenter - transform.position).normalized * 10, Color.blue);
			if (newForward != Vector3.zero) {
					transform.rotation = Quaternion.LookRotation (-side * newForward);
			}
			circleCenter = transform.position +  side*transform.right * TurnRadius;
			transform.position += transform.forward * Speed;

		} else {
			//Debug.Log ("NewWaypoint");



			//Debug.Log (- Mathf.Sign(Vector3.Dot(transform.right, _agent.steeringTarget - transform.position)) * Vector3.Cross(Vector3.up, circleCenter - transform.position));
			//transform.rotation = Quaternion.LookRotation (- Mathf.Sign(Vector3.Dot(transform.right, _agent.steeringTarget - transform.position)) * Vector3.Cross(Vector3.up, circleCenter - transform.position), Vector3.up);

			circleCenter = transform.position +  side*transform.right * TurnRadius;
			miniWaypoint = GetPointOnCircle (circleCenter);

			Debug.DrawLine(transform.position, miniWaypoint,Color.red);
			Debug.DrawLine(transform.position, circleCenter);
		}


    }

    private void MoveForward()
    {
        transform.position += transform.forward * Speed;
    }

    private Vector3 GetPointOnCircle(Vector3 center)
    {
        
		var cosA = 1 - delta / (TurnRadius * TurnRadius);
        var sinA = Mathf.Sqrt(1 - cosA * cosA);

		var point = center - side * transform.right * TurnRadius * cosA + transform.forward * sinA * TurnRadius;
        return point;
    }
}
