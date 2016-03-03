using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Unit : MonoBehaviour
{
    public string Workname;
    public string UnitName;
    public string ModelName;
    public string IconName;
    public string UnitDescription;
    public float HpMax;
    public float HpWreckage;
    public float HpRegen;
    public float CostParts;
    public float CostEnergy;
    public float CostSM;
    public float BasicBuildTime;
    public float DeployRadius;
    public float SpeedGlobal;
    public float Speed;
    public float ReverseFactor;
    public float TurnSpeed;
    public float Acceleration;
    public float Accuracy;
    public float Mass;
    public List<float> Recovery;
    public List<GameModelsAndEnums.EnumUnitClass> UnitClasses;
    public Dictionary<GameModelsAndEnums.EnumUnitClass, int> Priorities;
    public float Sight;
    public float OptimalDistance;
    public float RefArmPHYS;
    public float RefArmEN;
    public float RefArmHEAT;
    public float ConArmPHYS;
    public float ConArmEN;
    public float ConArmHEAT;
    public float UpgradeCost;
    public string Category;
    public string UpgradeRequirements;
    public float SchemeCostMetal;
    public float SchemeCostEnergy;

    public GameObject GO;
    public List<UnitUpgrade> Upgrades;
    public List<UnitBooster> Boosters;
    public List<string> EnabledBoosters;

    public Unit(string workName)
    {
        EnabledBoosters = new List<string>();
        Upgrades = new List<UnitUpgrade>();
        Boosters = new List<UnitBooster>();
        XMLWorker.LoadUnit(workName, this);
        RecountProps();
    }

    protected Unit()
    {


    }

    public void BuildMesh()
    {
        //if (GO != null) { Destroy(GO); }
        var prefab = Resources.Load<GameObject>("Prefabs/Units/" + ModelName);
        GO = Instantiate(prefab);
    }

    public void RecountProps()
    {
        HpMax += Upgrades.Sum(unitUpgrade => unitUpgrade.HpMax);
        HpWreckage += Upgrades.Sum(unitUpgrade => unitUpgrade.HpWreckage);
        HpRegen += Upgrades.Sum(unitUpgrade => unitUpgrade.HpRegen);
        CostParts += Upgrades.Sum(unitUpgrade => unitUpgrade.CostParts);
        CostEnergy += Upgrades.Sum(unitUpgrade => unitUpgrade.CostEnergy);
        CostSM += Upgrades.Sum(unitUpgrade => unitUpgrade.CostSM);
        BasicBuildTime += Upgrades.Sum(unitUpgrade => unitUpgrade.BasicBuildTime);
        DeployRadius += Upgrades.Sum(unitUpgrade => unitUpgrade.DeployRadius);
        SpeedGlobal += Upgrades.Sum(unitUpgrade => unitUpgrade.SpeedGlobal);
        Speed += Upgrades.Sum(unitUpgrade => unitUpgrade.Speed);
        ReverseFactor += Upgrades.Sum(unitUpgrade => unitUpgrade.ReverseFactor);
        TurnSpeed += Upgrades.Sum(unitUpgrade => unitUpgrade.TurnSpeed);
        Acceleration += Upgrades.Sum(unitUpgrade => unitUpgrade.Acceleration);
        Mass += Upgrades.Sum(unitUpgrade => unitUpgrade.Mass);
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
