using UnityEngine;
using System.Collections;

public class InventoryItem : MonoBehaviour {
    public string Name;
    public string Description;
    public int ItemAmount;
    public float Mass;
    public int Volume;
    public float MetalCost;
    public float EnergyCost;
    public float SupplyCurrent;
    public float SupplyMax;
    public bool Toggle;
    public GameModelsAndEnums.EnumItemFunction Function;
    public string Workname;
    public int StacksTill;
    public float EcoAmount;
    public float MilAmount;
    public float SciAmount;
    public float ElMagComponent;
    public float HydraulicsComponent;
    public float ReinforcedComponent;
    public float VoidComponent;
    public float ChronoComponent;
    public bool IsBreakable;
    public float BreakPointsCurrent;
    public float BreakPointsMax;
    public float BreakDegree;
    public bool IsContaminatable;
    public float ContaminationPointsCurrent;
    public float ContaminationPointsMax;
    public float ContaminationDegree;
    public string IconName;

    public InventoryItem(string workname)
    {
        XMLWorker.LoadInventoryItem(workname, this);
    }
}
