using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TestGlobalMapController : MonoBehaviour
{
    public SteamCitadel CurrentCitadel;
    public Transform MapTransform;
    private Camera _camera;
    private NavMeshAgent _agent;
    private Vector3 _target;
    // Use this for initialization
    void Start()
    {
        _camera = FindObjectOfType<Camera>();
        var citadel = new SteamCitadel("PlayersSC");
        XMLWorker.LoadSC(citadel);
        CurrentCitadel = citadel;
        CurrentCitadel.BuildCitadelMesh();
        CurrentCitadel.GO.transform.SetParent(MapTransform);
        CurrentCitadel.GO.transform.localPosition = Vector3.zero;
        CurrentCitadel.GO.AddComponent<BoxCollider>();
        CurrentCitadel.GO.AddComponent<Rigidbody>().isKinematic = true;
        _agent = CurrentCitadel.GO.AddComponent<NavMeshAgent>();
        _target = CurrentCitadel.GO.transform.position;
        //var controls = CurrentCitadel.GO.AddComponent<SimpleAgentScript>();
        //controls.Target = CurrentCitadel.GO.transform;
    }

    void Update()
    {
        _agent.SetDestination(_target);

        if(Input.GetMouseButtonDown(1))
        {
            var ray =_camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit))
            {
                _target = hit.point;
            }
        }
        if(Input.GetKeyDown(KeyCode.I))
        {
            var list = CurrentCitadel.Inventory.Items;
            foreach (var s in list)
            {
                Debug.Log(s.Name.ToString());
            }
            
        }
    }

}
