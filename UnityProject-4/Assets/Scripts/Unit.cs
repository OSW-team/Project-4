using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
    public List<string> UpgradeRequirements;
    public float SchemeCostMetal;
    public float SchemeCostEnergy;

    public List<UnitUpgrade> Upgrades;


    public Unit(string CSWokname, string workName)
    {
        Upgrades = new List<UnitUpgrade>();
        XMLWorker.LoadUnit(workName, this);
    }

    protected Unit()
    {
        
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
