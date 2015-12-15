using UnityEngine;
using System.Collections;

public class GameModelsAndEnums : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public class ModuleMultyply
    {
        public ModuleMultyply(EnumModuleType targetModule, string paramName, float percent)
        {
            
        }
    }

    public class ModuleToModuleDrain
    {
        public ModuleToModuleDrain(EnumModuleType fromModule, string fromParamName, float fromPercent, EnumModuleType toModule, string toParamName, float toPercent)
        {

        }
    }

    public enum EnumModuleType
    {
        Constructor,
        Engine,
        Storage,
        Bridge,
        Weapon,
        Hypo
    }

    public enum EnumCitadelRole
    {
        SMControl,
        PilotControl,
        SelfControl
    }

    public enum EnumRace
    {
        
    }
}
