using UnityEngine;
using System.Collections;

public class SwitcLod : MonoBehaviour {
    private bool show;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
        if(Input.GetKeyDown(KeyCode.B))
        {
            transform.GetChild(1).gameObject.SetActive(show);
            transform.GetChild(0).gameObject.SetActive(!show);
            show = !show;
        }
	}
}
