using UnityEngine;
using System.Collections;

public class City : MonoBehaviour {
    public Inventory Inventory;

	// Use this for initialization
	void Start () {
        Inventory = new Inventory();

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void  OnTriggerEnter(Collider col)
    {
        var items = FindObjectOfType<TestGlobalMapController>().CurrentCitadel.Inventory.Items;
        if (col.name == "Citadel PlayersSC"&& items.Count>0)
        {
            Inventory.Items.Add(items[0]);
            items.RemoveAt(0);
            Debug.Log("City items count = " + Inventory.Items.Count);
        }
    }
}
