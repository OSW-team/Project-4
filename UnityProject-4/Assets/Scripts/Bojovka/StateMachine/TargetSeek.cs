﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TargetSeek : MonoBehaviour {
    MasterMindNHWheels master;
    List<Agent> agents;
    public GameObject chosenTarget;
    GunModule gun;
    UnitStats stats;
    public float seeDistance;
    public int[] priority;

    // Use this for initialization
    void Start () {
        master = GameObject.FindWithTag("masterMind").GetComponent<MasterMindNHWheels>();
        stats = GetComponentInParent<UnitStats>();
        agents = master.agents;
        gun = GetComponent<GunModule>();
	}
	
	// Update is called once per frame
	void Update () {
	    foreach(Agent unit in agents)
        {
            if (unit.body.GetComponent<UnitStats>().team != stats.team && (unit.body.transform.position - transform.position).sqrMagnitude < seeDistance* seeDistance ) {
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