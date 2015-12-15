using UnityEngine;
using System.Collections;

public class BuildSteamCitadelController : MonoBehaviour
{
    public GameObject Platform;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	    if (Input.GetKeyDown(KeyCode.B))
	    {
	        var modelPath = XMLWorker.LoadItem(XMLWorker.EnumGameItemType.Modules, "Platform", "ModelName");
            var platform = Instantiate(Resources.Load<GameObject>(modelPath)) as GameObject;
	        Platform = platform;

	        Platform.transform.rotation = platform.transform.rotation;
	        Camera cam = FindObjectOfType<Camera>();
	        var Center = cam.ScreenToWorldPoint(Vector3.zero);
            Platform.transform.position = Center;

        }
	}

    public void AddModule()
    {
        
    }
}
