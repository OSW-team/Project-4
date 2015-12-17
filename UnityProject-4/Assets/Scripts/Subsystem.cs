using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Subsystem:MonoBehaviour 
{
    public string Workname;
    public List<string> Compatibility;
    public List<bool> BuildConditions;  
    public List<string> upgradeability;  
	// Use this for initialization
    public Subsystem(string CitadelName, string ModuleName, string SubName)
    {
        XMLWorker.SaveSubsystem(CitadelName, ModuleName, SubName);
    }
}
