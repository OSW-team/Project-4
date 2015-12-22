using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

public class CityConstructor : MonoBehaviour
{
    public GameObject CityContainerGameObject;
    
    private List<Transform> _locators;
    private List<GameObject> _prefabs;
    private GameObject _currentPrefab;
    private GameObject _currentGameObject;
    private float _percentFilterAmount;
    private string _typeFilter;
    private string _squareFilter;
    private string _shapeFilter;
    private string _placemetFilter;
    private string _orientationFilter;
    private string _exposureFilter;
    private int _itemChance;
    private List<bool> _rotationPermission;
	// Use this for initialization
	void Start ()
	{
        _locators = new List<Transform>();
        _rotationPermission = new List<bool>(3) {false,false,false};
        var locators = CityContainerGameObject.transform.FindChild("Locators");
        Iteration(locators);

	}

    public void Iteration(Transform locatorsTransform)
    {
        var locators = GetLocators(locatorsTransform);
        foreach (var locator in locators)
        {
            ParseLocatorNameToFilters(locator.name);
            if (_typeFilter == "") return;
            var r = Random.Range(1, 101);
            if (r <= _itemChance)
            {
                GetPrefabs("Prefabs/");
                SelectByFilters();
                if (_currentPrefab != null)
                {
                    PutPrefabToLocator(locator, _currentPrefab);
                    if (_currentPrefab != null)
                    {
                        var innerLocatorsTransform = _currentGameObject.transform.FindChild("Locators");
                        if (innerLocatorsTransform)
                        {
                            Iteration(innerLocatorsTransform);
                        }
                    }
                }
               
            }

        }
       
    }


    // Update is called once per frame
	void Update () {
	
	}

    public List<Transform> GetLocators(Transform locatorsTransform){
        var locators = new List<Transform>();

        for (var i = 0; i < locatorsTransform.childCount; i++)
        {
            locators.Add(locatorsTransform.GetChild(i));
        }
        return locators;
    }

    public void GetPrefabs(string path)
    {
        _prefabs = Resources.LoadAll<GameObject>(path).ToList();
    }

    public void ParseLocatorNameToFilters(string locatorName)
    {
        _typeFilter = "";
        _squareFilter = "";
        _shapeFilter = "";
        _placemetFilter = "";
        _orientationFilter = "";
        _exposureFilter = "";
        var nameParts = locatorName.Split('_').ToList();
        foreach (var namePart in nameParts)
        {
            if (namePart.Contains("Building") || namePart.Contains("Debris") || namePart.Contains("Part"))
            {
                _typeFilter = namePart;
            }
            if (namePart.Contains("Narrow") || namePart.Contains("Average") || namePart.Contains("Wide"))
            {
                _squareFilter = namePart;
            }
            if (namePart.Contains("Round") || namePart.Contains("Angular"))
            {
                _shapeFilter = namePart;
            }
            if (namePart.Contains("Face") || namePart.Contains("Edge") || namePart.Contains("Corner"))
            {
                _placemetFilter = namePart;
            }
            if (namePart.Contains("Horizontal") || namePart.Contains("Vertical"))
            {
                _orientationFilter = namePart;
            }
            if (namePart.Contains("External") || namePart.Contains("Internal"))
            {
                _exposureFilter = namePart;
            }
            if (namePart.Contains("RXR") || namePart.Contains("RYR") || namePart.Contains("RZR"))
            {
                switch (namePart)
                {
                    case "RXR":
                        _rotationPermission[0] = true;
                        break;
                    case "RYR":
                        _rotationPermission[1] = true;
                        break;
                    case "RZR":
                        _rotationPermission[2] = true;
                        break;
                }
            }
            if (namePart.Contains("["))
            {
                _itemChance = Int32.Parse(namePart.Replace("[","").Replace("]",""));
            }
        }

    }

    private void SelectByFilters()
    {
        
        var candidatList  = new List<GameObject>();
        candidatList = (from prefab in _prefabs let props = prefab.GetComponent<CityItem>() where props.GetTypes().Contains(_typeFilter) select prefab).ToList();
        if (_squareFilter!= "") { candidatList = (from prefab in candidatList let props = prefab.GetComponent<CityItem>() where props.GetSquares().Contains(_squareFilter) select prefab).ToList(); Debug.Log("Square = " +_squareFilter);}
        if (_shapeFilter != "") { candidatList = (from prefab in candidatList let props = prefab.GetComponent<CityItem>() where props.GetShapes().Contains(_shapeFilter)  select prefab).ToList(); Debug.Log("Shape = " + _shapeFilter); }
        if (_placemetFilter != "") candidatList = (from prefab in candidatList let props = prefab.GetComponent<CityItem>() where props.GetPlacements().Contains(_placemetFilter) select prefab).ToList();
        if (_orientationFilter != "") candidatList = (from prefab in candidatList let props = prefab.GetComponent<CityItem>() where props.GetOrientations().Contains(_orientationFilter)  select prefab).ToList();
        if (_exposureFilter != "") candidatList = (from prefab in candidatList let props = prefab.GetComponent<CityItem>() where props.GetExposures().Contains(_exposureFilter) select prefab).ToList();
        SelectFinalPrefab(candidatList);

    }

    private void SelectFinalPrefab(List<GameObject> finalList)
    {
        if (finalList.Count != 0)
        {
            var indexList = new List<int>();
            for (var k = 0; k < finalList.Count; k++)
            {
                var weight = finalList[k].GetComponent<CityItem>().Weight;
                if (weight < 0) weight = 1;
                for (var i = 0; i < weight; i++)
                {
                    indexList.Add(k);
                }
            }
            var r = Random.Range(0, indexList.Count);
            _currentPrefab = finalList[indexList[r]];
        }
        else
        {
            _currentPrefab = null;
        }
    }

    private void PutPrefabToLocator(Transform locator, GameObject prefab)
    {

        var go = Instantiate(prefab);
        _currentGameObject = go;
        _currentGameObject.transform.position = locator.position;
        _currentGameObject.transform.rotation = locator.rotation;
        
        if(_rotationPermission[0]) _currentGameObject.transform.localEulerAngles = new Vector3(Random.Range((locator.localEulerAngles.x-_currentPrefab.GetComponent<CityItem>().RandomRotation.x), (locator.localEulerAngles.x + _currentPrefab.GetComponent<CityItem>().RandomRotation.x)),locator.eulerAngles.y,locator.eulerAngles.z);
        if (_rotationPermission[1]) _currentGameObject.transform.localEulerAngles = new Vector3(locator.localEulerAngles.x, Random.Range((locator.localEulerAngles.y - _currentPrefab.GetComponent<CityItem>().RandomRotation.y), (locator.localEulerAngles.y + _currentPrefab.GetComponent<CityItem>().RandomRotation.y)), locator.eulerAngles.z);
        if (_rotationPermission[2]) _currentGameObject.transform.localEulerAngles = new Vector3(locator.localEulerAngles.x, locator.eulerAngles.y, Random.Range((locator.localEulerAngles.z - _currentPrefab.GetComponent<CityItem>().RandomRotation.z), (locator.localEulerAngles.z + _currentPrefab.GetComponent<CityItem>().RandomRotation.z)));

        _currentGameObject.transform.SetParent(locator.parent);

    }

}
