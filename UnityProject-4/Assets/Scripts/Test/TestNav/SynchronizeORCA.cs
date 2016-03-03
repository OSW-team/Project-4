using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RVO;

public class SynchronizeORCA : MonoBehaviour {
	public GameObject[] units;
	//public GameObject[] visualization;
	public Simulator simulator = Simulator.Instance;
	//public float[] weight;
	//public float[] massSupermacy;
	List<ORCAController> orcas;
	public Transform spawnArea;
	public List<GameObject> agentsInScene;
	public List<MinimalPhysicAgent> agents;
	public float maxSpeed;
	public float radius;
	public float maxAngularSpeed;
	public float acc;
	public Transform goal;
	public GameObject[] obstacles;
	public GameObject unit;
	public int manipulationSpheresNumber = 20;
	public int liveUnits = 0;
	public GotTheNavMeshBorders Borderer;
	public GameObject wreckage;

	float timer = 5;



	void Start(){
		Border();
		simulator.setAgentDefaults (10, 200, 10.0f, 5.0f, 6, 10, new RVO.Vector2 (0, 0));

		for( int i = 0; i < manipulationSpheresNumber; i++)
		{
			simulator.addAgent(new RVO.Vector2(10000, 10000));
			simulator.agents_ [i].active = false;
		}


		/*
		for (var i = 0; i < units.Length; i++) {
			simulator.addAgent (VectorConvert(units[i].transform.position));
			simulator.agents_ [agents[i].index].radius_ = 1;
			simulator.agents_ [agents[i].index].active = true;
			simulator.agents_ [agents[i].index].weight = weight[i];
			simulator.agents_ [agents[i].index].massSupermacy = massSupermacy[i];
			simulator.agents_ [agents[i].index].radius_ = 6;
			units [i].GetComponent<ORCAController> ().AgentChose (i);

		}
		*/
		simulator.processObstacles ();
	}

	void Update()
	{
		for(var i = 0; i < simulator.obstacles_.Count; i++){
			Debug.DrawLine( VectorConvert( simulator.obstacles_[i].point_), VectorConvert(simulator.obstacles_[i].nextObstacle.point_), Color.green);
		}
		simulator.setTimeStep(Time.deltaTime);
		RVOAgentsCalculation();
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


	int FindFreeAgent(){
		for (var i = 0; i < simulator.agents_.Count; i++) {
			if (!simulator.agents_ [i].active) {
				return i;
			}
		}
		return -1;
	}

	public void AddAgent(GameObject newAgent, Vector3 position)
	{
		int number = FindFreeAgent();



		if (number != -1) {
			agents.Add(new MinimalPhysicAgent(newAgent, position, number));
			simulator.agents_[number].active = true;
			simulator.agents_[number].radius_ = radius;
			simulator.agents_[number].maxSpeed_ = maxSpeed;
			simulator.agents_[number].neighborDist_ = 100;
			simulator.agents_[number].weight = newAgent.GetComponent<UnitStats> ().weight;
			simulator.agents_[number].massSupermacy = newAgent.GetComponent<UnitStats> ().massSupermacy;
			newAgent.GetComponent<UnitStats> ().meAgent = agents[agents.Count -1];
			newAgent.GetComponent<ORCAController> ().AgentChose (number);
		} else {
			Debug.Log ("NoFreeAgent");
		}
		liveUnits++;
	}


	void RVOAgentsCalculation()
	{
		for(int i = 0; i < liveUnits; i ++)
		{
			if (simulator.agents_ [agents[i].index] != null) {
				Vector3 nullPoint = Vector3.zero;
				if (agents [i].navMeshAgent != null) {
					nullPoint = agents [i].navMeshAgent.path.corners [0];
					agents [i].navMeshAgent.transform.localPosition = Vector3.zero;
				}
				/*if(agents [i].navMeshAgent.isOnNavMesh){
					
				}
				Debug.Log (agents [i].navMeshAgent.isOnNavMesh);*/
				//if ((new Vector3(agents [i].body.transform.position.x, 0, agents [i].body.transform.position.z) - new Vector3(agents [i].navMeshAgent.transform.position.x, 0, agents [i].navMeshAgent.transform.position.z)).sqrMagnitude
				//> 1) {
				//agents [i].body.transform.position += (new Vector3 (agents [i].navMeshAgent.transform.position.x, agents [i].body.transform.position.y, agents [i].navMeshAgent.transform.position.z) - agents [i].body.transform.position);
				//agents [i].navMeshAgent.transform.localPosition -= agents [i].navMeshAgent.transform.localPosition;
				//agents [i].body.transform.position += (agents [i].navMeshAgent.transform.position - agents [i].body.transform.position) / 2 * Time.deltaTime;
				//}
				Vector3 prefVel = Vector3.zero;

				if (agents [i].navMeshAgent != null) {
						prefVel = (agents [i].navMeshAgent.steeringTarget  - (agents [i].navMeshAgent.transform.position)).normalized * agents [i].body.GetComponent<ORCAController> ().maxSpeed;

				}

				if (agents [i].body != null) {
					//simulator.agents_ [agents[i].index].prefVelocity_ = new RVO.Vector2 (prefVel.x, prefVel.z);
					//simulator.agents_ [agents[i].index].radius_ = radius + CalculateError (simulator.agents_ [agents[i].index], Vector3.Angle (new Vector3 (simulator.agents_ [agents[i].index].velocity_.x_, 0, simulator.agents_ [agents[i].index].velocity_.y_), agents [i].body.transform.forward));
					//simulator.agents_ [agents[i].index].position_ = new RVO.Vector2 (agents [i].body.transform.position.x, agents [i].body.transform.position.z);

					Drive (units[i], i, VectorConvert (prefVel));
					DebugDraw (i, agents[i].navMeshAgent.path.corners[0], prefVel, simulator.agents_ [agents[i].index]);
					//Debug.Log ("GO");

				}
			}

		}
	}


	public void Drive(GameObject unit, int agentNo, RVO.Vector2 dir){
		
		simulator.agents_ [agentNo].prefVelocity_ = dir;

	}

	public void Border(){
		List<RVO.Vector2> _obstacles = new List<RVO.Vector2>();
		Vector3[] V = Borderer.GranList.ToArray ();

		Debug.Log (V.Length);

		for (int i = 0; i < V.Length; i += 2) {
			_obstacles.Clear();
			_obstacles.Add (new RVO.Vector2 (V [i].x, V [i].z));
			_obstacles.Add (new RVO.Vector2 (V [i+1].x, V [i+1].z));
			//Debug.Log ("SuperBefore " + simulator.obstacles_.Count);
			simulator.addObstacle(_obstacles);
			//Debug.Log ("SUperAfter " + simulator.obstacles_.Count);

		}

		simulator.processObstacles();
	}


	void ExchangeAgents(int agent1, int agent2)
	{
		agents [agent1].index = agent2;
		agents [agent2].index = agent1;

		RVO.Agent agentTemp = simulator.agents_ [agent1];
		simulator.agents_ [agent1] = simulator.agents_ [agent2];
		simulator.agents_ [agent2] = agentTemp;

		MinimalPhysicAgent tempAgent = agents [agent1];
		agents [agent1] = agents [agent2];
		agents [agent2] = tempAgent;


	}

	public void RemoveAgent(MinimalPhysicAgent removeThis)
	{
		GameObject _wreckage = Instantiate (wreckage, removeThis.body.transform.position, removeThis.body.transform.rotation) as GameObject;
		//ExchangeAgents(liveUnits -1, removeThis.index);
		//simulator.agents_ [liveUnits - 1].position_ = new RVO.Vector2 (10000, 10000);
		//simulator.agents_ [liveUnits - 1].position_ = VectorConvert (agents [liveUnits - 1].body.transform.position);
		//agents [liveUnits - 1].body.GetComponent<ORCAController> ().AgentChose (removeThis.index);
		//agents [liveUnits - 1].index = removeThis.index;
		simulator.agents_[removeThis.index].active = false;
		simulator.agents_ [removeThis.index].position_ = new RVO.Vector2 (10000, 10000);
		agents.Remove(removeThis);
		liveUnits--;

	}

	void DebugDraw(int i, Vector3 nullPoint, Vector3 prefVel, RVO.Agent agent)
	{
		Debug.DrawRay(agents[i].body.transform.position, new Vector3(agent.velocity_.x_, 0, agent.velocity_.y_), Color.red);
		Debug.DrawRay(agents[i].body.transform.position, prefVel, Color.blue);

		Debug.DrawLine( new Vector3 (simulator.agents_ [agents[i].index].position_.x_ + simulator.agents_ [agents[i].index].radius_, 0, simulator.agents_ [agents[i].index].position_.y_ + simulator.agents_ [agents[i].index].radius_) , 
			new Vector3 (simulator.agents_ [agents[i].index].position_.x_ + simulator.agents_ [agents[i].index].radius_, 0, simulator.agents_ [agents[i].index].position_.y_ - simulator.agents_ [agents[i].index].radius_),
			Color.red);

		Debug.DrawLine( new Vector3 (simulator.agents_ [agents[i].index].position_.x_ + simulator.agents_ [agents[i].index].radius_, 0, simulator.agents_ [agents[i].index].position_.y_ - simulator.agents_ [agents[i].index].radius_) , 
			new Vector3 (simulator.agents_ [agents[i].index].position_.x_ - simulator.agents_ [agents[i].index].radius_, 0, simulator.agents_ [agents[i].index].position_.y_ - simulator.agents_ [agents[i].index].radius_),
			Color.red);

		Debug.DrawLine( new Vector3 (simulator.agents_ [agents[i].index].position_.x_ - simulator.agents_ [agents[i].index].radius_, 0, simulator.agents_ [agents[i].index].position_.y_ - simulator.agents_ [agents[i].index].radius_) , 
			new Vector3 (simulator.agents_ [agents[i].index].position_.x_ - simulator.agents_ [agents[i].index].radius_, 0, simulator.agents_ [agents[i].index].position_.y_ + simulator.agents_ [agents[i].index].radius_),
			Color.red);

		Debug.DrawLine( new Vector3 (simulator.agents_ [agents[i].index].position_.x_ - simulator.agents_ [agents[i].index].radius_, 0, simulator.agents_ [agents[i].index].position_.y_ + simulator.agents_ [agents[i].index].radius_) , 
			new Vector3 (simulator.agents_ [agents[i].index].position_.x_ + simulator.agents_ [agents[i].index].radius_, 0, simulator.agents_ [agents[i].index].position_.y_ + simulator.agents_ [agents[i].index].radius_),
			Color.red);


		foreach (Vector3 point in agents[i].navMeshAgent.path.corners)
		{
			Debug.DrawLine(nullPoint, point);
			nullPoint = point;
		}
	}

}


