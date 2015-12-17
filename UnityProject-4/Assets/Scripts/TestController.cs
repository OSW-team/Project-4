using UnityEngine;
using System.Collections;

public class TestController : MonoBehaviour
{
    public SteamCitadel MyCitadel;
	// Use this for initialization
	void Start () {
        NewCitadel();
        BuildCitadelMesh();
        //MyCitadel.Modules[0].Subs.Add(new Subsystem(MyCitadel.Name, "Platform1", "Tube"));
    }

    public void NewCitadel()
    {
        MyCitadel = new SteamCitadel("PlayersSC");
        MyCitadel.Modules.Add(new Module(MyCitadel.Name, "Platform1", ""));
        MyCitadel.Modules.Add(new Module(MyCitadel.Name, "Constructor1", "Constructor_Site"));
        MyCitadel.Modules.Add(new Module(MyCitadel.Name, "Engine1", "Engine_Site"));
        MyCitadel.Modules.Add(new Module(MyCitadel.Name, "Bridge1", "Bridge_Site"));
        MyCitadel.Modules.Add(new Module(MyCitadel.Name, "Storage1", "Storage_Site"));
        MyCitadel.Modules.Add(new Module(MyCitadel.Name, "Radar1", "Hypo_Site1"));
        MyCitadel.Modules.Add(new Module(MyCitadel.Name, "Weapon1", "Weapon_Site1"));

    }

    void BuildCitadelMesh()
    {       
            SteamCitadelMeshConstrutor.BuildCitadelMesh(MyCitadel);
    }
	
	// Update is called once per frame
	void Update () {
	
        
       
	}
}
