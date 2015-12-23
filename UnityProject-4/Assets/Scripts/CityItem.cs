using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[Serializable]
public class CityItem : MonoBehaviour
{
    [SerializeField]
    public bool[] Type;
    [SerializeField]
    public bool[] Square;
    [SerializeField]
    public bool[] Shape;
    [SerializeField]
    public bool[] Placement;
    [SerializeField]
    public bool[] Orientation;
    [SerializeField]
    public bool[] Exposure;
    public Vector3 RandomRotation;
    [SerializeField]
    public float AllowToRepeatAtDistance;
    [SerializeField]
    public int Weight;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    public string GetTypes()
    {
        var resSt = "";

        if (Type[0] == true) resSt += "Building,";
        if (Type[1] == true) resSt += "Derbis,";
        if (Type[2] == true) resSt += "Part,";
        return resSt;
    }

    public string GetSquares()
    {
        var resSt = "";

        if (Square[0] == true) resSt += "Narrow,";
        if (Square[1] == true) resSt += "Average,";
        if (Square[2] == true) resSt += "Wide,";
        return resSt;
    }

    public string GetShapes()
    {
        var resSt = "";

        if (Shape[0] == true) resSt += "Round,";
        if (Shape[1] == true) resSt += "Angular,";
        return resSt;
    }

    public string GetPlacements()
    {
        var resSt = "";

        if (Placement[0] == true) resSt += "Face,";
        if (Placement[1] == true) resSt += "Edge,";
        if (Placement[2] == true) resSt += "Corner,";
        return resSt;
    }

    public string GetOrientations()
    {
        var resSt = "";

        if (Orientation[0] == true) resSt += "Horizontal,";
        if (Orientation[1] == true) resSt += "Vertical,";
        return resSt;
    }

    public string GetExposures()
    {
        var resSt = "";

        if (Exposure[0] == true) resSt += "Internal,";
        if (Exposure[1] == true) resSt += "External,";
        return resSt;
    }

}
