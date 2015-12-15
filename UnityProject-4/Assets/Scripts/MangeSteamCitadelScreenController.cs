using UnityEngine;
using System.Collections;

public class MangeSteamCitadelScreenController : MonoBehaviour
{
    public Camera Camera;
    public GameObject SteamCitadelManagementScreen;
    public GameObject DraggableField;
    public Vector3 CurrentPosition;
    public GameObject SteamCitadel;
    public float RotationSpeed;
    private bool _startDrag;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Show()
    {
        SteamCitadelManagementScreen.SetActive(true);

    }

    public void OnDraggableFieldDown()
    {
        if (!_startDrag)
        {
            _startDrag = true;
           // var curPos = Input.mousePosition;
            CurrentPosition = Input.mousePosition; 
            Debug.Log("Down");
        } 
    }

    public void OnDraggableFieldUp()
    {
        _startDrag = false;
        //var curPos = Input.mousePosition;
        CurrentPosition = Input.mousePosition;
        Debug.Log("Up");
    }


    public void OnDrag()
    {
        var offset = Input.mousePosition.x - CurrentPosition.x;
        var rotY= -1*offset*RotationSpeed;
        Debug.Log(offset);
        SteamCitadel.transform.localEulerAngles = new Vector3(SteamCitadel.transform.localEulerAngles.x, SteamCitadel.transform.localEulerAngles.y+rotY, SteamCitadel.transform.localEulerAngles.z);
    }
}
