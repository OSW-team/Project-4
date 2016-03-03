﻿using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class SimpleAgentScript : MonoBehaviour {
    public Transform Target;
	public float[] Speed = new float[3];
	public float currentSpeed;
	public float[] TurnRadius = new float[3];
	public bool[] badWay = new bool[3];
	bool onGoal = false;
	public int transmission;
    public float Acceleration;
    private NavMeshAgent _agent;
    public float delta = 0.1f;
	public float displacementTolerance;
	public float side, angleErrorTollerance, mass, massSupermacy;
	Vector3[] circleCenter = { Vector3.zero, Vector3.zero, Vector3.zero , Vector3.zero};
	//public GameObject WPlayerBody;
	public Vector3 destination;
	public PathInfo path;
	PathStorage storage;
	public List<Vector3> fearPoints;

	public float arriveDistance;
	//public GameObject WPlayerObject;


	Rigidbody rigBody;

	Vector3 miniWaypoint;
    void Start()
    {
		//WPlayerBody = Instantiate (WPlayerObject, transform.position, transform.rotation) as GameObject;
		//Destroy( WPlayerBody.GetComponent<MeshRenderer>());
		//Destroy( WPlayerBody.GetComponent<Collider>());
		//Destroy( WPlayerBody.GetComponent<MeshFilter>());
		//Destroy(WPlayerBody.GetComponent<Collider>());
		rigBody = GetComponent<Rigidbody> ();
        _agent = GetComponentInChildren<NavMeshAgent>();
		storage = FindObjectOfType<PathStorage> ();
		if (storage != null) {
			storage.Pathes.Add (path);
		}


    }

    void FixedUpdate()
	{
		
		_agent.transform.localPosition = Vector3.zero;
		ReciprocialProcessing ();
		path.points.Clear ();
		path.speed = Speed[transmission];
		for (var i = 0; i < TurnRadius.Length; i++) {
			circleCenter [i] = transform.position + side * transform.right * TurnRadius [i];
		}
		SpeedTransform ();

		side = Mathf.Sign (Vector3.Dot (transform.right, destination - transform.position));
        _agent.SetDestination(Target.position);
		destination = _agent.steeringTarget;
		//Debug.DrawLine (transform.position, circleCenter [0], Color.blue);

        if (_agent.hasPath)
        {
			/*for (int i = 1; i < _agent.path.corners.Length; i++) {
				Debug.DrawLine (_agent.path.corners [i], _agent.path.corners [i - 1]);
			}*/

			int k = 0;

			while ((destination - circleCenter [transmission]).sqrMagnitude < TurnRadius[transmission]*TurnRadius[transmission] || (destination - transform.position).sqrMagnitude < arriveDistance * arriveDistance) {
			//while ((destination - transform.position).sqrMagnitude < 4 *TurnRadius[transmission]*TurnRadius[transmission] ) {
				if (_agent.path.corners.Length > k ) {
					destination = _agent.path.corners [k++];
					onGoal = false;
				} else {
					onGoal = true;
					break;
				}
			}
			//Debug.DrawLine (transform.position, destination, Color.black);
			//Debug.DrawLine (transform.position, circleCenter[transmission], Color.blue);
			if(!onGoal){
	            var angle = Vector3.Angle(transform.forward, destination - transform.position);
				if (TryPath ()) {
					if (angle < angleErrorTollerance) {
						MoveForward ();
					} else {
						Turn ();
					}
				} else {
					MoveForward ();
				}
			//Debug.DrawLine (transform.position, _agent.steeringTarget, Color.black);
			//Debug.DrawRay (transform.position, transform.forward, Color.blue);
			}
        }
		//DebugDraw ();
    }

	private void Turn()
    {
		float turnAngle =  currentSpeed * Time.deltaTime / TurnRadius[transmission];

		//Debug.Log (miniWaypoint);
		if (miniWaypoint != Vector3.zero && (miniWaypoint - transform.position).sqrMagnitude > displacementTolerance * displacementTolerance) {
			Vector3 newForward = transform.forward * Mathf.Cos (turnAngle) + Mathf.Sign(currentSpeed)* side * transform.right * Mathf.Sin (turnAngle);
			if (TryMesh ()) {
				
			}

			//Debug.DrawRay (transform.position, newForward * 10, Color.green);
			//Debug.DrawRay (transform.position, Vector3.up * 10, Color.red);
			//Debug.DrawRay (transform.position, (circleCenter - transform.position).normalized * 10, Color.blue);
			if (newForward != Vector3.zero) {
					transform.rotation = Quaternion.LookRotation (newForward);
			}

			//transform.position += transform.forward * currentSpeed * Time.deltaTime; //GetPointOnCircle (1, transmission, Speed[transmission]*Time.deltaTime/(transform.position - circleCenter[transmission]).magnitude);
			transform.position += transform.forward * currentSpeed * Time.deltaTime;
			//rigBody.AddForce(transform.forward * Speed[0]*Time.deltaTime);

		} else {
			//Debug.Log ("NewWaypoint");



			//Debug.Log (- Mathf.Sign(Vector3.Dot(transform.right, _agent.steeringTarget - transform.position)) * Vector3.Cross(Vector3.up, circleCenter - transform.position));
			//transform.rotation = Quaternion.LookRotation (- Mathf.Sign(Vector3.Dot(transform.right, _agent.steeringTarget - transform.position)) * Vector3.Cross(Vector3.up, circleCenter - transform.position), Vector3.up);

			miniWaypoint = GetPointOnCircle (1, transmission, delta);

			/*Debug.DrawLine(transform.position, miniWaypoint,Color.red);
			Debug.DrawLine(transform.position, circleCenter[transmission]);*/
		}


    }

    private void MoveForward()
    {
		Vector3 WPLayer = transform.position;
		for (int i = 0; i < 30; i++) {
			path.points.Add (WPLayer += transform.forward * Mathf.Abs( currentSpeed)*Time.deltaTime);
		}
		//Debug.Log (path.points.Count + " and " + fearPoints.Count);
		for(int i = 0; i< path.points.Count; i++){
			for (int j = 0; j < fearPoints.Count; j++) {
				Debug.DrawLine (path.points [i], fearPoints [j], Color.grey);
				//Debug.Log ((path.points [i] - fearPoints [j]).sqrMagnitude+ " <? " + 4* path.radius * path.radius);
				if ((path.points [i] - fearPoints [j]).sqrMagnitude < 4 * path.radius * path.radius) {
					//destination = transform.right * arriveDistance * 10;
					transmission = 3;
					Debug.Log (gameObject + " affraid of " + fearPoints [j]);
					break;
					//return;
				} else {
					transmission = 0;
				}
			
			}
			if (transmission == 3) {
				break;
			}
		}
		//transmission = 0;
		transform.position += transform.forward * currentSpeed*Time.deltaTime;
		Vector3 point = transform.position;

		//rigBody.AddForce(transform.forward * Speed[0]*Time.deltaTime);

    }

	/*private Vector3 GetPointOnCircle(int count, int _transmission, float _delta)
    {
		int k = 0;
		badWay[_transmission] = false;

		var radius = (transform.position - circleCenter[_transmission]).magnitude;
		var cosA = 1 - _delta / (radius * radius);
		var sinA = Mathf.Sqrt (1 - cosA * cosA);
		var point = circleCenter[_transmission] - side * transform.right * radius * cosA + transform.forward * sinA * radius;

		for (var i = 1; i < count; i++) {
			k++;
			var oldpoint = point;
			point = circleCenter[_transmission] - (circleCenter[_transmission] - point) * cosA - side * Vector3.Cross(Vector3.up, (circleCenter[_transmission] - point)).normalized * sinA * radius;
			if (k % 2 == 1) {
				Debug.DrawLine (oldpoint, point, Color.yellow);
			} else {
				Debug.DrawLine (oldpoint, point, Color.green);
			}
			NavMeshHit hit;
			badWay[_transmission]  = badWay[_transmission]  || NavMesh.Raycast (point, oldpoint, out hit, NavMesh.AllAreas);

			if (Mathf.Sign (Vector3.Dot (point - _agent.steeringTarget, oldpoint - circleCenter[_transmission])) != 1) {
				break;
			}
		}
		Vector3 first = point;
		Vector3 second = first + (_agent.steeringTarget - first).normalized*delta;
		while ((first - _agent.steeringTarget).sqrMagnitude > delta * delta || (second - _agent.steeringTarget).sqrMagnitude > delta * delta && k < 1000) {
			k++;
			if (k % 2 == 1) {
				Debug.DrawLine (first, second, Color.yellow);
			} else {
				Debug.DrawLine (first, second, Color.green);
			}
			first = second;
			second = first + (_agent.steeringTarget - first).normalized * delta;
		}
        return point;
    }*/


	private Vector3 GetPointOnCircle(int count, int _transmission, float _delta)
	{
		float turnAngle =  Speed[_transmission] * Time.deltaTime / TurnRadius[_transmission];
		Vector3 position;
		Vector3 forward;
		//Transform WPlayer = WPlayerBody.transform;
		//WPlayerBody = WPlayerObject;
		position =  transform.position;
		forward = transform.forward;
		int k = 0;
		badWay[_transmission] = false;

		forward = forward * Mathf.Cos (turnAngle * Mathf.Deg2Rad) + side * Vector3.Cross(Vector3.up, forward) * Mathf.Sin (turnAngle * Mathf.Deg2Rad );

		//WPlayer.rotation = new Quaternion (WPlayer.rotation.x, WPlayer.rotation.y + turnAngle * Time.fixedDeltaTime, WPlayer.rotation.z, WPlayer.rotation.w);

		position += forward *  Mathf.Abs(Speed[_transmission]) * Time.deltaTime;

		var point = position;
		var oldpoint = point;
		for (var i = 1; i < count; i++) {
			

			forward = forward * Mathf.Cos (turnAngle) + side * Vector3.Cross(Vector3.up, forward) * Mathf.Sin (turnAngle);
			//Debug.DrawRay (position, forward, Color.red);
			//Debug.DrawRay (position, Vector3.Cross(Vector3.up, forward), Color.blue);
			position += forward * Mathf.Abs(Speed[_transmission]) * Time.deltaTime;

			point = position;

			if (_transmission == transmission) {
				if (path.points.Count == 0 || (path.points [path.points.Count - 1] - point).sqrMagnitude > delta * delta) {
					path.points.Add (point);
				}
			}


			NavMeshHit hit;
			if ((point - oldpoint).sqrMagnitude > delta * delta) {
				
				badWay [_transmission] = badWay [_transmission] || NavMesh.Raycast (point, oldpoint, out hit, NavMesh.AllAreas);

				for (int l = 0; l < fearPoints.Count; l++) {
					badWay[_transmission] = badWay [_transmission] || ((point - fearPoints [l]).sqrMagnitude < 4 * path.radius * path.radius);
					Debug.DrawLine (point, fearPoints [l]);
				}

				k++;

				if (k % 2 == 1) {
					Debug.DrawLine (oldpoint, point, Color.yellow);
				} else {
					Debug.DrawLine (oldpoint, point, Color.green);
				}
				oldpoint = point;
			}
			if (Mathf.Sign (Vector3.Dot (point - destination, Vector3.Cross(Vector3.up, forward))) != -side) {
				break;
			}
		}
		if (transmission == _transmission) {
			for (int i = 0; i < 30; i++) {
				path.points.Add (point += forward * currentSpeed * Time.deltaTime);
			}
		}
		Vector3 first = point;
		Vector3 second = first + (destination - first).normalized*delta;
		while ((first - destination).sqrMagnitude > delta * delta || (second - destination).sqrMagnitude > delta * delta && k < 1000) {
			k++;

			/*if (k % 2 == 1) {
				Debug.DrawLine (first, second, Color.yellow);
			} else {
				Debug.DrawLine (first, second, Color.green);
			}*/
			first = second;
			second = first + (destination - first).normalized * delta;

		}
		return point;
	}

	bool TryMesh(){
		
		//NavMeshHit hit;
		transmission = 0;
		for (int i = 0; i < badWay.Length; i++) {
			badWay [i] = false;
		}
		GetPointOnCircle (180, 0, delta);
		if (badWay [0]) {
			transmission = 1;
			GetPointOnCircle (180, 1, delta);
			/*
			if (badWay [1]) {
				transmission = 2;
				GetPointOnCircle (180, 2, delta);
				if (badWay [2]) {
					transmission = 3;
					GetPointOnCircle (180, 2, delta);
				}
			}*/
		} 
		if (badWay [1]) {
			transmission = 2;
			GetPointOnCircle (180, 2, delta);
		}
		if (badWay [2]) {
			transmission = 3;
			//GetPointOnCircle (180, 2, delta);
		}
			




		return false;
	}

	void SpeedTransform(){
		if(Mathf.Abs(currentSpeed - Speed[transmission]) > (Acceleration*2*Time.deltaTime)){
			currentSpeed += Mathf.Sign (Speed [transmission] - currentSpeed) * Acceleration * Time.deltaTime;
		}
	}

	void DebugDraw(){
		for (int i = 0; i < path.points.Count; i++) {
			for (int j = 0; j < 360; j++) {
				var point1 = path.points [i] + new Vector3 (path.radius, 0, 0) * Mathf.Sin (j * Mathf.Deg2Rad) + new Vector3 (0, 0, path.radius) * Mathf.Cos (j * Mathf.Deg2Rad);
				var point2 = path.points [i] + new Vector3 (path.radius, 0, 0) * Mathf.Sin ((j+1) * Mathf.Deg2Rad) + new Vector3 (0, 0, path.radius) * Mathf.Cos ((j+1) * Mathf.Deg2Rad);

				Debug.DrawLine (point1, point2);
			}
		}
	}

	void ReciprocialProcessing(){
		fearPoints.Clear();
		for (int i = 0; i < storage.Pathes.Count; i++) {
			
			if (storage.Pathes [i].mass > mass){
				for (int j = 0; j < storage.Pathes [i].points.Count; j++) {
					fearPoints.Add (storage.Pathes [i].points [j]);
				}
			} /*else if  ((storage.Pathes [i].mass == mass && storage.Pathes.IndexOf(path) < i) ){
				for (int j = 0; j < storage.Pathes [i].points.Count; j++) {
					fearPoints.Add (storage.Pathes [i].points [j]);
				}
			}*/
		}
	}

	bool TryPath(){
		if (fearPoints.Count == 0) {
			return true;
		}
		for (int i = 0; i < storage.Pathes.Count; i++) {
			for (int k = 0; k < storage.Pathes [i].points.Count; k++) {
				for (int j = 0; j < fearPoints.Count; j++) {
					if ((storage.Pathes [i].points[k] - fearPoints [j]).sqrMagnitude < 4 * path.radius * path.radius) {
						return true;
					}
				}
			}
		}
		return false;
	}

}



[System.Serializable]
public class PathInfo{
	public List<Vector3> points;
	public float radius;
	public float speed;
	public float mass;

}
