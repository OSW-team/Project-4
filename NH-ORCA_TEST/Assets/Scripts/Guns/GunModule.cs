using UnityEngine;
using System.Collections;
using System;

public class GunModule : MonoBehaviour {
    public Transform enemy;
    public float V;
    public float g = Physics.gravity.magnitude;
    public Transform verticalRotationGroup, horizontalRotationGroup, aimVector;
    public Transform[] spawnPoints;
    public float angularTargetingSpeedHorizontal = 10;
    public float angularTargetingSpeedVertical = 10;
    public bool shootEnable;
    public GameObject bullet;
    int currentBarrel;
    public float correct;
    public float accuracy;
    public float barrelShootTime, magazineReloadTime;
    public float timer;
    public float errorTolerance;
    UnitStats stats;
    TargetSeek seeker;
    public float maxDistance;

    // Use this for initialization
    void Start () {

        seeker = GetComponent<TargetSeek>();
        stats = GetComponentInParent<UnitStats>();
        currentBarrel = 0;
	}
	
	// Update is called once per frame
	void Update () {
        Targeting();
	}
    void Targeting()
    {
        if (seeker.chosenTarget != null)
        {
            enemy = seeker.chosenTarget.transform;
        }
        float t3;
        //Vector3 advance = enemy.position;
        if (enemy != null)
        {
            Vector3 shootPoint = AdvanceCalculation() + new Vector3(0, 2, 0);
            Vector3 onTarget = (shootPoint - transform.position);
            //Debug.Log(onTarget);
            float H = (shootPoint - aimVector.transform.position).y;
            float D = Vector3.ProjectOnPlane(shootPoint - aimVector.transform.position, Vector3.up).magnitude - 0.5f;
            float t1 = Mathf.Sqrt((Mathf.Abs(2 * (V * V - g * H) + 2 * Mathf.Sqrt(V * V * V * V - 2 * g * H * V * V - g * g * D * D))) / g / g);
            float t2 = Mathf.Sqrt((Mathf.Abs(2 * (V * V - g * H) - 2 * Mathf.Sqrt(V * V * V * V - 2 * g * H * V * V - g * g * D * D))) / g / g);
            t3 = Mathf.Min(t1, t2);

            float gamma = Mathf.Acos(D / (V * t3));
            float time = Vector3.ProjectOnPlane(onTarget, Vector3.up).magnitude / (V);
            float height = -(g * time * time) / 2;
            float deltaHeight = enemy.transform.position.y - transform.position.y;
            //Debug.Log(gamma);
            if (gamma == gamma)
            {
                horizontalRotationGroup.transform.rotation = Quaternion.RotateTowards(horizontalRotationGroup.rotation, Quaternion.LookRotation(Vector3.ProjectOnPlane(onTarget, horizontalRotationGroup.transform.up)), angularTargetingSpeedHorizontal * Time.deltaTime);
                verticalRotationGroup.rotation = Quaternion.RotateTowards(verticalRotationGroup.rotation, Quaternion.LookRotation(Vector3.up * Mathf.Sin(gamma) + Vector3.ProjectOnPlane(horizontalRotationGroup.forward, horizontalRotationGroup.up).normalized * Mathf.Cos(gamma)), angularTargetingSpeedVertical * Time.deltaTime);
            }
            shootEnable = ((enemy.transform.position - transform.position).sqrMagnitude < maxDistance*maxDistance&& Quaternion.RotateTowards(horizontalRotationGroup.rotation, Quaternion.LookRotation(Vector3.ProjectOnPlane(onTarget, horizontalRotationGroup.up)), errorTolerance) == Quaternion.LookRotation(Vector3.ProjectOnPlane(onTarget, horizontalRotationGroup.up)) && Quaternion.RotateTowards(verticalRotationGroup.rotation, Quaternion.LookRotation(Vector3.up * Mathf.Sin(gamma) + Vector3.ProjectOnPlane(horizontalRotationGroup.forward, Vector3.up).normalized * Mathf.Cos(gamma)), errorTolerance) == Quaternion.LookRotation(Vector3.up * Mathf.Sin(gamma) + Vector3.ProjectOnPlane(horizontalRotationGroup.forward, horizontalRotationGroup.up).normalized * Mathf.Cos(gamma)));
            if (currentBarrel < spawnPoints.Length)
            {
                if (Timer(barrelShootTime, shootEnable) && shootEnable )
                {
                    GameObject _bullet = Instantiate(bullet, spawnPoints[currentBarrel].position, spawnPoints[currentBarrel].rotation) as GameObject;
                    _bullet.GetComponent<Rigidbody>().AddForce(Missing(spawnPoints[currentBarrel], accuracy) * V * correct);
                    currentBarrel++;
                }
            }
            else
            {
                if (Timer(magazineReloadTime, true))
                {
                    //Debug.Log("magazine reload");
                    currentBarrel = 0;
                }
            }
            
            /*if (height > deltaHeight && flatness)
            {
                gamma *= -1;
            }*/
        }
    }

    bool Timer(float ResetTime, bool resetCondition)
    {
        if(timer > 0)
        {
            timer -= Time.deltaTime;
            return false;
        }
        else if(resetCondition)
        {
            timer = ResetTime;
            return true;
        } else
        {
            timer = 0;
            return true;
        }
    }

    float AccuracyFit(float _accuracy)
    {
        return _accuracy;
    }

    Vector3 Missing(Transform spawnPoint, float accuracy)
    {
        Vector2 missUn = UnityEngine.Random.insideUnitCircle;
        float s = missUn.sqrMagnitude;
        Vector2 missN = new Vector2(missUn.x*Mathf.Sqrt(-2*Mathf.Log(s)/s), missUn.y*Mathf.Sqrt(-2 * Mathf.Log(s) / s));
        return (spawnPoint.forward * AccuracyFit(accuracy) + spawnPoint.up * missN.y + spawnPoint.right * missN.x).normalized; 
    }

    Vector3 AdvanceCalculation()
    {
        Vector3 enemyVelocity, advance;
        int iterations = 0;
        enemyVelocity = enemy.GetComponent<Rigidbody>().velocity;
        advance = enemy.position;


        float H = (advance - aimVector.transform.position).y;
        float D = Vector3.ProjectOnPlane(enemy.transform.position - aimVector.transform.position, Vector3.up).magnitude;
        float t1 = Mathf.Sqrt((Mathf.Abs(2 * (V * V - g * H) + 2 * Mathf.Sqrt(V * V * V * V - 2 * g * H * V * V - g * g * D * D))) / g / g);

        float t2 = Mathf.Sqrt((Mathf.Abs(2 * (V * V - g * H) - 2 * Mathf.Sqrt(V * V * V * V - 2 * g * H * V * V - g * g * D * D))) / g / g);

        float t3 = 0;

        advance = enemy.transform.position + enemyVelocity * t1;
        //for(iterations = 0; iterations < 5; iterations ++)
        while (Mathf.Abs(t1 - t3) > 0.005 && Mathf.Abs(t2 - t3) > 0.005 && t1 == t1 && t2 == t2 && iterations < 10)
        {

            iterations++;

            t3 = Mathf.Min(t1, t2);
           
            advance = enemy.transform.position + enemyVelocity * t3;
            H = (advance - aimVector.transform.position).y;
            D = Vector3.ProjectOnPlane(advance - aimVector.transform.position, Vector3.up).magnitude;

            t1 = Mathf.Sqrt((Mathf.Abs(2 * (V * V - g * H) + 2 * Mathf.Sqrt(V * V * V * V - 2 * g * H * V * V - g * g * D * D))) / g / g);

            t2 = Mathf.Sqrt((Mathf.Abs(2 * (V * V - g * H) - 2 * Mathf.Sqrt(V * V * V * V - 2 * g * H * V * V - g * g * D * D))) / g / g);
            //Debug.Log ("t1 = "+ t1);
            //Debug.Log ("t2 = "+ t2);
            //Debug.Log ("t3 = "+ t3);
        }
        //Debug.Log (iterations);
        //iterations = 0;

        //advance = enemy.transform.position + enemyVelocity*t1;
        return advance;
    }

}
