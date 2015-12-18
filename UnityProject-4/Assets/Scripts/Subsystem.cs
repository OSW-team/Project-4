using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Subsystem:Module 
{
    public List<string> Compatibility;
    public List<bool> BuildConditions;  
    public List<string> Upgradeability;

	// Use this for initialization
    public Subsystem(string CitadelName, string ModuleName, string subName, string slot)
    {
        Slot = slot;
        //XMLWorker.SaveSubsystem(CitadelName, ModuleName, subName, slot);
        XMLWorker.LoadModule(subName, this);
    }
}
