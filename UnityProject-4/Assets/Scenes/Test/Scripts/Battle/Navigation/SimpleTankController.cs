using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SimpleTankController : SimpleController {
    public List<TrackInfo> trackInfos;
    public float stopSpeed = 0.1f;
    public float forwardAngle = 2;
    bool stop;
    bool right;
    bool forward;

    public override void Steering(Vector2 prefVelocity, float maxSpeed)
    {
        motor = prefVelocity.magnitude / maxSpeed * maxMotorTorque;
        Vector2 ownVeocity = new Vector2( rigBody.velocity.x, rigBody.velocity.z);
        forward =( Vector2.Angle(ownVeocity, prefVelocity) < forwardAngle);
        
        stop = (rigBody.velocity.sqrMagnitude < stopSpeed*stopSpeed);
        right = (Mathf.Sign(Vector2.Dot(new Vector2( transform.right.x, transform.right.z), prefVelocity)) == 1);
        //Debug.Log(Vector2.Angle(ownVeocity, prefVelocity) + " " + forward + " " + right + " " + stop);
    }

    void Update () {
        foreach (TrackInfo trackInfo in trackInfos)
        {
            if (forward)
            {
                foreach (WheelCollider wheel in trackInfo.leftTrack)
                {
                    wheel.motorTorque = motor;
                    wheel.brakeTorque = 0;
                }
                foreach (WheelCollider wheel in trackInfo.rightTrack)
                {
                    wheel.motorTorque = motor;
                    wheel.brakeTorque = 0;
                }
            }
            else if (right)
            {
                if (stop)
                {
                    foreach (WheelCollider wheel in trackInfo.leftTrack)
                    {
                        wheel.motorTorque = -motor;
                        wheel.brakeTorque = 0;
                    }
                    foreach (WheelCollider wheel in trackInfo.rightTrack)
                    {
                        wheel.motorTorque = motor;
                        wheel.brakeTorque = 0;
                    }
                } else
                {
                    foreach (WheelCollider wheel in trackInfo.leftTrack)
                    {
                        wheel.motorTorque = 0;
                        wheel.brakeTorque = maxBreaksTorque;
                    }
                    foreach (WheelCollider wheel in trackInfo.rightTrack)
                    {
                        wheel.motorTorque = motor;
                        wheel.brakeTorque = 0;
                    }
                }

            }
            else
            {
                if (stop)
                {
                    foreach (WheelCollider wheel in trackInfo.leftTrack)
                    {

                        wheel.motorTorque = motor;
                        wheel.brakeTorque = 0;
                    }
                    foreach (WheelCollider wheel in trackInfo.rightTrack)
                    {
                        wheel.motorTorque = -motor;
                        wheel.brakeTorque = 0;
                    }
                }
                else
                {
                    foreach (WheelCollider wheel in trackInfo.leftTrack)
                    {
                        wheel.motorTorque = motor;
                        wheel.brakeTorque = 0;
                    }
                    foreach (WheelCollider wheel in trackInfo.rightTrack)
                    {
                        wheel.motorTorque = 0;
                        wheel.brakeTorque = maxBreaksTorque;
                    }
                }
            }
        }
    }
}
