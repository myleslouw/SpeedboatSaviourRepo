using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class NPC : MonoBehaviour
{

    public CinemachineVirtualCamera npcCamera;     //each NPC has a virtual cam attached that the player will switch to when they in range (collider)
    DialogueManager dialogueManager;                //a reference to the dialogue manager
    public DialogueObj dialogue;                    //the dialogue obj that holds the sentances
    public string npcName;                          //the NPCs name

    private void Start()
    {
        npcName = dialogue.npcName;
    }

}
