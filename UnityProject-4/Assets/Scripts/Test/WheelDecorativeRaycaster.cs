﻿using UnityEngine;
using System.Collections;

public class WheelDecorativeRaycaster : MonoBehaviour {
    public Transform WheelBoneTransform;
    public float WheelDiameter;
    public float RaycastLength;
    public float MaxWheelDistance;
    private Vector3 _originalWeelPosition;
    
	// Use this for initialization
	void Start () {
        _originalWeelPosition = new Vector3(WheelBoneTransform.localPosition.x, WheelBoneTransform.localPosition.y, WheelBoneTransform.localPosition.z);


    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.J))
        {
            this.enabled = false;
        }
        Vector3 dwn = Vector3.down;
        RaycastHit hit;
        
        if (Physics.Raycast(transform.position, dwn, out hit, RaycastLength))
        {
            if(hit.distance <= MaxWheelDistance && hit.collider.gameObject.layer!=8 && hit.distance >= WheelDiameter * WheelBoneTransform.lossyScale.y)
            {
               // Debug.Log(hit.collider.name);
                var pos = hit.point+WheelDiameter*WheelBoneTransform.lossyScale;
                WheelBoneTransform.position = new Vector3(WheelBoneTransform.position.x, pos.y, WheelBoneTransform.position.z);
                WheelBoneTransform.localPosition = new Vector3(_originalWeelPosition.x, WheelBoneTransform.localPosition.y, _originalWeelPosition.z);
            }
        }
    }
}