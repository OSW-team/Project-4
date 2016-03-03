using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Inventory : MonoBehaviour {
    public List<InventoryItem> Items;
    public Inventory()
    {
        Items = new List<InventoryItem>();
        ListItems.ItemAdded += ItemAdded;
    }

    private void ItemAdded(InventoryItem item)
    {
        Debug.Log(item.Name + " ItemAdded + Amount = " +item.ItemAmount);
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

public static class ListItems
{
    public delegate void AddItemDelegate(InventoryItem item);
    public static event AddItemDelegate ItemAdded;
    public static void AddItem(this List<InventoryItem> items, InventoryItem item)
    {
        items.Add(item);
        ItemAdded(item);
    }
}
