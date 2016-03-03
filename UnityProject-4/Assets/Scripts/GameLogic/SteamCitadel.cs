using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SteamCitadel
{
    public int CurrrentTotalAmontOfModules;
    public int MaxTotalAmontOfModules;
    public float CurrentTotalStorage;
    public float MaxTotalStorage;
    public float TotalRadarRange;
    public float TotalRadarAcc;
    public float TotalMass;
    public float TotalForce;
    public float TotalConsumption;
    public float GloabalTotalSpeed;
    public float TacticalTotalSpeedFactor;
    public float TotalAcceleration;
    public float TotalTurnSpeed;
    public int MaxTotalControl;
    public int AvailableTotalControl;
    public int TotalSMAmount;
    public int MaxTotalSchemeAmount;
    public int CurrentTotalSchemeAmount;
    public GameModelsAndEnums.EnumCitadelRole Role;
    public float TotalRecoveryMetal;
    public float TotalRecoveryParts;
    public float TotalRecoveryEnergy;
    public float MaxTotalShield;
    public float CurrentTotalShield;
    public float PhysTotalConShieldArm;
    public float EnergyTotalConShieldArm;
    public float HeatTotalConShieldArm;
    public float TotalShieldPower;
    public float OnlineFinalTotalShieldRegen;
    public float OfflineFinalTotalShieldRegen;
    public float TotalShieldTreshHold;
    public float TotalConstructionSpeed;
    public float MaxStorageParts;
    public int CurrentStorageParts;
    public float TotalConversionSpeed;
    public float TotalConversionEff;
    public float TotalPotential;

    public List<Module> Modules;
    public string Name;
    public GameObject GO;
    public List<Unit> Units;
    public Inventory Inventory;
    // Use this for initialization
    public SteamCitadel(string name)
    {
        Name = name;
        Modules = new List<Module>();
        Units = new List<Unit>();
        Inventory = new Inventory();
       // XMLWorker.SaveSCItem(name);
    }

    public void BuildCitadelMesh()
    {
        SteamCitadelMeshConstrutor.BuildCitadelMesh(this);
    }

    public void RecountProps()
    {
        CurrentTotalStorage += Modules.Sum(module => module.CurrentStorageParts);
        MaxTotalStorage += Modules.Sum(module => module.MaxStorage);
        TotalRadarRange += Modules.Sum(module => module.RadarRange);
        TotalRadarAcc += Modules.Sum(module => module.RadarAcc);
        TotalMass += Modules.Sum(module => module.Mass);
        TotalForce += Modules.Sum(module => module.Force);
        TotalConsumption += Modules.Sum(module => module.Consumption);
        MaxTotalControl += Modules.Sum(module => module.MaxControl);
        MaxTotalSchemeAmount += Modules.Sum(module => module.MaxSchemeAmount);
        MaxTotalShield += Modules.Sum(module => module.MaxShield);
        TotalShieldPower += Modules.Sum(module => module.ShieldPower);
        TotalPotential += Modules.Sum(module => module.Potential);
        TotalConversionSpeed += Modules.Sum(module => module.ConversionSpeed);
        TotalConversionEff += Modules.Sum(module => module.ConversionEff);
        //...

    }

}
