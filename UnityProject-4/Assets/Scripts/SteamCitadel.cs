using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SteamCitadel : MonoBehaviour
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
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
