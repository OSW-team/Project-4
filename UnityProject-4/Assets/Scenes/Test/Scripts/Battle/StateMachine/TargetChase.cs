using UnityEngine;
using System.Collections;

public class TargetChase : MonoBehaviour {
    GameObject enemy;
    TargetSeek seeker;
    NavMeshAgent agent;
    public float optimalDistance;
	// Use this for initialization
	void Start () {
        agent = GetComponentInChildren<NavMeshAgent>();
        seeker = GetComponent<TargetSeek>();
	}
	
	// Update is called once per frame
	void Update () {
        enemy = seeker.chosenTarget;
        if(enemy != null)
        {
            agent.SetDestination(enemy.transform.position + (transform.position - enemy.transform.position).normalized * optimalDistance);
        }
	}
}
