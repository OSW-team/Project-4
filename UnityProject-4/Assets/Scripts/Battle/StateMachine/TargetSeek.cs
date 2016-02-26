using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TargetSeek : MonoBehaviour {
	//MasterMindTranslate master;
	SynchronizeORCA master;
	List<MinimalPhysicAgent> agents;
    public GameObject chosenTarget;
    GunModule gun;
    UnitStats stats;
    public float seeDistance;
    public int[] priority;

    // Use this for initialization
    void Start () {
		//master = GameObject.FindWithTag("masterMind").GetComponent<MasterMindTranslate>();
		master = GameObject.FindWithTag("masterMind").GetComponent<SynchronizeORCA>();
        stats = GetComponentInParent<UnitStats>();
        agents = master.agents;
        gun = GetComponent<GunModule>();
	}
	
	// Update is called once per frame
	void Update () {
		foreach(MinimalPhysicAgent unit in agents)
        {
			if (unit != null && unit.body != null && unit.body.GetComponent<UnitStats>().team != stats.team && (unit.body.transform.position - transform.position).sqrMagnitude < seeDistance* seeDistance ) {
                if (chosenTarget != null && priority[unit.body.GetComponent<UnitStats>().type] > priority[chosenTarget.GetComponent<UnitStats>().type])
                {
                    chosenTarget = unit.body;
                    Debug.Log("Big priority");
                } else if (chosenTarget == null || (transform.position - chosenTarget.transform.position).sqrMagnitude > (transform.position - unit.body.transform.position).sqrMagnitude)
                {
                    chosenTarget = unit.body;
                }
            }
        }
        
	}
}
