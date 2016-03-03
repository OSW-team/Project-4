using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class TestController : MonoBehaviour
{
    public Player Player;
    public SteamCitadel MyCitadel;
    public SteamCitadel EnemyCitadel;
    public SteamCitadel CurrentCitadel;
    public Transform ScreensButtons;
    public List<GameObject> Screens;
    public GameObject MainCamera;
    public GameObject ManualCamera;
    private bool _isCameraManual;
    private GameObject _lastUnitGO;
    private bool citadelToogle;

    // Use this for initialization
    void Start () {
        //NewCitadel();

        Player = new Player();
        Player.Subsystems.Add(new Subsystem("Box1"));
        Player.Subsystems.Add(new Subsystem("Tube1"));
        var citadel = new SteamCitadel("PlayersSC");
        Player.Citadel = citadel;
        XMLWorker.LoadSC(citadel);
        EnemyCitadel = new SteamCitadel("EnemySC1");

        XMLWorker.LoadSC(EnemyCitadel);

        CurrentCitadel = EnemyCitadel;
        MyCitadel = citadel;
        // CurrentCitadel.Inventory.Items.Add(new InventoryItem("Item1"));


        if (Application.loadedLevelName == "ModulesSetupScreen")
        {
            //ShowModulesScreen(CurrentCitadel);
            ShowUnitScreen();
        }

    }

    public void ShowUnitScreen()
    {
        var citadel = MyCitadel;
        CurrentCitadel = MyCitadel;
        if (!citadelToogle)
        {
            citadel = EnemyCitadel;
            CurrentCitadel = EnemyCitadel;
        }
        //var moduleScreen = FindObjectOfType<MangeSteamCitadelScreenController>();
        var unitScreen = FindObjectOfType<ManageUnitsScreenController>();
        //moduleScreen.gameObject.SetActive(false);
        unitScreen.ShowUnit(citadel.Units[0]);
        citadelToogle = !citadelToogle;
    }

    public void ShowModulesScreen(SteamCitadel citadel)
    {
        var moduleScreen = FindObjectOfType<ManageModulesScreen>();
        moduleScreen.ShowCitadel(citadel);
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
        MyCitadel.Units.Add(new Unit("Unit1"));
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
            //MyCitadel.Units[0].EnabledBoosters.Add("NewBooster");
            XMLWorker.SaveSC(CurrentCitadel);
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

    public void LoadLevel()
    {
        SceneManager.LoadScene("DesertMainBattleScene");
    }
}
