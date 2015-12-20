using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    public SteamCitadel Citadel;
    public List<Module> Modules; 
    public List<Subsystem> Subsystems;
    // Use this for initialization
    public Player()
    {
        Modules = new List<Module>();
        Subsystems = new List<Subsystem>();
    }
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
