using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//used to hold where and what oil to spawn

//type 0 = small
//type 1 - big
public class OilItem
{
    public OilInfo[] OilSpillsInLevel;
    public OilItem(OilInfo[] inputArray)
    {
        OilSpillsInLevel = inputArray;  
    }
}

public class OilInfo
{
    public Vector3 oilPosition;
    public int oilType;

    public OilInfo(Vector3 _position, int _type)
    {
        oilPosition = _position;
        oilType = _type;
    }
}