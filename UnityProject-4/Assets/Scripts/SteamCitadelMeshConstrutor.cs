using UnityEngine;
using System.Collections;
using System.Linq;

public  class SteamCitadelMeshConstrutor:MonoBehaviour {
	// Use this for initialization
    private static GameObject _citadel;
    public static void BuildCitadelMesh(SteamCitadel Citadel)
    {
        if (Citadel.GO != null)
        {
            Destroy(Citadel.GO);
        }
        Citadel.GO = new GameObject("Citadel " + Citadel.Name);
        AddModules(Citadel);
        PlaceModules(Citadel);
        AddSubs(Citadel);
        PlaceSubs(Citadel);
    }

    private static void AddModules(SteamCitadel myCitadel)
    {
        foreach (var module in myCitadel.Modules)
        {
            AddSingleModule(myCitadel,module);
        }
    }

    public static void AddSingleModule(SteamCitadel Citadel, Module module)
    {
        var modelPath = "Models/SCModules/"+ Citadel.Name+"/" + module.ModelName;
        module.GO = Instantiate(Resources.Load<GameObject>(modelPath));
        module.GO.name = "Module " + module.Workname;
        module.GO.transform.SetParent(Citadel.GO.transform);
    }

    public static void PlaceModules(SteamCitadel citadel)
    {
        var platform = citadel.Modules.FirstOrDefault(x => x.Type == GameModelsAndEnums.EnumModuleType.Platform).GO;
        foreach (var module in citadel.Modules)
        {
            if (module.Type != GameModelsAndEnums.EnumModuleType.Platform)
            {
                var slot = module.Slot;
                GameObject parentModuleGO = platform;
                foreach (var module1 in citadel.Modules)
                {
                    if (slot.Contains(module1.Workname))
                    {
                        parentModuleGO = module1.GO;
                        slot = slot.Replace(module1.Workname, "").Remove(0, 1);
                    }
                }
                module.GO.transform.position = parentModuleGO.transform.FindChild("Module_Sites").FindChild(slot).position;
                module.GO.transform.rotation = parentModuleGO.transform.FindChild("Module_Sites").FindChild(slot).rotation;
            }
           
        }

    }

    private static void AddSubs(SteamCitadel citadel)
    {
        foreach (var module in citadel.Modules)
        {
            foreach (var subsystem in module.Subs)
            {
                AddSingleSubsystem(citadel, module, subsystem);
            }
        }
    }

    public static void AddSingleSubsystem(SteamCitadel citadel, Module module, Subsystem subsystem)
    {
        var modelPath = "Models/SCSubs/" + citadel.Name + "/" + subsystem.ModelName;
        Debug.Log(modelPath);
        subsystem.GO = Instantiate(Resources.Load<GameObject>(modelPath));
        subsystem.GO.name = "Subsystem " + subsystem.Workname;
        subsystem.GO.transform.SetParent(module.GO.transform);
    }

    public static void PlaceSubs(SteamCitadel citadel)
    {
        foreach (var module in citadel.Modules)
        {
            foreach (var subsystem in module.Subs)
            {
                var slot = subsystem.Slot;
                subsystem.GO.transform.position = module.GO.transform.FindChild("Subsystem_Slots").FindChild(slot).position;
                subsystem.GO.transform.rotation = module.GO.transform.FindChild("Subsystem_Slots").FindChild(slot).rotation;
            }
        }
    }
}
