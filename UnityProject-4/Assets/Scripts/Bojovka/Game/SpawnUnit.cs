using UnityEngine;
using System.Collections;

public class SpawnUnit : MonoBehaviour {
    public Unit[] Units;
    public Transform SpawnPoint;
    public MasterMindNHWheels Master;
    public Transform PointDown, PointDeploy, PointMarch;
    public GameObject EnemySF;

    public Transform SpawnArea;

    public int Team;

    int layerMask = ~(1 << 8);
    // Use this for initialization



    void Start () {
	
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
			Units [0].BuildMesh ();
			GameObject _unit = Units [0].GO;
			_unit.transform.position = SpawnPoint.transform.position;
			_unit.transform.rotation = SpawnPoint.transform.rotation;
            _unit.GetComponent<UnitStats>().team = Team;
            Master.AddAgent(_unit, PointDown.position);
            _unit.GetComponent<StateMachine>().SetupStateSet(PointDown.position, _hit.point, PointMarch.position, EnemySF);
        }
    }
}
