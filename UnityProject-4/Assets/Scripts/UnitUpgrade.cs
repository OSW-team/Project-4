using UnityEngine;
using System.Collections;

public class UnitUpgrade : Unit {

	// Use this for initialization
    public UnitUpgrade(string CSname,string unitName, string workname) 
    {
        XMLWorker.LoadUnit(workname, this);
    }

    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
