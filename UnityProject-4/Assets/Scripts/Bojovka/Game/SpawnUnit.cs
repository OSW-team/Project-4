using UnityEngine;
using System.Collections;

public class SpawnUnit : MonoBehaviour {
    public GameObject[] units;
    public Transform spawnPoint;
    public MasterMindNHWheels master;
    public Transform pointDown, pointDeploy, pointMarch;
    public GameObject enemySF;

    public Transform spawnArea;

    public int team;

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
        if (Input.GetMouseButtonDown(0) && Physics.Raycast(_ray, out _hit, Mathf.Infinity, layerMask) && (_hit.point - spawnArea.position).sqrMagnitude < spawnArea.localScale.x * spawnArea.localScale.x / 4)
        {
            GameObject _unit = Instantiate(units[0], spawnPoint.position, spawnPoint.rotation) as GameObject;
            _unit.GetComponent<UnitStats>().team = team;
            master.AddAgent(_unit, pointDown.position);
            _unit.GetComponent<StateMachine>().SetupStateSet(pointDown.position, _hit.point, pointMarch.position, enemySF);
        }
    }
}
