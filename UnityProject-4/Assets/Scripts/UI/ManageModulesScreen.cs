using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class ManageModulesScreen : ManagementScreen
{
    public GameObject SteamCitadelManagementScreen;
    public GameObject AlowedSubsystems;
    public Material GhostMat;
    private List<GameObject> _moduleButtons;  
    private List<GameObject> _subButtons;  
    // Use this for initialization

    public void ShowCitadel(SteamCitadel citadel)
    {
        ManagementScreenGameObject.SetActive(true);
        SteamCitadelMeshConstrutor.BuildCitadelMesh(citadel);
        var citadelGO = citadel.GO;
        Camera cam = FindObjectOfType<Camera>();
        var Center = new Vector3(0.056f,0.838f,-1.792f);
        CurrentManagingGameObject = citadelGO;
        CurrentManagingGameObject.transform.position = Center;
        CurrentManagingGameObject.transform.eulerAngles = new Vector3(0,150,0);
        _moduleButtons = new List<GameObject>();
        _subButtons = new List<GameObject>();
        AddModuleButtonByMesh();
        foreach (var module in citadel.Modules)
        {
            FillButtonsWithModules(module);
            //FillButtonsWithModules(module);
            //var moduleButton = ModuleButtons[0];
            //switch (module.Type)
            //{
            //        case GameModelsAndEnums.EnumModuleType.Engine:
            //        moduleButton = ModuleButtons[0];
            //        break;
            //    case GameModelsAndEnums.EnumModuleType.Bridge:
            //        moduleButton = ModuleButtons[1];
            //        break;
            //    case GameModelsAndEnums.EnumModuleType.Storage:
            //        moduleButton = ModuleButtons[2];
            //        break;
            //    case GameModelsAndEnums.EnumModuleType.Hypo:
            //        moduleButton = ModuleButtons[3];
            //        break;
            //    case GameModelsAndEnums.EnumModuleType.Constructor:
            //        moduleButton = ModuleButtons[4];
            //        break;
            //    case GameModelsAndEnums.EnumModuleType.Platform:
            //        moduleButton = ModuleButtons[5];
            //        break;
            //    case GameModelsAndEnums.EnumModuleType.Weapon:
            //        moduleButton = ModuleButtons[6];
            //        break;
            //}

            //Debug.Log("Module - "+module.GO.name+"; Button - "+moduleButton);
        }

    }

    private void AddModuleButtonByMesh()
    {
        var controlsTransform = gameObject.transform.FindChild("Controls");
        for (int i = 0; i < controlsTransform.childCount; i++)
        {
            Destroy(controlsTransform.GetChild(i).gameObject);
        }
        var currentMeshTransform = CurrentManagingGameObject.transform;
        var platformButton = Instantiate(Resources.Load<GameObject>("Prefabs/UI/ModuleButton"));
        platformButton.name = "Platform";
        platformButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            Controller.MyCitadel.Modules.Add(new Module(Controller.MyCitadel.Name, "Weapon1",
                platformButton.name));
            ShowCitadel(Controller.MyCitadel);
            XMLWorker.SaveSC(Controller.CurrentCitadel);
        });
        platformButton.transform.SetParent(controlsTransform);
        _moduleButtons.Add(platformButton);
        for (var i = 0; i < CurrentManagingGameObject.transform.childCount; i++)
        {
            var childModulesTransform = currentMeshTransform.GetChild(i).FindChild("Module_Sites");
            if (childModulesTransform)
            {
                for (var j = 0; j < childModulesTransform.childCount; j++)
                {
                    var childModuleButton = Instantiate(Resources.Load<GameObject>("Prefabs/UI/ModuleButton"));
                    childModuleButton.name = currentMeshTransform.GetChild(i).name.Replace("Module ","") + "_"+childModulesTransform.GetChild(j).name;
                    childModuleButton.GetComponent<Button>().onClick.AddListener(()=>
                    {
                        Controller.MyCitadel.Modules.Add(new Module(Controller.MyCitadel.Name, "Weapon1",
                            childModuleButton.name));
                        ShowCitadel(Controller.MyCitadel);
                        XMLWorker.SaveSC(Controller.CurrentCitadel);
                    });
                    childModuleButton.transform.SetParent(controlsTransform);
                    _moduleButtons.Add(childModuleButton);
                    
                }

            }
        }
        
        

    }

    private void AddSubsystemButtonByMesh(Module module, Transform moduleButton)
    {
        var sprites = Resources.LoadAll<Sprite>("UI/Management screen UI/1. Modules' screen/Icons");
        var moduleTransform = module.GO.transform;
            var childSubsTransform = moduleTransform.FindChild("Subsystem_Slots");
            if (childSubsTransform)
            {
                for (var j = 0; j < childSubsTransform.childCount; j++)
                {
                    var subButton = Instantiate(Resources.Load<GameObject>("Prefabs/UI/SubButton"));
                    subButton.name = childSubsTransform.GetChild(j).name;
                    foreach (var sub in module.Subs)
                    {
                    subButton.GetComponent<Button>().onClick.AddListener(() =>
                    {
                        ShowAlowedSubsystems(module, subButton.name);
                    });
                    if (sub.Slot == subButton.name)
                        {
                        subButton.transform.FindChild("ModuleImage").GetChild(0).GetComponent<Image>().sprite =
                           sprites.FirstOrDefault(x => x.name == sub.IconName);
                        subButton.GetComponent<Button>().onClick.RemoveAllListeners();
                        subButton.GetComponent<Button>().onClick.AddListener(() =>
                        {
                            
                        });
                    }
                    }
                   
                    subButton.transform.SetParent(moduleButton.transform);
                    _subButtons.Add(subButton);
            }
        }
    }

    private void FillButtonsWithModules(Module module)
    {
        var sprites = Resources.LoadAll<Sprite>("UI/Management screen UI/1. Modules' screen/Icons");
        GameObject moduleButton = module.GO;
        if (module.Type == GameModelsAndEnums.EnumModuleType.Platform)
        {
            moduleButton = _moduleButtons.FirstOrDefault(x => x.name == "Platform");
        }
        else
        {
            foreach (var button in _moduleButtons)
            {
                if (module.Slot == button.name)
                {
                    moduleButton = button;
                    AddSubsystemButtonByMesh(module, moduleButton.transform);
                }
                
            }
        }
            
        var module1 = module;
        Debug.Log("ModuleButton "+moduleButton.name);
        moduleButton.GetComponent<Button>().onClick.RemoveAllListeners();
        moduleButton.GetComponent<Button>().onClick.AddListener(() => {
            var meshes = module1.GO.GetComponentsInChildren<MeshRenderer>();foreach (var meshRenderer in meshes){meshRenderer.material = GhostMat;} });
        moduleButton.transform.FindChild("ModuleImage").FindChild("ModuleIcon").GetComponent<Image>().sprite = sprites.FirstOrDefault(x => x.name == module.IconName);
        foreach (var sub in module1.Subs)
        {
            foreach (var o in _subButtons)
            {

                if (module1.Workname + sub.Slot == o.name)
                {
                    o.GetComponent<Button>().onClick.AddListener(()=>Debug.Log("Sub"+o.name));
                }
            }
        }
        //for (var i = 0; i < module.MaxSlotAmount; i++)
        //{
        //    if (i < module.Subs.Count)
        //    {

        //        moduleButton.transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite =
        //            sprites.FirstOrDefault(x => x.name == module.Subs[i].IconName);
        //    }
        //    else
        //    {
        //        moduleButton.transform.GetChild(i).GetComponent<Button>().onClick.RemoveAllListeners();
        //        var i1 = i;
        //        module1 = module;
        //        moduleButton.transform.GetChild(i).GetComponent<Button>().onClick.AddListener(() =>
        //        {
        //            ShowAlowedSubsystems(module1, module1.GO.transform.FindChild("Subsystem_Slots").GetChild(i1).name);
        //        });
        //    }
        //}
    }

    private void ShowAlowedSubsystems(Module module, string subSlotName)
    {
        Debug.Log(subSlotName);
        var sprites = Resources.LoadAll<Sprite>("UI/Management screen UI/1. Modules' screen/Icons");
        for (var i = 0; i<Controller.Player.Subsystems.Count; i++)
        {
            var sub = AlowedSubsystems.transform.GetChild(i);
            sub.GetChild(0).GetComponent<Image>().sprite = sprites.FirstOrDefault(x => x.name == Controller.Player.Subsystems[i].IconName);
            sub.gameObject.SetActive(true);
            sub.GetComponent<Button>().onClick.RemoveAllListeners();
            var i1 = i;
            var module1 = module;
            sub.GetComponent<Button>().onClick.AddListener(()=> { TrySubsystem(module1, Controller.Player.Subsystems[i1].Workname, subSlotName); });

        }
    }

    private void TrySubsystem(Module module, string subName, string subSlotName)
    {
        var rotation = new Vector3(Controller.Player.Citadel.GO.transform.localEulerAngles.x, Controller.Player.Citadel.GO.transform.localEulerAngles.y, Controller.Player.Citadel.GO.transform.localEulerAngles.z);
        Destroy(Controller.Player.Citadel.GO);
        Controller.Player.Citadel.GO  = new GameObject("Citadel " + name);
        Debug.Log(module.GO.name);
        var newSub = new Subsystem(Controller.Player.Citadel.Name, module.Workname, subName, subSlotName);
        module.Subs.Add(newSub);
        
        SteamCitadelMeshConstrutor.BuildCitadelMesh(Controller.Player.Citadel);
        newSub.GO.GetComponent<MeshRenderer>().material = GhostMat;
        ShowCitadel(Controller.Player.Citadel);
        Controller.Player.Citadel.GO.transform.localEulerAngles = rotation;
    }

    void OnDisable()
    {
        Destroy(CurrentManagingGameObject);
    }

    public enum EnumModuleScreenState
    {
        ModuleSelected,
        SubsystemSelected
    }
}
