using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RVO;

public class MasterMindTranslate : MonoBehaviour {
	int layerMask = ~(1 << 8);
	public Transform spawnArea;
	public GameObject[] units;
	public Simulator simulator = Simulator.Instance;
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

	// Use this for initialization
	void Start()
	{
		ProcessObstacles();

		simulator.setAgentDefaults(radius*10, 20, 10.0f, 10.0f, radius, maxSpeed, new RVO.Vector2(0, 0));

		for( int i = 0; i < manipulationSpheresNumber; i++)
		{
			simulator.addAgent(new RVO.Vector2(10000, 10000));
			simulator.agents_ [i].active = false;
		}

		foreach (GameObject agent in agentsInScene)
		{
			AddAgent(agent, goal.transform.position);
		}


	}

	void Update()
	{
		for(var i = 0; i < simulator.obstacles_.Count; i++){
			Debug.DrawLine( VectorConvert( simulator.obstacles_[i].point_), VectorConvert(simulator.obstacles_[i].nextObstacle.point_), Color.green);
		}
		simulator.setTimeStep(Time.deltaTime);
		RVOAgentsCalculation();
		simulator.doStep();

	}

	/*void SpawnUnit()
    {

        Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit _hit;
        if (Input.GetMouseButtonDown(0) && Physics.Raycast(_ray, out _hit, Mathf.Infinity, layerMask) && (_hit.point - spawnArea.position).sqrMagnitude < spawnArea.localScale.x * spawnArea.localScale.x / 4)
        {
            GameObject _unit = Instantiate(unit, _hit.point, Quaternion.identity) as GameObject;
            AddAgent(_unit, goal.position);
        }
    }*/

	float CalculateError(RVO.Agent agent, float angle)
	{
		float t = Time.deltaTime;
		float V2 = agent.velocity_.x_ * agent.velocity_.x_ + agent.velocity_.y_ * agent.velocity_.y_;
		float V = Mathf.Sqrt(V2);
		return (Mathf.Sqrt(maxSpeed * maxSpeed * t * t - 2 * maxSpeed * t * Mathf.Sin(angle) / maxAngularSpeed * V + 2 * (1 - Mathf.Cos(angle)) / maxAngularSpeed / maxAngularSpeed * V2)) * maxSpeed;

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
			newAgent.GetComponent<UnitStats> ().meAgent = agents[agents.Count - 1];
		} else {
			Debug.Log ("NoFreeAgent");
		}
	}

	void RVOAgentsCalculation()
	{
		for(int i = 0; i < liveUnits; i ++)
		{
			if (simulator.agents_ [agents [i].index] != null) {
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

					prefVel = (agents [i].navMeshAgent.steeringTarget - agents [i].body.transform.position).normalized * maxSpeed;
					//Debug.Log(prefVel.magnitude);

				}

				if (agents [i].body != null) {
					simulator.agents_ [agents [i].index].prefVelocity_ = new RVO.Vector2 (prefVel.x, prefVel.z);
					simulator.agents_ [agents [i].index].radius_ = radius + CalculateError (simulator.agents_ [agents [i].index], Vector3.Angle (new Vector3 (simulator.agents_ [agents [i].index].velocity_.x_, 0, simulator.agents_ [agents [i].index].velocity_.y_), agents [agents [i].index].body.transform.forward));
					simulator.agents_ [agents [i].index].position_ = new RVO.Vector2 (agents [i].body.transform.position.x, agents [i].body.transform.position.z);
				

					DebugDraw (i, nullPoint, VectorConvert(simulator.agents_[agents [i].index].prefVelocity_), simulator.agents_ [agents [i].index]);
					Steering (i);
				}
			}

		}
	}

	void Steering(int i)
	{
		RVO.Agent agent = simulator.agents_[agents [i].index];
		agents[i].Controller.Steering(new UnityEngine.Vector2(agent.velocity_.x_, agent.velocity_.y_), agent.maxSpeed_);
	}

	void ProcessObstacles()
	{
		List<RVO.Vector2> _obstacles = new List<RVO.Vector2>();
		foreach (GameObject obstacle in obstacles)
		{


			_obstacles.Add(new RVO.Vector2(obstacle.transform.position.x + obstacle.transform.localScale.x / 2, obstacle.transform.position.z + obstacle.transform.localScale.z / 2));
			_obstacles.Add(new RVO.Vector2(obstacle.transform.position.x + obstacle.transform.localScale.x / 2, obstacle.transform.position.z - obstacle.transform.localScale.z / 2));
			simulator.addObstacle(_obstacles);
			_obstacles.Clear();

			_obstacles.Add(new RVO.Vector2(obstacle.transform.position.x + obstacle.transform.localScale.x / 2, obstacle.transform.position.z - obstacle.transform.localScale.z / 2));
			_obstacles.Add(new RVO.Vector2(obstacle.transform.position.x - obstacle.transform.localScale.x / 2, obstacle.transform.position.z - obstacle.transform.localScale.z / 2));
			simulator.addObstacle(_obstacles);
			_obstacles.Clear();

			_obstacles.Add(new RVO.Vector2(obstacle.transform.position.x - obstacle.transform.localScale.x / 2, obstacle.transform.position.z - obstacle.transform.localScale.z / 2));
			_obstacles.Add(new RVO.Vector2(obstacle.transform.position.x - obstacle.transform.localScale.x / 2, obstacle.transform.position.z + obstacle.transform.localScale.z / 2));
			simulator.addObstacle(_obstacles);
			_obstacles.Clear();

			_obstacles.Add(new RVO.Vector2(obstacle.transform.position.x - obstacle.transform.localScale.x / 2, obstacle.transform.position.z + obstacle.transform.localScale.z / 2));
			_obstacles.Add(new RVO.Vector2(obstacle.transform.position.x + obstacle.transform.localScale.x / 2, obstacle.transform.position.z + obstacle.transform.localScale.z / 2));
			simulator.addObstacle(_obstacles);
			_obstacles.Clear();

			Debug.DrawLine(new Vector3( obstacle.transform.position.x + obstacle.transform.localScale.x / 2, 1, obstacle.transform.position.z + obstacle.transform.localScale.z / 2), new Vector3(obstacle.transform.position.x + obstacle.transform.localScale.x / 2, 1, obstacle.transform.position.z - obstacle.transform.localScale.z / 2), Color.blue, 1000);
			Debug.DrawLine(new Vector3(obstacle.transform.position.x + obstacle.transform.localScale.x / 2, 1, obstacle.transform.position.z - obstacle.transform.localScale.z / 2), new Vector3(obstacle.transform.position.x - obstacle.transform.localScale.x / 2, 1, obstacle.transform.position.z - obstacle.transform.localScale.z / 2), Color.blue, 1000);
			Debug.DrawLine(new Vector3(obstacle.transform.position.x - obstacle.transform.localScale.x / 2, 1, obstacle.transform.position.z - obstacle.transform.localScale.z / 2), new Vector3(obstacle.transform.position.x - obstacle.transform.localScale.x / 2,1, obstacle.transform.position.z + obstacle.transform.localScale.z / 2), Color.blue, 1000);
			Debug.DrawLine(new Vector3(obstacle.transform.position.x - obstacle.transform.localScale.x / 2, 1, obstacle.transform.position.z + obstacle.transform.localScale.z / 2), new Vector3(obstacle.transform.position.x + obstacle.transform.localScale.x / 2,1, obstacle.transform.position.z + obstacle.transform.localScale.z / 2), Color.blue, 1000);

			simulator.addObstacle(_obstacles);

		}
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

	public Vector3 VectorConvert(RVO.Vector2 vector){
		return new Vector3 (vector.x_, 0, vector.y_);
	}
	public RVO.Vector2 VectorConvert (Vector3 vector){
		RVO.Vector2 _vector = new RVO.Vector2 ();
		_vector.x_ = vector.x;
		_vector.y_ = vector.z;
		return _vector;
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

	void ExchangeAgents(RVO.Agent agent1, RVO.Agent agent2)
	{
		RVO.Agent agentTemp = agent1;
		agent1 = agent2;
		agent2 = agentTemp;
	}

	public void RemoveAgent(MinimalPhysicAgent removeThis)
	{
		GameObject _wreckage = Instantiate (wreckage, removeThis.body.transform.position, removeThis.body.transform.rotation) as GameObject;
		ExchangeAgents(simulator.agents_[liveUnits -1], simulator.agents_[removeThis.index]);
		agents.Remove(removeThis);
		liveUnits--;

	}
}

[System.Serializable]
public class MinimalPhysicAgent
{
	public int index;
	public GameObject body;
	public TranslateController Controller;
	public NavMeshAgent navMeshAgent;
	public Vector3 goal;
	public MinimalPhysicAgent(GameObject _body, Vector3 _goal, int _index)
	{
		body = _body;
		Controller = body.GetComponent<TranslateController>();
		navMeshAgent = body.GetComponentInChildren<NavMeshAgent>();
		goal = _goal;
		int k = 0;

		navMeshAgent.enabled = false;
		navMeshAgent.enabled = true;
		if (!navMeshAgent.SetDestination (goal)) {
			navMeshAgent.enabled = false;
			navMeshAgent.enabled = true;
			navMeshAgent.SetDestination (goal);
		}


		index = _index;

	}
}
