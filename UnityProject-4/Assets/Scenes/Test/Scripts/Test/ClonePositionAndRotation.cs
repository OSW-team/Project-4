using UnityEngine;
using System.Collections;

public class ClonePositionAndRotation : MonoBehaviour {
    public Transform TargetTransform;
    public float YOffset;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = TargetTransform.position+Vector3.up*YOffset;
        transform.rotation = TargetTransform.rotation;
	}
}
