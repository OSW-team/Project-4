using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

public class BoxMovement : MonoBehaviour {
    // public bool IsPhysics;
    // Use this for initialization
    public List<Transform> RayPoints;
    public float RaycastLength;
    public float coef;
	float correct;
	public float power = 1;
    private Rigidbody RigidBody;
    private float _originalDrag;
    private float _OriginalDragAngular;
    public bool _InAir;

    void Start () {
        RigidBody = GetComponent<Rigidbody>();
		_originalDrag = RigidBody.drag;
		_OriginalDragAngular = RigidBody.angularDrag;
		correct = coef /Time.fixedDeltaTime;

    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKey(KeyCode.UpArrow))
        {
           // transform.localPosition += Vector3.forward * 0.10f;
			RigidBody.AddForce(transform.forward * power *coef);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
			RigidBody.AddForce(-transform.forward * power *coef);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
			RigidBody.AddForce(transform.right* power *coef);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
			RigidBody.AddForce(-transform.right* power *coef);
        }
        var hits = 0;
        foreach (var tr in RayPoints)
        {
            Vector3 dwn = Vector3.down;
            RaycastHit hit;
            if (_InAir == true) {
                RigidBody.drag = 2;
                RigidBody.angularDrag = 2;
            }
            if (Physics.Raycast(tr.position, dwn, out hit, RaycastLength, ~(1 << 8)))
            {
                var delta = RaycastLength - hit.distance;
                RigidBody.AddForceAtPosition(-Physics.gravity.y * delta * correct * Vector3.up, tr.position);
                hits++;
                _InAir = false;
                StopCoroutine(Timer());
                RigidBody.drag = _originalDrag;
                RigidBody.angularDrag = _OriginalDragAngular;
            }
        }
        if (hits == 0) { StartCoroutine(Timer()); } 
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(1f);
        _InAir = true;
        Debug.Log("InAir");
    }
}
