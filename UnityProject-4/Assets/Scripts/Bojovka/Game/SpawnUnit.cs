using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SpawnUnit : MonoBehaviour {
    public Unit[] Units;
    public Transform SpawnPoint;
    public MasterMindNHWheels Master;
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
	
	}
	
	// Update is called once per frame
	void Update () {
        SpawnOnClick();
	}

    void SpawnOnClick()
    {

        Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit _hit;
        if (Input.GetMouseButtonDown(0) && Physics.Raycast(_ray, out _hit, Mathf.Infinity, layerMask) && (_hit.point - SpawnArea.position).sqrMagnitude < SpawnArea.localScale.x * SpawnArea.localScale.x / 4)
        {
			Units [switchUnit].BuildMesh ();
			GameObject _unit = Units [switchUnit].GO;
			_unit.transform.position = SpawnPoint.transform.position;
			_unit.transform.rotation = SpawnPoint.transform.rotation;
            _unit.GetComponent<UnitStats>().team = Team;
            Master.AddAgent(_unit, PointDown.position);
            _unit.GetComponent<StateMachine>().SetupStateSet(PointDown.position, _hit.point, PointMarch.position, EnemySF);
        }
    }

	public void UnitSwitch(int unitIndex){
		switchUnit = unitIndex;
	}
}
