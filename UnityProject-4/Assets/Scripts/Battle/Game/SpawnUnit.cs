using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class SpawnUnit : MonoBehaviour {
    public Unit[] Units;
    public List<GameObject> SpawnedUnitsGameObjects;
	Camera cam;
    public Transform SpawnPoint;
    public MasterMindTranslate Master;
    public Transform PointDown, PointDeploy, PointMarch;
    public GameObject EnemySF;
	int switchUnit;

    public Transform SpawnArea;

    public int Team;

    int layerMask = ~(1 << 8);
    // Use this for initialization



    void Start () {
		var a = GameObject.Find ("ButtonSnake").GetComponent<Button> ();
		a.onClick.AddListener (delegate () {this.UnitSwitch(0);} );
		var b = GameObject.Find ("ButtonTank").GetComponent<Button> ();
		b.onClick.AddListener (delegate () {this.UnitSwitch(1);} );

		var c = GameObject.Find ("ButtonMissile").GetComponent<Button> ();
		c.onClick.AddListener (delegate () {this.UnitSwitch(2);} );

		cam = GameObject.Find ("Camera").GetComponent<Camera> ();
        SpawnedUnitsGameObjects = new List<GameObject>();
        

    }
	
	// Update is called once per frame
	void Update () {
        SpawnOnClick();
	}

    void SpawnOnClick()
    {

        Ray _ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit _hit;
		Debug.DrawRay (cam.transform.position, _ray.direction * 1000);
		if (Input.GetMouseButtonDown(0) && Physics.Raycast(_ray, out _hit, Mathf.Infinity, layerMask) && (_hit.point - SpawnArea.position).sqrMagnitude < SpawnArea.localScale.x * SpawnArea.localScale.x / 4)
        {
			Debug.Log (" Here we go! ");
			Units [switchUnit].BuildMesh ();
			GameObject _unit = Units [switchUnit].GO;
			_unit.transform.position = SpawnPoint.transform.position;
			_unit.transform.rotation = SpawnPoint.transform.rotation;
            var unitStats = _unit.GetComponent<UnitStats>();
            unitStats.team = Team;
            Units[switchUnit].RecountProps();
            unitStats.maxHP = Units[switchUnit].HpMax;
            unitStats.weight = Units[switchUnit].Mass;
            Master.AddAgent(_unit, PointDown.position);
            _unit.GetComponent<StateMachine>().SetupStateSet(PointDown.position, _hit.point, PointMarch.position, EnemySF);
            SpawnedUnitsGameObjects.Add(_unit);
        }
    }

	public void UnitSwitch(int unitIndex){
		switchUnit = unitIndex;
	}
}
