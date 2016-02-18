using UnityEngine;
using System.Collections;
using RVO;

public class SynchronizeORCA : MonoBehaviour {
	public GameObject[] units;
	public Simulator simulator = Simulator.Instance;
	public float[] weight;
	public float[] massSupermacy;
	void Start(){

		simulator.setAgentDefaults (10, 200, 10.0f, 5.0f, 0, 1, new RVO.Vector2 (0, 0));

		for (var i = 0; i < units.Length; i++) {
			simulator.addAgent (VectorConvert(units[i].transform.position));
			simulator.agents_ [i].radius_ = 1;
			simulator.agents_ [i].active = true;
			simulator.agents_ [i].weight = weight[i];
			simulator.agents_ [i].massSupermacy = massSupermacy[i];
		}
		simulator.processObstacles ();
	}

	void Update()
	{
		simulator.setTimeStep(Time.deltaTime);
		for (var i = 0; i < units.Length; i++) {
			units [i].transform.position =VectorConvert(simulator.agents_ [i].position_);
			simulator.agents_ [i].prefVelocity_ = VectorConvert(Vector3.forward * (1 - i%2*2));
			Debug.DrawRay (units [i].transform.position, VectorConvert( simulator.agents_ [i].velocity_), Color.red);
			Debug.DrawRay (units [i].transform.position, VectorConvert( simulator.agents_ [i].prefVelocity_), Color.blue);
		}
		simulator.doStep();
		//SpawnUnit();

	}

	public Vector3 VectorConvert(RVO.Vector2 vector){
		return new Vector3 (vector.x_, 0, vector.y_);
	}
	public RVO.Vector2 VectorConvert (Vector3 vector){
		RVO.Vector2 _vector = new RVO.Vector2 ();
		_vector.x_ = vector.x;
		_vector.y_ = vector.z;
		return _vector;
	}
}
