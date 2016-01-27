using UnityEngine;
using System.Collections;

public class TankWheelsToBonesAttacheer : MonoBehaviour {
    public GameObject TargetWheel;
    public Vector3 Offset;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = TargetWheel.transform.position+Offset;
	}
}
