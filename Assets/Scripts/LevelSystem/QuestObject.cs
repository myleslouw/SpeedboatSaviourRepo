using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestObject", menuName = "CustomItems/QuestObject")]
public class QuestObject : ScriptableObject
{
    public int GlassRequirement;
    public int GWRequirement;
    public int PlasticRequirement;
}