using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[Serializable]
[CustomEditor(typeof(UpgradeController))]
[CanEditMultipleObjects]
public class UpgradeControllerInspector : Editor
{
    private UpgradeController _upgradeController;
    private string[] _upgrades;
    private string[] _filters;
    private int _AIndex = 0;
    private int _A1Index = 0;
    private int _BIndex = 0;
    private int _B1Index = 0;
    private int _CIndex = 0;
    private int _C1Index = 0;
    private int _filterFromIndex = 0;
    private int _filterToIndex = 0;
    private bool _getUpgardes;
    public override void OnInspectorGUI()
    {
        _upgradeController = (UpgradeController)target;
        _upgradeController.Workname = EditorGUILayout.TextField("Unit workname", _upgradeController.Workname);
        _AIndex = _upgradeController.AIndex;
        _A1Index = _upgradeController.A1Index;
        _BIndex = _upgradeController.BIndex;
        _B1Index = _upgradeController.B1Index;
        _CIndex = _upgradeController.CIndex;
        _C1Index = _upgradeController.C1Index;
        _filterFromIndex = _upgradeController.FromFilterIndex;
        _filterToIndex = _upgradeController.ToFilterIndex;

        if (GUILayout.Button("Get units upgrades"))
        {
            _getUpgardes = true;
        }
        if (_getUpgardes)
        {
            _upgradeController.GetUnitsUpgrades();
            _getUpgardes = false;
        }
        if (_upgradeController.Upgrades!=null)
        {
            //From
            _upgrades = _upgradeController.Upgrades.ToArray();
            EditorGUILayout.LabelField("From upgrade", GUILayout.ExpandWidth(false), GUILayout.Width(120));
            _AIndex = EditorGUILayout.Popup(_AIndex, _upgrades);
            _upgradeController.A = _upgradeController.Upgrades[_AIndex];
            _upgradeController.AIndex = _AIndex;

            //Operator from popup
            _filters = _upgradeController.Filters.ToArray();
            EditorGUILayout.LabelField("Boolean operators", GUILayout.ExpandWidth(false), GUILayout.Width(120));
            _filterFromIndex = EditorGUILayout.Popup(_filterFromIndex, _filters);
            _upgradeController.CurrentFromFilter = _upgradeController.Filters[_filterFromIndex];
            _upgradeController.FromFilterIndex = _filterFromIndex;
            //
            if (_upgradeController.CurrentFromFilter != "NONE")
            {              
                _BIndex = EditorGUILayout.Popup(_BIndex, _upgrades);
                _upgradeController.B = _upgradeController.Upgrades[_BIndex];
                _upgradeController.BIndex = _BIndex;
                if (_upgradeController.CurrentFromFilter == "AND(_OR_)" || _upgradeController.CurrentFromFilter == "OR(_AND_)" || _upgradeController.CurrentFromFilter == "AND_AND_")
                {
                    _CIndex = EditorGUILayout.Popup(_CIndex, _upgrades);
                    _upgradeController.C = _upgradeController.Upgrades[_CIndex];
                    _upgradeController.CIndex = _CIndex;
                }
            }
            //To
            EditorGUILayout.LabelField("Until upgrade", GUILayout.ExpandWidth(false), GUILayout.Width(120));
            _A1Index = EditorGUILayout.Popup(_A1Index, _upgrades);           
            _upgradeController.A1 = _upgradeController.Upgrades[_A1Index];            
            _upgradeController.A1Index = _A1Index;
            //Operator to popup
            _filters = _upgradeController.Filters.ToArray();
            EditorGUILayout.LabelField("Boolean operators", GUILayout.ExpandWidth(false), GUILayout.Width(120));
            _filterToIndex = EditorGUILayout.Popup(_filterToIndex, _filters);
            _upgradeController.CurrentToFilter = _upgradeController.Filters[_filterToIndex];
            _upgradeController.ToFilterIndex = _filterToIndex;
            //
            if (_upgradeController.CurrentToFilter != "NONE")
            { 
                _B1Index = EditorGUILayout.Popup(_B1Index, _upgrades);
                _upgradeController.B1 = _upgradeController.Upgrades[_B1Index];
                _upgradeController.B1Index = _B1Index;
                if (_upgradeController.CurrentToFilter == "AND(_OR_)" || _upgradeController.CurrentToFilter == "OR(_AND_)" || _upgradeController.CurrentToFilter == "AND_AND_")
                {
                    _C1Index = EditorGUILayout.Popup(_C1Index, _upgrades);
                    _upgradeController.C1 = _upgradeController.Upgrades[_C1Index];
                    _upgradeController.C1Index = _C1Index;
                }
            }

        }
        EditorUtility.SetDirty(_upgradeController);
    }


    // Use this for initialization
        void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
