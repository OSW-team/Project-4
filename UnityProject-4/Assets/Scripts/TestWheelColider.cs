using UnityEngine;
using System.Collections;

public class TestWheelColider : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	    if (Input.GetKey(KeyCode.W))
	    {
	        transform.position += Vector3.forward*5;
	    }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position += Vector3.back * 5;
        }
    }
}
