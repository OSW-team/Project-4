using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class TestController : MonoBehaviour
{
    public Player Player;
    public SteamCitadel MyCitadel;
    public Transform ScreensButtons;
    public List<GameObject> Screens;
    public GameObject MainCamera;
    public GameObject ManualCamera;
    private bool _isCameraManual;
    private GameObject _lastUnitGO;

    // Use this for initialization
    void Start () {
        //NewCitadel();

        Player = new Player();
        Player.Subsystems.Add(new Subsystem("Box1"));
        Player.Subsystems.Add(new Subsystem("Tube1"));
        var citadel = new SteamCitadel("PlayersSC");
        Player.Citadel = citadel;
        XMLWorker.LoadSC(citadel);

        //ShowManagementScreen();


        MyCitadel = citadel;
    }

    public void ShowManagementScreen()
    {
        var moduleScreen = FindObjectOfType<MangeSteamCitadelScreenController>();
        var unitScreen = FindObjectOfType<ManageUnitsScreenController>();
        ScreensButtons.GetChild(1).GetComponent<Button>().onClick.RemoveAllListeners();
        ScreensButtons.GetChild(1).GetComponent<Button>().onClick.AddListener(() =>
        {
            moduleScreen.gameObject.SetActive(false);
            unitScreen.ShowUnit(MyCitadel.Units[0]);
        });
        ScreensButtons.GetChild(0).GetComponent<Button>().onClick.RemoveAllListeners();
        ScreensButtons.GetChild(0).GetComponent<Button>().onClick.AddListener(() =>
        {
            unitScreen.gameObject.SetActive(false);
            moduleScreen.ShowCitadel(MyCitadel);
        });
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
        MyCitadel.Modules.FirstOrDefault(x=>x.Workname == "Engine1").Subs.Add(new Subsystem(MyCitadel.Name, "Engine1", "Tube1", "Sub_Slot_A1 2"));
        MyCitadel.Units.Add(new Unit(MyCitadel.Name, "Unit1"));
        MyCitadel.Units[0].Upgrades.Add(new UnitUpgrade(MyCitadel.Name, "Unit1","Upg1"));
        //XMLWorker.SaveUnit(MyCitadel.Name, "Unit1", new List<string>(1) { "Upg1" });


    }

    public void UpgradeChange(Unit unit)
    {

        var upgradableParts = unit.GO.GetComponentsInChildren<UpgradeController>().ToList();
        foreach (var part in upgradableParts)
        {
            part.CheckUpgrades();
        }
    }

    // Update is called once per frame
    void Update () {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SwitchUnitControl();
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            //MyCitadel.Units[0].Upgrades[1].Boosters[0].Activate();
            //MyCitadel.Units[0].RecountProps();
            MyCitadel.Units[0].EnabledBoosters.Add("NewBooster");
            XMLWorker.SaveSC(MyCitadel);
        }
    }

    void SwitchUnitControl()
    {

        if (MyCitadel.GO.GetComponent<SpawnUnit>().SpawnedUnitsGameObjects.Count > 0)
        {
            _isCameraManual = !_isCameraManual;

            Debug.Log(_isCameraManual + "isManual");
            MainCamera.SetActive(!_isCameraManual);
            var unit = MyCitadel.GO.GetComponent<SpawnUnit>().SpawnedUnitsGameObjects.Last();
            if (!_isCameraManual) { unit = _lastUnitGO; }
            ManualCamera.GetComponent<CameraControl>().target = unit.transform;
            unit.GetComponent<BasicInput>().enabled = _isCameraManual;
            var acc = unit.GetComponent<AdwancedCarController>();
            if (acc != null)
            {
                acc.enabled = !_isCameraManual;
                if (acc.tail != null) { acc.tail.gameObject.GetComponentInChildren<TailSteering>().enabled = _isCameraManual; }
            }
            var atc = unit.GetComponent<AdwancedTankController>();
            if (atc != null)
            {
                atc.enabled = !_isCameraManual;
                unit.GetComponent<TestGears>().enabled = _isCameraManual;
                unit.GetComponent<TestTorque>().enabled = _isCameraManual;
            }
            unit.GetComponent<BasicInput>().enabled = _isCameraManual;
            _lastUnitGO = unit;
            ManualCamera.SetActive(_isCameraManual);
        }
        
    }
}
