using UnityEngine;
using System.Collections;
using System.Reflection;

public class UnitBooster : MonoBehaviour {

    public string TargetFieldName;
    public int IntBoostValue; 
    public float FloatBoostValue;
    public string BoosterName;
    public bool IsActive;
    public Unit Unit;
    private FieldInfo publicField;
    private float originValue;

    public delegate void ActivateDelgate();
    public event ActivateDelgate activate;

    public UnitBooster(string targetFieldName, float boostValue, string boosterName, Unit unit)
    {
        TargetFieldName = targetFieldName;
        FloatBoostValue = boostValue;
        BoosterName = boosterName;
        Unit = unit;
        publicField = typeof(Unit).GetField(TargetFieldName, BindingFlags.Public | BindingFlags.Instance);
        originValue = (float)publicField.GetValue(Unit);
    }

    public UnitBooster(ref int intValue, int boostValue, string boosterName)
    {
        IntBoostValue = boostValue;
        BoosterName = boosterName;
        
    }
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Activate()
    {
        publicField.SetValue(Unit, originValue + FloatBoostValue);
    }

    public void Deactivate()
    {
        publicField.SetValue(Unit, originValue);
    }
}
