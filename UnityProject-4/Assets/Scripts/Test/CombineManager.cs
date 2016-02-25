using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class CombineManager : MonoBehaviour {
    private List<CombineChildren> _combiningGroups;
	// Use this for initialization
	void Start () {
        gameObject.AddComponent<CombineChildren>();
        _combiningGroups = new List<CombineChildren>();
        SearchCombiningGroups(transform);
        CombineInOrder();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void SearchCombiningGroups(Transform tr)
    {
        var combsAtLayer = new List<Transform>();
        for (var i = 0; i < tr.childCount; i++)
        {
            var childTransforms = tr.GetComponentsInChildren<Transform>().Where(x => x.gameObject.CompareTag("CombineSecond")).ToArray();
            if (tr.GetChild(i).CompareTag("CombineSecond"))
            {
                var combineComponent = tr.GetChild(i).gameObject.AddComponent<CombineChildren>();
                _combiningGroups.Add(combineComponent);
            }
            if (childTransforms.Length > 0) combsAtLayer.Add(tr.GetChild(i));
        }
        foreach (var com in combsAtLayer)
        {
            SearchCombiningGroups(com);
        }
    }

    private void CombineInOrder()
    {
        _combiningGroups.Reverse();
        foreach(var com in _combiningGroups)
        {
            com.Combine();
        }
        gameObject.GetComponent<CombineChildren>().Combine();
    }
}
