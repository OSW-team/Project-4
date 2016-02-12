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

		simulator.setAgentDefaults(0, 200, 10.0f, 5.0f, 0, 0, new RVO.Vector2(0, 0));

		for( int i = 0; i < manipulationSpheresNumber; i++)
		{
			simulator.addAgent(new RVO.Vector2(10000, 10000));
			simulator.agents_ [i].active = true;
		}

		foreach (GameObject agent in agentsInScene)
		{
			AddAgent(agent, goal.transform.position);
		}


	}

	void Update()
	{
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
		return (Mathf.Sqrt(maxSpeed * maxSpeed * t * t - 2 * maxSpeed * t * Mathf.Sin(angle) / maxAngularSpeed * V + 2 * (1 - Mathf.Cos(angle)) / maxAngularSpeed / maxAngularSpeed * V2));

	}

	public void AddAgent(GameObject newAgent, Vector3 position)
	{

		agents.Add(new MinimalPhysicAgent(newAgent, position, liveUnits++));

		RVO.Agent _agent = simulator.agents_[liveUnits - 1];
		_agent.radius_ = radius;
		_agent.maxSpeed_ = maxSpeed;
		_agent.neighborDist_ = 100;
		newAgent.GetComponent<UnitStats>().meAgent = agents[liveUnits-1];
	}

	void RVOAgentsCalculation()
	{
		for(int i = 0; i < liveUnits; i ++)
		{
			Vector3 nullPoint = agents[i].navMeshAgent.path.corners[0];
			/*if(agents [i].navMeshAgent.isOnNavMesh){
				agents [i].navMeshAgent.transform.position = agents [i].body.transform.position;
			}
			Debug.Log (agents [i].navMeshAgent.isOnNavMesh);*/
			if ((agents [i].navMeshAgent.transform.position.x * agents [i].navMeshAgent.transform.position.x + agents [i].navMeshAgent.transform.position.z * agents [i].navMeshAgent.transform.position.z - agents [i].body.transform.position.x * agents [i].body.transform.position.x - agents [i].body.transform.position.z * agents [i].body.transform.position.z) 
				> 1) {
				agents [i].body.transform.position += (new Vector3 (agents [i].navMeshAgent.transform.position.x, agents [i].body.transform.position.y, agents [i].navMeshAgent.transform.position.z) - agents [i].body.transform.position)*Time.deltaTime;
				agents [i].navMeshAgent.transform.localPosition -= agents [i].navMeshAgent.transform.localPosition * Time.deltaTime;
				//agents [i].body.transform.position += (agents [i].navMeshAgent.transform.position - agents [i].body.transform.position) / 2 * Time.deltaTime;
			}
			Vector3 prefVel = (agents[i].navMeshAgent.steeringTarget - agents[i].body.transform.position).normalized * maxSpeed;

			simulator.agents_[i].prefVelocity_ = new RVO.Vector2(prefVel.x, prefVel.z);
			simulator.agents_[i].radius_ = radius + CalculateError(simulator.agents_[i], Vector3.Angle(new Vector3(simulator.agents_[i].velocity_.x_, 0, simulator.agents_[i].velocity_.y_), agents[i].body.transform.forward));
			simulator.agents_[i].position_ = new RVO.Vector2(agents[i].body.transform.position.x, agents[i].body.transform.position.z);


			DebugDraw(i, nullPoint, new Vector3(simulator.agents_[i].prefVelocity_.x_, 0, simulator.agents_[i].prefVelocity_.y_), simulator.agents_[i]);
			Steering(i);

		}
	}

	void Steering(int i)
	{
		RVO.Agent agent = simulator.agents_[i];
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

	void DebugDraw(int i, Vector3 nullPoint, Vector3 prefVel, RVO.Agent agent)
	{
		Debug.DrawRay(agents[i].body.transform.position, new Vector3(agent.velocity_.x_, 0, agent.velocity_.y_), Color.red);
		Debug.DrawRay(agents[i].body.transform.position, prefVel, Color.blue);
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

		if (!navMeshAgent.SetDestination (goal)) {
			navMeshAgent.enabled = false;
			navMeshAgent.enabled = true;
			navMeshAgent.SetDestination (goal);
		}


		index = _index;

	}
}
