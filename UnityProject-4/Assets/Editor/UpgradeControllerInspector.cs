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
    private int _fromUpgradeIndex = 0;
    private int _untilUpgradeIndex = 0;
    private bool _getUpgardes;
    public override void OnInspectorGUI()
    {
        _upgradeController = (UpgradeController)target;
        _upgradeController.Workname = EditorGUILayout.TextField("Unit workname", _upgradeController.Workname);
        _fromUpgradeIndex = _upgradeController.FromIndex;
        _untilUpgradeIndex = _upgradeController.UntilIndex;
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
            _upgrades = _upgradeController.Upgrades.ToArray();
            EditorGUILayout.LabelField("From upgrade", GUILayout.ExpandWidth(false), GUILayout.Width(120));
            _fromUpgradeIndex = EditorGUILayout.Popup(_fromUpgradeIndex, _upgrades);
            EditorGUILayout.LabelField("Until upgrade", GUILayout.ExpandWidth(false), GUILayout.Width(120));
            _untilUpgradeIndex = EditorGUILayout.Popup(_untilUpgradeIndex, _upgrades);
            _upgradeController.ShowFrom = _upgradeController.Upgrades[_fromUpgradeIndex];
            _upgradeController.ShowUntil = _upgradeController.Upgrades[_untilUpgradeIndex];
            _upgradeController.FromIndex = _fromUpgradeIndex;
            _upgradeController.UntilIndex = _untilUpgradeIndex;
            EditorUtility.SetDirty(_upgradeController);
        }
    }


    // Use this for initialization
        void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
