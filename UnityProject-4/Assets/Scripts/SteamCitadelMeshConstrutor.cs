using UnityEngine;
using System.Collections;
using System.Linq;

public  class SteamCitadelMeshConstrutor:MonoBehaviour {
	// Use this for initialization
    private static GameObject _citadel;
    public static void BuildCitadelMesh(SteamCitadel Citadel)
    {
        AddModules(Citadel);
        PlaceModules(Citadel);
        AddSubs(Citadel);
        PlaceSubs(Citadel);
        //var modelPath = "";
        //var platform = Instantiate(Resources.Load<GameObject>(modelPath)) as GameObject;
        //Platform = platform;

        //Platform.transform.rotation = platform.transform.rotation;
        //Camera cam = FindObjectOfType<Camera>();
        //var Center = cam.ScreenToWorldPoint(Vector3.zero);
        //Platform.transform.position = Center;
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
        var modelPath = "Models/" + module.ModelName;
        module.GO = Instantiate(Resources.Load<GameObject>(modelPath));
        module.GO.name = "Module " + module.Workname;
        module.GO.transform.SetParent(Citadel.GO.transform);
    }

    public static void PlaceModules(SteamCitadel citadel)
    {
        var platform = citadel.Modules.FirstOrDefault(x => x.Type == GameModelsAndEnums.EnumModuleType.Platform).GO;
        foreach (var module in citadel.Modules)
        {
            var slot = module.Slot;
            module.GO.transform.position = platform.transform.FindChild("Module_Sites").FindChild(slot).position;
            module.GO.transform.rotation = platform.transform.FindChild("Module_Sites").FindChild(slot).rotation;
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
        var modelPath = "Models/" + subsystem.ModelName;
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
