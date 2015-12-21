using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Module : MonoBehaviour
{
    public string Workmame;
    public string GameName;
    public string Description;
    public string ModelName;
    public string IconName;
    public float Mass;
    public GameModelsAndEnums.EnumModuleType Type;
    public int MaxSlotAmount;
    public int CurrntSlotAmount;
    public float MaxHP;
    public float CurrentHP;
    public float Exhaust;
    public Dictionary<float, float> MalfunctionThreshold;
    public float ExhaustSpeed;
    public float PhysycReflectArmor;
    public float EnergyReflectArmor;
    public float HeatReflectArmor;
    public float PhysycConsumArmor;
    public float EnergyConsumArmor;
    public float HeatConsumArmor;
    public float Vulnerability;
    public float MetalCost;
    public float EnergyCost;
    public List<GameModelsAndEnums.EnumRace> PurchaseRestrictions;

    public float RadarRange;
    public float RadarAcc;
    public float MaxStorage;
    public float Force;
    public float Consumption;
    public int MaxControl;
    public float MaxShield;
    public float PhysConShieldArm;
    public float EnergyConShieldArm;
    public float HeatConShieldArm;
    public float OnlineBaseShieldRegen;
    public float OfflineBaseShieldRegen;
    public float ShieldPower;
    public float ShieldTreshold;
    public int MaxSchemeAmount;
    public float ConstructionSpeed;
    public int MaxStorageParts;
    public int CurrentStorageParts;
    public float ConversionSpeed;
    public float ConversionEff;
    public float ConversionReverseEff;
    public float Potential;
    public GameModelsAndEnums.ModuleMultyply Multyply;
    public GameModelsAndEnums.ModuleToModuleDrain Drain;

    public List<Subsystem> Subs;
    public GameObject GO;
    public List<bool> SlotsBools;
    public string Slot;
    // Use this for initialization
    public Module(string SCWorkname, string workname, string slot)
    {

        Slot = slot;
       // XMLWorker.SaveModule(SCWorkname, workname, slot);
        XMLWorker.LoadModule(workname, this);
        Subs = new List<Subsystem>(MaxSlotAmount);
        SlotsBools = new List<bool>();
        for (var i = 0; i < MaxSlotAmount; i++)
        {
            SlotsBools.Add(false);
        }
    }

    protected Module()
    {
        
    }
}
