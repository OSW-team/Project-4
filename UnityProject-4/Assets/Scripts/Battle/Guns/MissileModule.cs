using UnityEngine;
using System.Collections;
using System;

public class MissileModule : MonoBehaviour {
	public Transform enemy;
	public Transform verticalRotationGroup, horizontalRotationGroup, aimVector;
	public Transform[] spawnPoints;
	public float angularTargetingSpeedHorizontal = 10;
	public float angularTargetingSpeedVertical = 10;
	public bool shootEnable;
	public GameObject bullet;
	int currentBarrel;
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

		Vector3 shootPoint = enemy.position;
		Vector3 onTarget = (shootPoint - aimVector.transform.position);
		horizontalRotationGroup.transform.rotation = Quaternion.RotateTowards (horizontalRotationGroup.rotation, Quaternion.LookRotation (Vector3.ProjectOnPlane (onTarget, Vector3.up), horizontalRotationGroup.up), angularTargetingSpeedHorizontal * Time.deltaTime);
		verticalRotationGroup.rotation = Quaternion.RotateTowards (verticalRotationGroup.rotation, Quaternion.LookRotation (transform.forward * (0.1f + onTarget.magnitude/maxDistance) + Vector3.up), angularTargetingSpeedVertical * Time.deltaTime);

		Vector3 hitVector = Vector3.RotateTowards (horizontalRotationGroup.forward, Vector3.ProjectOnPlane (onTarget, horizontalRotationGroup.up).normalized, errorTolerance * Mathf.Deg2Rad, 100);
		Double x = (double)hitVector.x;
		Double y = (double)hitVector.y;
		Double z = (double)hitVector.z;
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

		if (currentBarrel < spawnPoints.Length)
		{
			if (Timer(barrelShootTime, shootEnable) && shootEnable )
			{

				GameObject _bullet = Instantiate(bullet, spawnPoints[currentBarrel].position, spawnPoints[currentBarrel].rotation) as GameObject;
				_bullet.GetComponent<Homing> ().target = enemy;
				currentBarrel++;
			}
		}
		else
		{
			if (Timer(magazineReloadTime, true))
			{
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

}
