using UnityEngine;
using System.Collections;

public class TestController : MonoBehaviour
{
    public SteamCitadel MyCitadel;
	// Use this for initialization
	void Start () {

        //MyCitadel.Modules[0].Subs.Add(new Subsystem(MyCitadel.Name, "Platform1", "Tube"));
    }

    public void NewCitadel()
    {
        MyCitadel = new SteamCitadel("PlayersSC");
        MyCitadel.Modules.Add(new Module(MyCitadel.Name, "Platform1"));
        MyCitadel.Modules.Add(new Module(MyCitadel.Name, "Engine1"));
        MyCitadel.Modules.Add(new Module(MyCitadel.Name, "Bridge1"));
        MyCitadel.Modules.Add(new Module(MyCitadel.Name, "Storage1"));
        MyCitadel.Modules.Add(new Module(MyCitadel.Name, "Radar1"));
        MyCitadel.Modules.Add(new Module(MyCitadel.Name, "Weapon1"));

    }

    void BuildCitadel()
    {
        
    }
	
	// Update is called once per frame
	void Update () {
	
        
       
	}
}
