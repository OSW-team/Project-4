using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GameModelsAndEnums : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public class ModuleMultyply
    {
        public EnumModuleType TargetModule;
        public string ParamName;
        public float Percent;
        public ModuleMultyply(EnumModuleType targetModule, string paramName, float percent)
        {
            TargetModule = targetModule;
            ParamName = paramName;
            Percent = percent;
        }
    }

    public class ModuleToModuleDrain
    {
        public EnumModuleType FromModule;
        public string FromParamName;
        public float FromPercent;
        public EnumModuleType ToModule;
        public string ToParamName;
        public float ToPercent;
        public ModuleToModuleDrain(EnumModuleType fromModule, string fromParamName, float fromPercent, EnumModuleType toModule, string toParamName, float toPercent)
        {
            FromModule = fromModule;
            FromParamName = fromParamName;
            FromPercent = fromPercent;
            ToModule = toModule;
            ToParamName = toParamName;
            ToPercent = toPercent;
        }
    }

    public enum EnumUnitClass
    {
        Infantry,
        LightMateriel,
        HeavyMateriel,
        MissileLauncher,
        Artillery,
        LightAir,
        HeavyAir,
        Building,
        SF
    }

    public enum EnumModuleType
    {
        Platform,
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

    public static EnumModuleType GetModuleTypeByString(string s)
    {
        var res = EnumModuleType.Hypo;
        switch (s)
        {
            case "Platform":
                res = EnumModuleType.Platform;
                break;
            case "Engine":
                res = EnumModuleType.Engine;
                break;
            case "Bridge":
                res = EnumModuleType.Bridge;
                break;
            case "Constructor":
                res = EnumModuleType.Constructor;
                break;
            case "Storage":
                res = EnumModuleType.Storage;
                break;
            case "Weapon":
                res = EnumModuleType.Weapon;
                break;
            case "Hypo":
                res = EnumModuleType.Hypo;
                break;

        }
        return res;
    }

    public static Dictionary<float, float> GetMalfunctionTreshold(string s)
    {
        var malF = new Dictionary<float, float>();
        return malF;
    }

    public static List<EnumRace> GetPurchaseRestrictionsByString(string s)
    {
        var l = new List<EnumRace>();
        return l;
    }

    public static ModuleMultyply GetMultyply(string s)
    {
        var module = EnumModuleType.Constructor;
        var param = "";
        float percent = 1;
        var multyply = new ModuleMultyply(module, param, percent);
        return multyply;
    }

    public static ModuleToModuleDrain GetDrain(string s)
    {
        var fromModule = EnumModuleType.Constructor;
        var fromParam = "";
        float fromPercent = 1;
        var toModule = EnumModuleType.Constructor;
        var toParam = "";
        float toPercent = 1;
        var drain = new ModuleToModuleDrain(fromModule, fromParam, fromPercent, toModule, toParam, toPercent);
        return drain;
    }

    public static List<string> GetCompatibility(string innerText)
    {
        var list = new List<string>();

        return list;
    }

    public static List<float> GetUnitRecovery(string innerText)
    {
        return new List<float>();
    }

    public static List<EnumUnitClass> GetUnitClasses(string innerText)
    {
       return new List<EnumUnitClass>();
    }

    public static Dictionary<EnumUnitClass, int> GetUnitPriorities(string innerText)
    {
        return new Dictionary<EnumUnitClass, int>();
    }

    public static string GetUpgradeRequirements(string innerText)
    {
        var resStr = "";
        return resStr;
    }

    public static string CheckBoost(string text, ref bool isBooster, ref string boosterName, ref string originalvalue)
    {
        var res = text;
        if(text.Contains("Boost"))
        {
            var index = text.IndexOf(";");
            res = text.Remove(0, index + 1).Replace("]", "");
            isBooster = true;
            boosterName = text.Remove(index, text.Length-index).Replace("[", "");
            if (text.Contains("_")){

                var originalIndex = text.IndexOf("_");
                boosterName = text.Remove(0, originalIndex + 1);
                originalvalue = text.Remove(originalIndex, text.Length - originalIndex);
            }

        }
        return res;
    }



}
