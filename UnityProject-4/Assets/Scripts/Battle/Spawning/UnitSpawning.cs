using UnityEngine;
using System.Collections;

public class UnitSpawning : MonoBehaviour {
    public GameObject[] units;
    public MasterMindNHWheels master;
    int layerMask = ~( 1 << 8);
    public Transform spawnArea;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit _hit;
        if (Input.GetMouseButtonDown(0) && Physics.Raycast(_ray, out _hit, Mathf.Infinity, layerMask) && (_hit.point-spawnArea.position ).sqrMagnitude < spawnArea.localScale.x* spawnArea.localScale.x/4)
        {
            //Debug.Log("something spawns");
            //GameObject _unit = Instantiate(units[0], _hit.point, Quaternion.identity) as GameObject;
            master.AddAgent(units[0], _hit.point);
        }
    }
}
