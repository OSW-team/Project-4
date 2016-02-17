using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class ManagementScreen : MonoBehaviour {
    public Camera Camera;
    public GameObject ManagementScreenGameObject;
    public TestController Controller;
    public GameObject DraggableField;
    public Vector3 CurrentPosition;
    public GameObject CurrentManagingGameObject;
    public float RotationSpeed;
    protected bool _startDrag;
    // Use this for initialization
    public void Start()
    {
        Controller = FindObjectOfType<TestController>();

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void OnDraggableFieldDown()
    {
        if (!_startDrag)
        {
            _startDrag = true;
            // var curPos = Input.mousePosition;
            CurrentPosition = Input.mousePosition;

        }
    }

    public void OnDraggableFieldUp()
    {
        _startDrag = false;
        //var curPos = Input.mousePosition;
        CurrentPosition = Input.mousePosition;

    }


    public void OnDrag()
    {
        var offset = Input.mousePosition.x - CurrentPosition.x;
        var rotY = -1 * offset * RotationSpeed;

        CurrentManagingGameObject.transform.localEulerAngles = new Vector3(CurrentManagingGameObject.transform.localEulerAngles.x, CurrentManagingGameObject.transform.localEulerAngles.y + rotY, CurrentManagingGameObject.transform.localEulerAngles.z);
    }

}
