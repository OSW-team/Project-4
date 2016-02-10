using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class BoxMovement : MonoBehaviour {
    // public bool IsPhysics;
    // Use this for initialization
    public List<Transform> RayPoints;
    public float RaycastLength;
    public float coef;
    private Rigidbody RigidBody;
	void Start () {
        RigidBody = GetComponent<Rigidbody>();


    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKey(KeyCode.W))
        {
            transform.localPosition += Vector3.forward * 0.10f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.localPosition += Vector3.back * 0.10f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.localPosition += Vector3.right * 0.10f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.localPosition += Vector3.left * 0.10f;
        }

        foreach (var tr in RayPoints)
        {
            Vector3 dwn = Vector3.down;
            RaycastHit hit;
            if (Physics.Raycast(tr.position, dwn, out hit, RaycastLength))
            {
                //print("distance to the ground = " + hit.distance);
                var delta = RaycastLength - hit.distance;
                RigidBody.AddForceAtPosition(-Physics.gravity.y * delta * coef * Vector3.up, tr.position);
            }
            
        }
    }
}
