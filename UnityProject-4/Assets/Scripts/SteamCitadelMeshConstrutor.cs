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

    private static void AddSingleModule(SteamCitadel Citadel, Module module)
    {
        var modelPath = "Models/" + module.ModelName;
        module.GO = Instantiate(Resources.Load<GameObject>(modelPath));
        module.GO.name = "Module " + module.Workmame;
        module.GO.transform.SetParent(Citadel.GO.transform);
    }

    public static void PlaceModules(SteamCitadel citadel)
    {
        var platform = citadel.Modules.FirstOrDefault(x => x.Type == GameModelsAndEnums.EnumModuleType.Platform).GO;
        foreach (var module in citadel.Modules)
        {
            var slot = module.Slot;
            module.GO.transform.position = platform.transform.FindChild("Module_Sites").FindChild(slot).position;
        }

    }
}
