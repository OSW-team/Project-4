using UnityEngine;
using System.Collections;
using System;

public class Mine : MonoBehaviour
{
    public string MineFaction;
    public string MineResourceType;
    public float MineResourceReserve;
    public float MineResourceCurrent;
    public float MineResourceMax;
    public float MineExtractionVolume;
    public float MineExtractionTime;
    public float MineLoadVolume;
    public float MineLoadTime;
    public bool IsExtracting;
    public bool IsLoading;

    // Use this for initialization
    void Start()
    {
        MineResourceReserve = 1000;
        MineResourceMax = 200;
        MineExtractionVolume = 10;
        MineExtractionTime = 5f;
        MineLoadVolume = 30;
        MineLoadTime = 3f;
        Extract();

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider col)
    {
        if (col.name == "Citadel PlayersSC"&& !IsLoading)
        {
            LoadResource();
        }
    }

    void OnTriggerStay(Collider col)
    {
        if (col.name == "Citadel PlayersSC"&&!IsLoading)
        {
            LoadResource();
        }
    }

    void OnTriggerExit(Collider col)
    {
        IsLoading = false;
    }

    public void Extract()
    {
        if (MineResourceCurrent<= MineResourceMax && MineResourceReserve - MineExtractionVolume >= 0)
        {
            StartCoroutine("ExtractCoroutine");
        }
        else
        {
            IsExtracting = false;
            StopCoroutine("ExtractCoroutine");
        }
    }

    private IEnumerator ExtractCoroutine()
    {
        IsExtracting = true;
        yield return new WaitForSeconds(MineExtractionTime);
        MineResourceReserve -= MineExtractionVolume;
        MineResourceCurrent += MineExtractionVolume;
        Extract();
    }

    public void LoadResource()
    {
        if (MineResourceCurrent >= MineExtractionVolume)
        {
            Debug.Log("Start Loading");
            IsLoading = true;
            StartCoroutine("LoadCoroutine");
        }
        else StopCoroutine("LoadCoroutine");
       

    }

    private IEnumerator LoadCoroutine()
    {
        yield return new WaitForSeconds(MineLoadTime);
        var inventory = FindObjectOfType<TestGlobalMapController>().CurrentCitadel.Inventory;
        var st = new InventoryItem("MineItem");
        st.ItemAmount = Convert.ToInt32(Math.Min(MineResourceReserve, MineLoadVolume));
        inventory.Items.AddItem(st);
        MineResourceCurrent -= st.ItemAmount;
        if (!IsExtracting) Extract();
        Debug.Log("Loaded");
        IsLoading = false;
    }
}
