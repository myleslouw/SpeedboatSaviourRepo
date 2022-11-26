using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CineMachineSwitcher : MonoBehaviour
{
    Animator animator;
    public CinemachineVirtualCamera npcCam;
    public CinemachineFreeLook mainCam;

    //higher priority cam will be show
    private int mainPriority = 10, offCam = 1;

    // Start is called before the first frame update
    void Start()
    {
        //listener for when the NPC talk and Leave event 
        EventManager.OnDelegateEvent SetNPCCamDelegate = SetNPCCam;
        EventManager.OnDelegateEvent RevertNPCCamDelegate = RevertNPCCam;

        EventManager.Instance.AddListener(EventManager.EVENT_TYPE.NPC_TALK, SetNPCCamDelegate);
        EventManager.Instance.AddListener(EventManager.EVENT_TYPE.NPC_LEAVE, RevertNPCCamDelegate);
    }

    private void CameraNPC()
    {
        //focuses on the NPC
        npcCam.gameObject.SetActive(true);
        mainCam.Priority = offCam;
        CinemachineFreeLook.Orbit[] newOrbit = mainCam.m_Orbits;

        npcCam.Priority = mainPriority;
        mainCam.gameObject.SetActive(false);

    }

    private void CameraPlayer()
    {
        //focuses the camera on the player
        mainCam.gameObject.SetActive(true);
        mainCam.Priority = mainPriority;
        npcCam.Priority = offCam;
        npcCam.gameObject.SetActive(false);
    }

    public void SetNPCCam(EventManager.EVENT_TYPE eventType, Component sender, object Params = null)
    {
        // casts the obj recieved from the event to a NPC and then switches to that cam
        NPC NPCinRange = (NPC)Params;
        //sets the npc cam to the one in range
        npcCam = NPCinRange.npcCamera;
        //changes the cam to show the npcw
        CameraNPC();
    }

    public void RevertNPCCam(EventManager.EVENT_TYPE eventType, Component sender, object Params = null)
    {
        //onces the player leaves the NPC speakable area

        //focuses on the player
        CameraPlayer();
    }
}
