using UnityEngine;
using System.Collections;
using System;

public class StateMachine : MonoBehaviour {
    public NavMeshAgent agent;
    public StateSet stateSet;
    public TargetSeek targetSeeker;
    public float optimalDistance;
	public float chaseDistance;
	public float fearDistance;

	
    void Update()
    {
        if(stateSet != null){
           // Debug.Log(stateSet.currentState.GetType());
            stateSet.currentState.Behavior();
            if (targetSeeker.chosenTarget != null)
            {
                //Debug.Log("time to agro!" + (stateSet == stateSet.agro.set));
                stateSet.currentState = stateSet.currentState.Transition(stateSet.agro, targetSeeker.chosenTarget.transform);
            }
        }
    }

    public void SetupStateSet(Vector3 pointDown, Vector3 pointDeploy, Vector3 pointMarch, GameObject enemySF)
    {

        agent = GetComponentInChildren<NavMeshAgent>();
        targetSeeker = GetComponent<TargetSeek>();
        stateSet = new StateSet();
        //Debug.Log(pointDown);
        stateSet.down = new StateDown(agent, pointDown, transform, ref stateSet);
        stateSet.deploy = new StateDeploy(agent, pointDeploy, transform, ref stateSet);
        stateSet.marsh = new StateMarsh(agent, pointMarch, transform, ref stateSet);
        stateSet.assault = new StateAssault(agent, enemySF.transform.position, transform, ref stateSet);
        stateSet.agro = new StateAgro(agent, enemySF.transform.position, transform, ref stateSet);
        stateSet.agro.optimalDistance = optimalDistance;
		stateSet.agro.chaseDistance = chaseDistance;
		stateSet.agro.fearDistance = fearDistance;
        stateSet.currentState = stateSet.down;
    }
}

[System.Serializable]
public abstract class State
{
    protected float stopDistance = 7;
    protected Vector3 point;
    protected NavMeshAgent agent;
    protected Transform transform;
    public StateSet set;

    public State()
    {
        point = Vector3.zero;
        agent = new NavMeshAgent();
    }
    public State(NavMeshAgent _agent, Vector3 _point, Transform _transform, ref StateSet _set)
    {
        set = _set;
        transform = _transform;
        agent = _agent;
        point = _point;
    }

    public abstract void Behavior();
    public abstract State Transition(StateDown newState);
    public abstract State Transition(StateDeploy newState);
    public abstract State Transition(StateMarsh newState);
    public abstract State Transition(StateAssault newState);
    public abstract State Transition(StateAgro newState, Transform enemy);
}

public class StateSet
{
    public StateDown down; //отвечает за съезжание с трапа
    public StateDeploy deploy; // Высадка
    public StateMarsh marsh; //Марш
    public StateAssault assault; //Штурм
    public StateAgro agro; //Агра
    public State currentState;//Текущее состояние
}

public class StateDown : State
{
    public StateDown(NavMeshAgent _agent, Vector3 _point, Transform _transform, ref StateSet _set)
    {
        set = _set;
        transform = _transform;
        agent = _agent;
        point = _point;
        //Debug.Log(point);
    }
    public override void Behavior()
    {
        //Debug.Log("Down"+" " + (point - transform.position).sqrMagnitude);
        agent.SetDestination(point);
		agent.GetComponentInParent<ORCAController> ().steer = 0;
		agent.GetComponentInParent<ORCAController> ().gas = 1;
        if((point - transform.position).sqrMagnitude < stopDistance * stopDistance)
        {
            set.currentState = Transition(set.deploy);
        }
    }

    public override State Transition(StateAgro newState, Transform enemy)
    {
        Debug.Log("Such a transition is impossible");
        return this;
    }
    public override State Transition(StateDown newState)
    {
        return newState;
    }
    public override State Transition(StateDeploy newState)
    {
        return newState;
    }
    public override State Transition(StateMarsh newState)
    {
        Debug.Log("Such a transition is impossible");
        return this;
    }
    public override State Transition(StateAssault newState)
    {
        Debug.Log("Such a transition is impossible");
        return this;
    }
}

public class StateDeploy : State
{
    public StateDeploy(NavMeshAgent _agent, Vector3 _point, Transform _transform, ref StateSet _set)
    {
        set = _set;
        transform = _transform;
        agent = _agent;
        point = _point;
    }
    public override void Behavior()
    {
       //Debug.Log("Deploy" + " " + (point - transform.position).sqrMagnitude);
        agent.SetDestination(point);
        if ((point - transform.position).sqrMagnitude < stopDistance * stopDistance)
        {
            set.currentState = Transition(set.marsh);
        }
    }

    public override State Transition(StateAgro newState, Transform enemy)
    {
        Debug.Log("Such a transition is impossible");
        return this;
    }
    public override State Transition(StateDown newState)
    {
        Debug.Log("Such a transition is impossible");
        return this;
    }
    public override State Transition(StateDeploy newState)
    {
        return newState;
    }
    public override State Transition(StateMarsh newState)
    {
        return newState;
    }

    public override State Transition(StateAssault newState)
    {
        Debug.Log("Such a transition is impossible");
        return this;
    }

}

public class StateMarsh : State
{
    public StateMarsh(NavMeshAgent _agent, Vector3 _point, Transform _transform, ref StateSet _set)
    {
        set = _set;
        transform = _transform;
        agent = _agent;
        point = _point;
    }
    public override void Behavior()
    {
        //Debug.Log("Marsh" + " " + (point - transform.position).sqrMagnitude);
        agent.SetDestination(point);
        if ((point - transform.position).sqrMagnitude < stopDistance * stopDistance)
        {
            set.currentState = Transition(set.assault);
        }
    }

    public override State Transition(StateAgro newState, Transform enemy)
    {
        set.agro.setPrevState(set.marsh);
        set.agro.setEnemy(enemy);
        return newState;
    }
    public override State Transition(StateDown newState)
    {
        Debug.Log("Such a transition is impossible");
        return this;
    }
    public override State Transition(StateDeploy newState)
    {
        Debug.Log("Such a transition is impossible");
        return this;
    }
    public override State Transition(StateMarsh newState)
    {
        return newState;
    }

    public override State Transition(StateAssault newState)
    {
        return newState;
    }

}

public class StateAssault : State
{
    public StateAssault(NavMeshAgent _agent, Vector3 _point, Transform _transform, ref StateSet _set)
    {
        set = _set;
        transform = _transform;
        agent = _agent;
        point = _point;
    }
    public override void Behavior()
    {
        //Debug.Log("Assault" + " " + (point - transform.position).sqrMagnitude);
        agent.SetDestination(point);
    }
    public override State Transition(StateAgro newState, Transform enemy)
    {
        set.agro.setPrevState(set.assault);
        set.agro.setEnemy(enemy);
        return newState;
    }
    public override State Transition(StateDown newState)
    {
        Debug.Log("Such a transition is impossible");
        return this;
    }
    public override State Transition(StateDeploy newState)
    {
        Debug.Log("Such a transition is impossible");
        return this;
    }
    public override State Transition(StateMarsh newState)
    {
        return newState;
    }

    public override State Transition(StateAssault newState)
    {
        return newState;
    }

}

public class StateAgro : State
{
    State prevState;
    Transform enemy;
    public float optimalDistance;
	public float chaseDistance;
	public float fearDistance;

    public void setEnemy(Transform _enemy)
    {
        enemy = _enemy;
    }
    public void setPrevState(State _state)
    {
        prevState = _state;
    }

    public StateAgro(NavMeshAgent _agent, Vector3 _point, Transform _transform, ref StateSet _set)
    {
        set = _set;
        transform = _transform;
        agent = _agent;
        point = _point;
    }
    public override void Behavior()
    {
        //Debug.Log("Agro"+ (enemy == null));
        if (enemy != null)
        {
			if (((enemy.position - transform.position).sqrMagnitude > chaseDistance * chaseDistance) || ((enemy.position - transform.position).sqrMagnitude < fearDistance * fearDistance)) {
				agent.SetDestination (enemy.position + (transform.position - enemy.position).normalized * optimalDistance);
			} else {
				agent.SetDestination(agent.transform.position);
			}
        } else
        {
            set.currentState = Transition(set.marsh);
        }

    }
    public override State Transition(StateAgro newState, Transform enemy)
    {
        set.agro.setPrevState(set.currentState);
        set.agro.setEnemy(enemy);
        return newState;
    }
    public override State Transition(StateDown newState)
    {
        Debug.Log("Such a transition is impossible");
        return this;
    }
    public override State Transition(StateDeploy newState)
    {
        Debug.Log("Such a transition is impossible");
        return this;
    }
    public override State Transition(StateMarsh newState)
    {
        return newState;
    }

    public override State Transition(StateAssault newState)
    {
        return newState;
    }

}
