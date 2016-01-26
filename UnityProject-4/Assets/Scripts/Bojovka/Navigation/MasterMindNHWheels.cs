using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RVO;

public class MasterMindNHWheels : MonoBehaviour
{

    int layerMask = ~(1 << 8);
    public Transform spawnArea;
    public GameObject[] units;
    public Simulator simulator = Simulator.Instance;
    public List<GameObject> agentsInScene;
    public List<Agent> agents;
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

    float timer = 5;

    // Use this for initialization
    void Start()
    {
        ProcessObstacles();
        
        simulator.setAgentDefaults(0, 200, 10.0f, 5.0f, 0, 0, new RVO.Vector2(0, 0));
        
        for( int i = 0; i < manipulationSpheresNumber; i++)
        {
            simulator.addAgent(new RVO.Vector2(10000, 10000));
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
        //SpawnUnit();
        
    }

    void SpawnUnit()
    {

        Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit _hit;
        if (Input.GetMouseButtonDown(0) && Physics.Raycast(_ray, out _hit, Mathf.Infinity, layerMask) && (_hit.point - spawnArea.position).sqrMagnitude < spawnArea.localScale.x * spawnArea.localScale.x / 4)
        {
            GameObject _unit = Instantiate(unit, _hit.point, Quaternion.identity) as GameObject;
            AddAgent(_unit, goal.position);
        }
    }
    
    float CalculateError(RVO.Agent agent, float angle)
    {
        float t = Time.deltaTime;
        float V2 = agent.velocity_.x_ * agent.velocity_.x_ + agent.velocity_.y_ * agent.velocity_.y_;
        float V = Mathf.Sqrt(V2);
        return (Mathf.Sqrt(maxSpeed * maxSpeed * t * t - 2 * maxSpeed * t * Mathf.Sin(angle) / maxAngularSpeed * V + 2 * (1 - Mathf.Cos(angle)) / maxAngularSpeed / maxAngularSpeed * V2));
        
    }
    
    public void AddAgent(GameObject newAgent, Vector3 position)
    {
       
        agents.Add(new Agent(newAgent, goal.position, liveUnits++));

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
		for (int i = 0; i < V.Length; i += 2) {
			_obstacles.Clear();
			_obstacles.Add (new RVO.Vector2 (V [i].x, V [i].z));
			_obstacles.Add (new RVO.Vector2 (V [i+1].x, V [i+1].z));
			simulator.addObstacle(_obstacles);
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

    public void RemoveAgent(Agent removeThis)
    {
        ExchangeAgents(simulator.agents_[liveUnits -1], simulator.agents_[removeThis.index]);
        agents.Remove(removeThis);
        liveUnits--;

    }
}

[System.Serializable]
public class Agent
{
    public int index;
    public GameObject body;
    public SimpleController Controller;
    public NavMeshAgent navMeshAgent;
    public Vector3 goal;
    public Rigidbody rigBody;
    public Agent(GameObject _body, Vector3 _goal, int _index)
    {
        body = _body;
        rigBody = body.GetComponent<Rigidbody>();
        Controller = body.GetComponent<SimpleController>();
        navMeshAgent = body.GetComponentInChildren<NavMeshAgent>();
        goal = _goal;
        navMeshAgent.SetDestination(goal);
        index = _index;

    }
}