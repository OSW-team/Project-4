using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SteamCitadel
{
    public int CurrrentTotalAmontOfModules;
    public int MaxTotalAmontOfModules;
    public float CurrntTotalStorage;
    public float MaxTotalStorage;
    public int TotalRadarRange;
    public int TotalRadarAcc;
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
    public int MaxStorageParts;
    public int CurrentStorageParts;
    public float TotalConversionSpeed;
    public float TotalConversionEff;
    public float TotalPotential;

    public List<Module> Modules;
    public string Name;
    public GameObject GO;
    public List<Unit> Units; 
    // Use this for initialization
    public SteamCitadel(string name)
    {
        Name = name;
        Modules = new List<Module>();
        Units = new List<Unit>();
       // XMLWorker.SaveSC(name);
    }


}
