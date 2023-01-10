using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Pollutant", menuName ="CustomItems/Pollutant")]
public class PollutantObject : ScriptableObject
{
    //scriptable object for every pollutant
    public PollutantType.type pollutantType;            //the type of the pollutant (G, P, GW)
    public Vector3 pollutantRotation;                   //its rotation in the water
    public Vector3 startOffset;                         //GW spawns sideways
}
