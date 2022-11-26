using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialoguePiece", menuName = "CustomItems/DialoguePiece")]
public class DialogueObj : ScriptableObject
{
    public string npcName;
    public string[] sentences;
    public bool rewardAfter;
}
