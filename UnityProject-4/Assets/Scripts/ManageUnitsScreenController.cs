using UnityEngine;
using System.Collections;

public class ManageUnitsScreenController : ManagementScreen {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ShowUnit(Unit unit)
    {
        unit.BuildMesh();
        var Center = new Vector3(-1.55f, -10.17f, 31.7f);
        CurrentManagingGameObject = unit.GO;
        CurrentManagingGameObject.transform.position = Center;
        CurrentManagingGameObject.transform.eulerAngles = new Vector3(0, 150, 0);

    }
}
