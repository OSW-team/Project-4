using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class SimpleCarController : SimpleController
{

    public List<AxleInfo> axleInfos;

    public override void Steering(Vector2 prefVelocity, float maxSpeed)
    {
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
            motor = -maxMotorTorque * Mathf.Sqrt(prefVelocity.x * prefVelocity.x + prefVelocity.y * prefVelocity.y) /  maxSpeed;
        }
        else if (UnityEngine.Vector2.Angle(new UnityEngine.Vector2(transform.forward.x, transform.forward.z).normalized, new UnityEngine.Vector2(prefVelocity.x, prefVelocity.y).normalized) > 80)
        {
            motor = maxMotorTorque * Mathf.Sqrt(prefVelocity.x * prefVelocity.x + prefVelocity.y * prefVelocity.y) /  maxSpeed;
        }
        else
        {
            motor = maxMotorTorque / 2 * Mathf.Sqrt(prefVelocity.x * prefVelocity.x + prefVelocity.y * prefVelocity.y) /  maxSpeed;
        }
        if (rigBody != null && Vector3.Dot(rigBody.velocity,  transform.forward) < 0)
        {
            steering = -steering;
        }
    }

    public void FixedUpdate()
    {
        foreach (AxleInfo axleInfo in axleInfos)
        {
            if (axleInfo.steering)
            {
                
                axleInfo.leftWheel.steerAngle = steering*maxSteeringAngle;
                axleInfo.rightWheel.steerAngle = steering * maxSteeringAngle;
            }
            if (axleInfo.motor)
            {
                axleInfo.leftWheel.motorTorque = motor;
                axleInfo.rightWheel.motorTorque = motor;
            }
            axleInfo.leftWheel.brakeTorque = 0;
            axleInfo.rightWheel.brakeTorque = 0;
            
        }
    }
    
}