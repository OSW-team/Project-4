using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MassOrientedMoving : MonoBehaviour {
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

		path.speed = Speed[transmission];
		for (var i = 0; i < TurnRadius.Length; i++) {
			circleCenter [i] = transform.position + side * transform.right * TurnRadius [i];
		}
		SpeedTransform ();

		side = Mathf.Sign (Vector3.Dot (transform.right, destination - transform.position));
		_agent.SetDestination(Target.position);
		destination = _agent.steeringTarget;

		if (_agent.hasPath)
		{
			
			int k = 0;

			while ((destination - circleCenter [transmission]).sqrMagnitude < TurnRadius[transmission]*TurnRadius[transmission] || (destination - transform.position).sqrMagnitude < arriveDistance * arriveDistance) {
				if (_agent.path.corners.Length > k ) {
					destination = _agent.path.corners [k++];
					onGoal = false;
				} else {
					onGoal = true;
					break;
				}
			}
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
			}
		}

	}

	private void Turn()
	{
		float turnAngle =  currentSpeed * Time.deltaTime / TurnRadius[transmission];


		if (miniWaypoint != Vector3.zero && (miniWaypoint - transform.position).sqrMagnitude > displacementTolerance * displacementTolerance) {
			Vector3 newForward = transform.forward * Mathf.Cos (turnAngle) + Mathf.Sign(currentSpeed)* side * transform.right * Mathf.Sin (turnAngle);
			if (TryMesh ()) {

			}


			if (newForward != Vector3.zero) {
				transform.rotation = Quaternion.LookRotation (newForward);
			}

			transform.position += transform.forward * currentSpeed * Time.deltaTime;

		} else {
			miniWaypoint = GetPointOnCircle (1, transmission, delta);
		}


	}

	private void MoveForward()
	{
		Vector3 WPLayer = transform.position;

		for(int i = 0; i< path.points.Count; i++){
			for (int j = 0; j < fearPoints.Count; j++) {
				Debug.DrawLine (path.points [i], fearPoints [j], Color.grey);
				if ((path.points [i] - fearPoints [j]).sqrMagnitude < 4 * path.radius * path.radius) {
					transmission = 3;
					Debug.Log (gameObject + " affraid of " + fearPoints [j]);
					break;
				} else {
					transmission = 0;
				}

			}
			if (transmission == 3) {
				break;
			}
		}

		transform.position += transform.forward * currentSpeed*Time.deltaTime;
		Vector3 point = transform.position;



	}

	private Vector3 GetPointOnCircle(int count, int _transmission, float _delta)
	{
		float turnAngle =  Speed[_transmission] * Time.deltaTime / TurnRadius[_transmission];
		Vector3 position;
		Vector3 forward;
		position =  transform.position;
		forward = transform.forward;
		int k = 0;
		badWay[_transmission] = false;

		forward = forward * Mathf.Cos (turnAngle * Mathf.Deg2Rad) + side * Vector3.Cross(Vector3.up, forward) * Mathf.Sin (turnAngle * Mathf.Deg2Rad );

		position += forward *  Mathf.Abs(Speed[_transmission]) * Time.deltaTime;

		var point = position;
		var oldpoint = point;
		for (var i = 1; i < count; i++) {


			forward = forward * Mathf.Cos (turnAngle) + side * Vector3.Cross(Vector3.up, forward) * Mathf.Sin (turnAngle);
			position += forward * Mathf.Abs(Speed[_transmission]) * Time.deltaTime;

			point = position;




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

		Vector3 first = point;
		Vector3 second = first + (destination - first).normalized*delta;
		while ((first - destination).sqrMagnitude > delta * delta || (second - destination).sqrMagnitude > delta * delta && k < 1000) {
			k++;

			first = second;
			second = first + (destination - first).normalized * delta;

		}
		return point;
	}

	bool TryMesh(){

		transmission = 0;
		for (int i = 0; i < badWay.Length; i++) {
			badWay [i] = false;
		}
		GetPointOnCircle (180, 0, delta);
		if (badWay [0]) {
			transmission = 1;
			GetPointOnCircle (180, 1, delta);

		} 
		if (badWay [1]) {
			transmission = 2;
			GetPointOnCircle (180, 2, delta);
		}
		if (badWay [2]) {
			transmission = 3;
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
			} 
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

	bool MotionPlan(){
		if (Vector3.Angle (destination, transform.forward) < angleErrorTollerance) {
			if (ForwardPlan()) {
				transmission = 0;
				return true;
			}
		}
		return false;
	}

	bool ForwardPlan(){
		
		Vector3 oldpoint = transform.position;
		Vector3 point = transform.position + transform.forward;
		for (int i = 0; i < path.points.Count; i++) {

		}
		return false;
	}



}