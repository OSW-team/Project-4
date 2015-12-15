using UnityEngine;
using System.Collections;

public class SteamCitadelConstrutor : MonoBehaviour {
    public GameObject Bridge;
    public GameObject Constructor;
    public GameObject Engine;
    public GameObject Radar;
    public GameObject Storage;
    public GameObject Weapon;
    public GameObject Platform;
	// Use this for initialization
	void Start () {
	Initialize();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void Initialize()
    {
        Debug.Log(XMLWorker.LoadItem(XMLWorker.EnumGameItemType.Citadel, "0", "HP"));
    }
}
