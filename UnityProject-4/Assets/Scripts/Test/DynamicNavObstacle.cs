using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RVO;

public class DynamicNavObstacle : MonoBehaviour {
	Simulator simulator = Simulator.Instance;
	MasterMindNHWheels master;
	int[] obstacles = new int[8];
	// Use this for initialization
	void OnEnable () {
		Debug.Log("before " + simulator.obstacles_.Count);
		List<RVO.Vector2> _obstacles = new List<RVO.Vector2>();
		GameObject obstacle = this.gameObject;

		_obstacles.Add(new RVO.Vector2(obstacle.transform.position.x + obstacle.transform.localScale.x / 2, obstacle.transform.position.z - obstacle.transform.localScale.z / 2));
		_obstacles.Add(new RVO.Vector2(obstacle.transform.position.x + obstacle.transform.localScale.x / 2, obstacle.transform.position.z + obstacle.transform.localScale.z / 2));

		simulator.addObstacle(_obstacles);

		obstacles [0]  = simulator.obstacles_.Count - 1;
		obstacles [1]  = simulator.obstacles_.Count - 2;
		_obstacles.Clear();


		_obstacles.Add(new RVO.Vector2(obstacle.transform.position.x - obstacle.transform.localScale.x / 2, obstacle.transform.position.z - obstacle.transform.localScale.z / 2));
		_obstacles.Add(new RVO.Vector2(obstacle.transform.position.x + obstacle.transform.localScale.x / 2, obstacle.transform.position.z - obstacle.transform.localScale.z / 2));

		simulator.addObstacle(_obstacles);

		obstacles [2]  = simulator.obstacles_.Count - 1;
		obstacles [3]  = simulator.obstacles_.Count - 2;
		_obstacles.Clear();

		_obstacles.Add(new RVO.Vector2(obstacle.transform.position.x - obstacle.transform.localScale.x / 2, obstacle.transform.position.z + obstacle.transform.localScale.z / 2));
		_obstacles.Add(new RVO.Vector2(obstacle.transform.position.x - obstacle.transform.localScale.x / 2, obstacle.transform.position.z - obstacle.transform.localScale.z / 2));

		simulator.addObstacle(_obstacles);

		obstacles [4]  = simulator.obstacles_.Count - 1;
		obstacles [5]  = simulator.obstacles_.Count - 2;
		_obstacles.Clear();

		_obstacles.Add(new RVO.Vector2(obstacle.transform.position.x + obstacle.transform.localScale.x / 2, obstacle.transform.position.z + obstacle.transform.localScale.z / 2));
		_obstacles.Add(new RVO.Vector2(obstacle.transform.position.x - obstacle.transform.localScale.x / 2, obstacle.transform.position.z + obstacle.transform.localScale.z / 2));

		simulator.addObstacle(_obstacles);

		obstacles [6]  = simulator.obstacles_.Count - 1;
		obstacles [7]  = simulator.obstacles_.Count - 2;

		_obstacles.Clear();


		Debug.DrawLine(new Vector3( obstacle.transform.position.x + obstacle.transform.localScale.x / 2, 1, obstacle.transform.position.z + obstacle.transform.localScale.z / 2), new Vector3(obstacle.transform.position.x + obstacle.transform.localScale.x / 2, 1, obstacle.transform.position.z - obstacle.transform.localScale.z / 2), Color.blue, 1000);
		Debug.DrawLine(new Vector3(obstacle.transform.position.x + obstacle.transform.localScale.x / 2, 1, obstacle.transform.position.z - obstacle.transform.localScale.z / 2), new Vector3(obstacle.transform.position.x - obstacle.transform.localScale.x / 2, 1, obstacle.transform.position.z - obstacle.transform.localScale.z / 2), Color.blue, 1000);
		Debug.DrawLine(new Vector3(obstacle.transform.position.x - obstacle.transform.localScale.x / 2, 1, obstacle.transform.position.z - obstacle.transform.localScale.z / 2), new Vector3(obstacle.transform.position.x - obstacle.transform.localScale.x / 2,1, obstacle.transform.position.z + obstacle.transform.localScale.z / 2), Color.blue, 1000);
		Debug.DrawLine(new Vector3(obstacle.transform.position.x - obstacle.transform.localScale.x / 2, 1, obstacle.transform.position.z + obstacle.transform.localScale.z / 2), new Vector3(obstacle.transform.position.x + obstacle.transform.localScale.x / 2,1, obstacle.transform.position.z + obstacle.transform.localScale.z / 2), Color.blue, 1000);

		//simulator.addObstacle(_obstacles);
		simulator.processObstacles();

		Debug.Log("after " + simulator.obstacles_.Count);
	

	}

	void Remove(){
		Debug.Log ("AAAAGHFTFHGHF!");
		for(var i = 0; i < obstacles.Length; i ++){

			Debug.Log ("AAAAGHFTFHGHF! " + obstacles [i] );
			simulator.obstacles_[obstacles [i]].point_ = new RVO.Vector2(10000, 10000);
		}

		simulator.processObstacles ();
	}
	
	// Update is called once per frame
	void OnDestroy () {
		Remove ();
	}
}
