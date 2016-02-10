using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class InputRaycastRigidbody : MonoBehaviour {
    public GameObject UnitPrefab;
    public Transform EditableUnitParent;
    public InputField RaycastLength;
    public InputField Coef;
    public InputField Mass;
    public InputField Drag;
    public InputField AngularDrag;
    public Text RaycastLengthValue;
    public Text CoefValue;
    public Text MassValue;
    public Text DragValue;
    public Text AngularDragValue;
    public GameObject ShowButton;
    public GameObject HideButton;
    private List<BoxMovement> _unitsList;

    // Use this for initialization
    void Start () {
        RaycastLength.text = UnitPrefab.GetComponent<BoxMovement>().RaycastLength.ToString();
        Coef.text = UnitPrefab.GetComponent<BoxMovement>().coef.ToString();
        Mass.text = UnitPrefab.GetComponent<Rigidbody>().mass.ToString();
        Drag.text = UnitPrefab.GetComponent<Rigidbody>().drag.ToString();
        AngularDrag.text = UnitPrefab.GetComponent<Rigidbody>().angularDrag.ToString();
        _unitsList = new List<BoxMovement>(EditableUnitParent.GetComponentsInChildren<BoxMovement>());
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void AcceptChanges()
    {
        foreach (var EditableUnit in _unitsList)
        {
            EditableUnit.RaycastLength = Convert.ToSingle(RaycastLengthValue.text);
            EditableUnit.coef = Convert.ToSingle(CoefValue.text);
            EditableUnit.GetComponent<Rigidbody>().mass = Convert.ToSingle(MassValue.text);
            EditableUnit.GetComponent<Rigidbody>().drag = Convert.ToSingle(DragValue.text);
            EditableUnit.GetComponent<Rigidbody>().angularDrag = Convert.ToSingle(AngularDragValue.text);
        } 
    }

    public void SavePrefab()
    {
        UnitPrefab.GetComponent<BoxMovement>().RaycastLength = Convert.ToSingle(RaycastLengthValue.text);
        UnitPrefab.GetComponent<BoxMovement>().coef = Convert.ToSingle(CoefValue.text);
        UnitPrefab.GetComponent<Rigidbody>().mass = Convert.ToSingle(MassValue.text);
        UnitPrefab.GetComponent<Rigidbody>().drag = Convert.ToSingle(DragValue.text);
        UnitPrefab.GetComponent<Rigidbody>().angularDrag = Convert.ToSingle(AngularDragValue.text);
    }

    public void Show(bool show)
    {
        transform.FindChild("Panel").gameObject.SetActive(show);
        ShowButton.SetActive(!show);
        HideButton.SetActive(show);
    }

    public void LoadAnotherScene()
    {
        SceneManager.LoadScene(1);
    }
}
