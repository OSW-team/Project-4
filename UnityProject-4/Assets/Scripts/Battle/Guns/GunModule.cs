using UnityEngine;
using System.Collections;
using System;

public class GunModule : MonoBehaviour {
    public Transform enemy;
	public int flattness = -1; // 1 = flat, -1 = plunging
    public float V;
    float g = Physics.gravity.magnitude;
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
		if (seeker.chosenTarget == null) {
			return;
		} 
		enemy = seeker.chosenTarget.transform;
        
		Vector3 shootPoint = AdvanceCalculation();
		Vector3 onTarget = (shootPoint - aimVector.transform.position);

		//Debug.Log(" Whatt?! = " + shootPoint);


        /*float H = (shootPoint - aimVector.transform.position).y;
		float D = Vector3.ProjectOnPlane (shootPoint - aimVector.transform.position, Vector3.up).magnitude - 0.5f; //Не помню, зачем -0.5, но вдруг надо!
		//float t1 = Mathf.Sqrt((Mathf.Abs(2 * (V * V - g * H) + 2 * Mathf.Sqrt(V * V * V * V - 2 * g * H * V * V - g * g * D * D))) / (g * g));
		//float t2 = Mathf.Sqrt((Mathf.Abs(2 * (V * V - g * H) - 2 * Mathf.Sqrt(V * V * V * V - 2 * g * H * V * V - g * g * D * D))) / (g * g));
        //t3 = Mathf.Min(t1, t2);
		float t3 = Mathf.Sqrt((Mathf.Abs(2 * (V * V - g * H) - 2 * Mathf.Sqrt(V * V * V * V - 2 * g * H * V * V - g * g * D * D))) / (g * g));

        float gamma = Mathf.Acos(D / (V * t3));
        float time = Vector3.ProjectOnPlane(onTarget, Vector3.up).magnitude / (V);
        float height = (g * t3 * t3) / 8;
		float deltaHeight = enemy.transform.position.y - aimVector.position.y;
        //Debug.Log(gamma);
        if (gamma == gamma)
        {
            horizontalRotationGroup.transform.rotation = Quaternion.RotateTowards(horizontalRotationGroup.rotation, Quaternion.LookRotation(Vector3.ProjectOnPlane(onTarget, horizontalRotationGroup.transform.up)), angularTargetingSpeedHorizontal * Time.deltaTime);
            verticalRotationGroup.rotation = Quaternion.RotateTowards(verticalRotationGroup.rotation, Quaternion.LookRotation(Vector3.up * Mathf.Sin(gamma) + Vector3.ProjectOnPlane(horizontalRotationGroup.forward, horizontalRotationGroup.up).normalized * Mathf.Cos(gamma)), angularTargetingSpeedVertical * Time.deltaTime);
        }
		*/


		float X = Mathf.Sqrt(onTarget.x * onTarget.x + onTarget.z * onTarget.z), Y = onTarget.y;

		//Debug.Log (" X = " + X + " Y = " + Y + " g = " + g + " V = " + V);

		float sqrtDiscriminant = Mathf.Sqrt ((Y * g * X * X - V * V * X * X) * (Y * g * X * X - V * V * X * X) - (X * X + Y) * g * g * X * X * X * X);
		//Debug.Log ("Discrim = " + ((Y * g * X * X - V * V * X * X) * (Y * g * X * X - V * V * X * X) - (X * X + Y) * g * g * X * X * X * X));

		Vector2 V0 = new Vector3 ();
		V0.x = (-(Y*g*X*X - V*V*X*X) - flattness* sqrtDiscriminant)/(2*(X*X + Y));
		//Debug.Log("-(Y*g*X*X - V*V*X*X) " + (-(Y*g*X*X - V*V*X*X)) + "sqrtDiscriminant " + sqrtDiscriminant + "(-(Y*g*X*X - V*V*X*X) - sqrtDiscriminant)" + (-(Y*g*X*X - V*V*X*X) - sqrtDiscriminant));
		V0.y = Mathf.Sqrt(V * V - V0.x);
		V0.x = Mathf.Sqrt (V0.x);

		//Debug.DrawLine (aimVector.position, aimVector.position + (Vector3.up * Y + Vector3.ProjectOnPlane (onTarget, Vector3.up).normalized * X));
		//Debug.Log (V0);

		/*Vector3 prevPoint = aimVector.position, beginPoint = prevPoint;
		for (float x = 0; x < 1000; x++) {
			
			float y = (V0.y/V0.x)*x - (g*x*x)/(2*V0.x*V0.x);
			Debug.DrawLine (prevPoint, beginPoint + (Vector3.up * y + Vector3.ProjectOnPlane (onTarget, Vector3.up).normalized * x), Color.red);
			prevPoint = beginPoint + (Vector3.up * y + Vector3.ProjectOnPlane (onTarget, Vector3.up).normalized * x);
		}*/

		//V0.Normalize ();
		//Debug.Log (V0);

		if (V0.x == V0.x && V0.y == V0.y) {
			//Debug.DrawRay (horizontalRotationGroup.position, horizontalRotationGroup.forward * 100, Color.yellow);
			//Debug.DrawRay (verticalRotationGroup.position, horizontalRotationGroup.forward * 100, Color.green);
			horizontalRotationGroup.transform.rotation = Quaternion.RotateTowards (horizontalRotationGroup.rotation, Quaternion.LookRotation (Vector3.ProjectOnPlane (onTarget, horizontalRotationGroup.transform.up), horizontalRotationGroup.up), angularTargetingSpeedHorizontal * Time.deltaTime);
			verticalRotationGroup.rotation = Quaternion.RotateTowards (verticalRotationGroup.rotation, Quaternion.LookRotation (Vector3.up * V0.y + Vector3.ProjectOnPlane (horizontalRotationGroup.forward, Vector3.up).normalized * V0.x), angularTargetingSpeedVertical * Time.deltaTime);
		}

		Double x = (double)Vector3.RotateTowards (horizontalRotationGroup.forward, Vector3.ProjectOnPlane (onTarget, horizontalRotationGroup.up).normalized, errorTolerance * Mathf.Deg2Rad, 100).x;
		Double y = (double)Vector3.RotateTowards (horizontalRotationGroup.forward, Vector3.ProjectOnPlane (onTarget, horizontalRotationGroup.up).normalized, errorTolerance * Mathf.Deg2Rad, 100).y;
		Double z = (double)Vector3.RotateTowards (horizontalRotationGroup.forward, Vector3.ProjectOnPlane (onTarget, horizontalRotationGroup.up).normalized, errorTolerance * Mathf.Deg2Rad, 100).z;
		Double _x = (double)onTarget.normalized.x;
		Double _y = (double)onTarget.normalized.y;
		Double _z = (double)onTarget.normalized.z;
		x = Math.Round (x, 2);
		y = Math.Round (y, 2);
		z = Math.Round (z, 2);
		_x = Math.Round (_x, 2);
		_y = Math.Round (_y, 2);
		_z = Math.Round (_z, 2);

		shootEnable = ((enemy.transform.position - transform.position).sqrMagnitude < maxDistance*maxDistance && x == _x && z == _z);
		// ((enemy.transform.position - transform.position).sqrMagnitude < maxDistance*maxDistance&& Quaternion.RotateTowards(horizontalRotationGroup.rotation, Quaternion.LookRotation(Vector3.ProjectOnPlane(onTarget, horizontalRotationGroup.up)), errorTolerance) == Quaternion.LookRotation(Vector3.ProjectOnPlane(onTarget, horizontalRotationGroup.up)) && Quaternion.RotateTowards(verticalRotationGroup.rotation, Quaternion.LookRotation(Vector3.up * Mathf.Sin(gamma) + Vector3.ProjectOnPlane(horizontalRotationGroup.forward, Vector3.up).normalized * Mathf.Cos(gamma)), errorTolerance) == Quaternion.LookRotation(Vector3.up * Mathf.Sin(gamma) + Vector3.ProjectOnPlane(horizontalRotationGroup.forward, horizontalRotationGroup.up).normalized * Mathf.Cos(gamma)));
        if (currentBarrel < spawnPoints.Length)
        {
            if (Timer(barrelShootTime, shootEnable) && shootEnable )
            {
				
                GameObject _bullet = Instantiate(bullet, spawnPoints[currentBarrel].position, spawnPoints[currentBarrel].rotation) as GameObject;
				if (_bullet.GetComponent<Homing> () != null) {
					_bullet.GetComponent<Homing> ().target = enemy;
				} else {
					_bullet.GetComponent<Rigidbody> ().AddForce (Missing (spawnPoints [currentBarrel], accuracy) * V * correct);
				}
				//Debug.Log ("Bang " + _bullet.GetComponent<Rigidbody>().velocity.magnitude);
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
		Rigidbody enemyRB = enemy.GetComponent<Rigidbody> ();
		if (enemyRB != null) {
			enemyVelocity = enemyRB.velocity;
		} else {
			enemyVelocity = Vector3.zero;
		}

        advance = enemy.position;


        float H = (advance - aimVector.transform.position).y;
        float D = Vector3.ProjectOnPlane(enemy.transform.position - aimVector.transform.position, Vector3.up).magnitude;
        //float t1 = Mathf.Sqrt((Mathf.Abs(2 * (V * V - g * H) + 2 * Mathf.Sqrt(V * V * V * V - 2 * g * H * V * V - g * g * D * D))) / g / g);
        float t1 = Mathf.Sqrt((Mathf.Abs(2 * (V * V - g * H) - 2 * Mathf.Sqrt(V * V * V * V - 2 * g * H * V * V - g * g * D * D))) / g / g);

        float t2 = 0;

        advance = enemy.transform.position + enemyVelocity * t1;
        //for(iterations = 0; iterations < 5; iterations ++)
        while (Mathf.Abs(t1 - t2) > 0.005 && t1 == t1 && t2 == t2 && iterations < 10)
        {

            iterations++;
			t1 = t2;
            
           
            advance = enemy.transform.position + enemyVelocity * t2;
            H = (advance - aimVector.transform.position).y;
            D = Vector3.ProjectOnPlane(advance - aimVector.transform.position, Vector3.up).magnitude;

            //t1 = Mathf.Sqrt((Mathf.Abs(2 * (V * V - g * H) + 2 * Mathf.Sqrt(V * V * V * V - 2 * g * H * V * V - g * g * D * D))) / g / g);

            t2 = Mathf.Sqrt((Mathf.Abs(2 * (V * V - g * H) - 2 * Mathf.Sqrt(V * V * V * V - 2 * g * H * V * V - g * g * D * D))) / g / g);
            //Debug.Log ("t1 = "+ t1);
            //Debug.Log ("t2 = "+ t2);
            //Debug.Log ("t3 = "+ t3);
        }
        //Debug.Log (iterations);
        //iterations = 0;

        //advance = enemy.transform.position + enemyVelocity*t1;*/
		return advance;
    }

}
