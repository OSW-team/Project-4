using UnityEngine;
using System.Collections;

public class BoxMovement : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
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
    }
}
