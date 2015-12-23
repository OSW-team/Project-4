using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class MangeSteamCitadelScreenController : MonoBehaviour
{
    public Camera Camera;
    public GameObject SteamCitadelManagementScreen;
    public TestController Controller;
    public GameObject DraggableField;
    public GameObject AlowedSubsystems;
    public Vector3 CurrentPosition;
    public GameObject SteamCitadel;
    public float RotationSpeed;
    private bool _startDrag;
    public List<GameObject> ModuleButtons;
    public Material GhostMat;
    // Use this for initialization
    void Start ()
    {
        Controller = FindObjectOfType<TestController>();

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Show()
    {
        SteamCitadelManagementScreen.SetActive(true);

    }

    public void OnDraggableFieldDown()
    {
        if (!_startDrag)
        {
            _startDrag = true;
           // var curPos = Input.mousePosition;
            CurrentPosition = Input.mousePosition; 

        } 
    }

    public void OnDraggableFieldUp()
    {
        _startDrag = false;
        //var curPos = Input.mousePosition;
        CurrentPosition = Input.mousePosition;

    }


    public void OnDrag()
    {
        var offset = Input.mousePosition.x - CurrentPosition.x;
        var rotY= -1*offset*RotationSpeed;

        SteamCitadel.transform.localEulerAngles = new Vector3(SteamCitadel.transform.localEulerAngles.x, SteamCitadel.transform.localEulerAngles.y+rotY, SteamCitadel.transform.localEulerAngles.z);
    }

    public enum EnumModuleScreenState
    {
        ModuleSelected,
        SubsystemSelected
    }

    public void ShowCitadel(SteamCitadel citadel)
    {
        var citadelGO = citadel.GO;
        Camera cam = FindObjectOfType<Camera>();
        var Center = new Vector3(0.056f,0.838f,-1.792f);
        citadelGO.transform.position = Center;
        SteamCitadel = citadelGO;
        SteamCitadel.transform.eulerAngles = new Vector3(0,150,0);
        var sprites = Resources.LoadAll<Sprite>("Icons");
        foreach (var module in citadel.Modules)
        {
            var moduleButton = ModuleButtons[0];
            switch (module.Type)
            {
                    case GameModelsAndEnums.EnumModuleType.Engine:
                    moduleButton = ModuleButtons[0];
                    break;
                case GameModelsAndEnums.EnumModuleType.Bridge:
                    moduleButton = ModuleButtons[1];
                    break;
                case GameModelsAndEnums.EnumModuleType.Storage:
                    moduleButton = ModuleButtons[2];
                    break;
                case GameModelsAndEnums.EnumModuleType.Hypo:
                    moduleButton = ModuleButtons[3];
                    break;
                case GameModelsAndEnums.EnumModuleType.Constructor:
                    moduleButton = ModuleButtons[4];
                    break;
                case GameModelsAndEnums.EnumModuleType.Platform:
                    moduleButton = ModuleButtons[5];
                    break;
                case GameModelsAndEnums.EnumModuleType.Weapon:
                    moduleButton = ModuleButtons[6];
                    break;
            }
            moduleButton.SetActive(true);
            var module1 = module;
            moduleButton.GetComponent<Button>().onClick.AddListener(()=> { var meshes = module1.GO.GetComponentsInChildren<MeshRenderer>();foreach (var meshRenderer in meshes){meshRenderer.material = GhostMat;} });
            moduleButton.transform.FindChild("ModuleImage").FindChild("ModuleIcon").GetComponent<Image>().sprite =sprites.FirstOrDefault(x => x.name == module.IconName);
            for (var i = 0; i < module.MaxSlotAmount; i++)
            {
                if (i < module.Subs.Count)
                {

                    moduleButton.transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite =
                        sprites.FirstOrDefault(x => x.name == module.Subs[i].IconName);                  
                }
                else
                {
                    moduleButton.transform.GetChild(i).GetComponent<Button>().onClick.RemoveAllListeners();
                    var i1 = i;
                    module1 = module;
                    moduleButton.transform.GetChild(i).GetComponent<Button>().onClick.AddListener(() =>
                    {
                        ShowAlowedSubsystems(module1, module1.GO.transform.FindChild("Subsystem_Slots").GetChild(i1).name);
                    });
                }
            }
            Debug.Log("Module - "+module.GO.name+"; Button - "+moduleButton);
        }

    }

    private void ShowAlowedSubsystems(Module module, string subSlotName)
    {
        Debug.Log(subSlotName);
        var sprites = Resources.LoadAll<Sprite>("Icons");
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
}
