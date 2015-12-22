using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[Serializable]
[CustomEditor(typeof (CityItem))]
[CanEditMultipleObjects]
public class CityItemInspector : Editor
{
    private CityItem _item;
    private GUIStyle Style;
    void OnEnable()
    {
        _item = (CityItem) target;
        if (_item.Type == null){ _item.Type = new bool[3] {false,false,false}; Debug.Log("Null"); }
        if (_item.Square == null) _item.Square = new bool[3] {false,false,false};
        if (_item.Shape == null) _item.Shape = new bool[2] {false,false};
        if (_item.Placement == null) _item.Placement = new bool[3] {false,false,false};
        if (_item.Orientation == null) _item.Orientation = new bool[2] {false,false};
        if (_item.Exposure == null) _item.Exposure = new bool[2] {false,false};
        Style = Resources.Load<GUISkin>("MySkin").GetStyle("Label");
    }

    public void OnDisable()
    {

    }

    public override void OnInspectorGUI()
    {
        

        EditorGUILayout.LabelField("Type", Style);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Building", GUILayout.ExpandWidth(false), GUILayout.Width(48));
        _item.Type[0] = EditorGUILayout.Toggle("", _item.Type[0], GUILayout.ExpandWidth(false), GUILayout.Width(28));
        EditorGUILayout.LabelField("Debris", GUILayout.ExpandWidth(false), GUILayout.Width(40));
        _item.Type[1] = EditorGUILayout.Toggle("", _item.Type[1], GUILayout.ExpandWidth(false), GUILayout.Width(28));
        EditorGUILayout.LabelField("Part", GUILayout.ExpandWidth(false), GUILayout.Width(28));
        _item.Type[2] = EditorGUILayout.Toggle("", _item.Type[2], GUILayout.ExpandWidth(false), GUILayout.Width(28));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.LabelField("Square", Style);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Narrow", GUILayout.ExpandWidth(false), GUILayout.Width(48));
        _item.Square[0] = EditorGUILayout.Toggle("", _item.Square[0], GUILayout.ExpandWidth(false), GUILayout.Width(28));
        EditorGUILayout.LabelField("Average", GUILayout.ExpandWidth(false), GUILayout.Width(40));
        _item.Square[1] = EditorGUILayout.Toggle("", _item.Square[1], GUILayout.ExpandWidth(false), GUILayout.Width(28));
        EditorGUILayout.LabelField("Wide", GUILayout.ExpandWidth(false), GUILayout.Width(28));
        _item.Square[2] = EditorGUILayout.Toggle("", _item.Square[2], GUILayout.ExpandWidth(false), GUILayout.Width(28));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.LabelField("Shape", Style);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Round", GUILayout.ExpandWidth(false), GUILayout.Width(48));
        _item.Shape[0] = EditorGUILayout.Toggle("", _item.Shape[0], GUILayout.ExpandWidth(false), GUILayout.Width(28));
        EditorGUILayout.LabelField("Angular", GUILayout.ExpandWidth(false), GUILayout.Width(40));
        _item.Shape[1] = EditorGUILayout.Toggle("", _item.Shape[1], GUILayout.ExpandWidth(false), GUILayout.Width(28));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.LabelField("Placement", Style);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Face", GUILayout.ExpandWidth(false), GUILayout.Width(48));
        _item.Placement[0] = EditorGUILayout.Toggle("", _item.Placement[0], GUILayout.ExpandWidth(false), GUILayout.Width(28));
        EditorGUILayout.LabelField("Edge", GUILayout.ExpandWidth(false), GUILayout.Width(40));
        _item.Placement[1] = EditorGUILayout.Toggle("", _item.Placement[1], GUILayout.ExpandWidth(false), GUILayout.Width(28));
        EditorGUILayout.LabelField("Corner", GUILayout.ExpandWidth(false), GUILayout.Width(28));
        _item.Placement[2] = EditorGUILayout.Toggle("", _item.Placement[2], GUILayout.ExpandWidth(false), GUILayout.Width(28));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.LabelField("Orientation", Style);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Horizontal", GUILayout.ExpandWidth(false), GUILayout.Width(48));
        _item.Orientation[0] = EditorGUILayout.Toggle("", _item.Orientation[0], GUILayout.ExpandWidth(false), GUILayout.Width(28));
        EditorGUILayout.LabelField("Vertical", GUILayout.ExpandWidth(false), GUILayout.Width(40));
        _item.Orientation[1] = EditorGUILayout.Toggle("", _item.Orientation[1], GUILayout.ExpandWidth(false), GUILayout.Width(28));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.LabelField("Exposure", Style);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Internal", GUILayout.ExpandWidth(false), GUILayout.Width(48));
        _item.Exposure[0] = EditorGUILayout.Toggle("", _item.Exposure[0], GUILayout.ExpandWidth(false), GUILayout.Width(28));
        EditorGUILayout.LabelField("External", GUILayout.ExpandWidth(false), GUILayout.Width(40));
        _item.Exposure[1] = EditorGUILayout.Toggle("", _item.Exposure[1], GUILayout.ExpandWidth(false), GUILayout.Width(28));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.LabelField("RandomRotation", Style);
        EditorGUILayout.BeginHorizontal();
        _item.RandomRotation = EditorGUILayout.Vector3Field("",_item.RandomRotation);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.LabelField("Weight", Style);
        EditorGUILayout.BeginHorizontal();
        _item.Weight = EditorGUILayout.IntField(_item.Weight);
        EditorGUILayout.EndHorizontal();

        if (GUI.changed)EditorUtility.SetDirty(_item);
    }

}
