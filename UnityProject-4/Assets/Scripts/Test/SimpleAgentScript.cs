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
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();

    }

    void FixedUpdate()
    {
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
                Debug.Log("Turn");
                Turn();
            }
        }
    }

    private void Turn()
    {
        var circleCenter = transform.right * TurnRadius;
        Debug.DrawLine(transform.position, GetPointOnCircle(circleCenter),Color.red);
        Debug.DrawRay(transform.position, circleCenter);

    }

    private void MoveForward()
    {
        transform.position += transform.forward * Speed;
    }

    private Vector3 GetPointOnCircle(Vector3 center)
    {
        var point = Vector3.zero;
        var cosA = 1 - delta / (TurnRadius * TurnRadius);
        var sinA = Mathf.Sqrt(1 - cosA * cosA);
        point.x = center.x + TurnRadius * cosA;
        point.y = center.y;
        point.z = center.z + TurnRadius*sinA;
        return point;
    }
}
