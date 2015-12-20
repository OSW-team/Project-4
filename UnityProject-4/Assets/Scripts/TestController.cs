using UnityEngine;
using System.Collections;
using System.Linq;

public class TestController : MonoBehaviour
{
    public Player Player;
    public SteamCitadel MyCitadel;
	// Use this for initialization
	void Start () {
        //NewCitadel();
        Player = new Player();
        Player.Subsystems.Add(new Subsystem("Box1"));
        Player.Subsystems.Add(new Subsystem("Tube1"));
        var citadel = new SteamCitadel("PlayersSC");
	    Player.Citadel = citadel;
        XMLWorker.LoadSC(citadel);
        SteamCitadelMeshConstrutor.BuildCitadelMesh(citadel);
        var moduleScreen = FindObjectOfType<MangeSteamCitadelScreenController>();
        moduleScreen.ShowCitadel(citadel);
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
        MyCitadel.Modules.FirstOrDefault(x=>x.Workmame == "Engine1").Subs.Add(new Subsystem(MyCitadel.Name, "Engine1", "Tube1", "Sub_Slot_A1 2"));


    }

	
	// Update is called once per frame
	void Update () {
	
        
       
	}
}
