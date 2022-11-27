using System;using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class NPCAnim : MonoBehaviour
{
    private static NPCState _currentState;

    private static Animator _control;

    private static string idle = "IsIdle", talking = "IsTalking";

    private void Start()
    {
        _control = GetComponent<Animator>();
        _currentState = NPCState.IDLE;
        SwitchState(_currentState);
    }

    //For Debugging Purposes
    [ContextMenu("Switch State")]
    private void Control()
    {
        SwitchState(_currentState);
    }

    public static void SwitchState(NPCState newState)
    {
        _currentState = newState;

        print(newState);
        
        if (_currentState == NPCState.IDLE)
        {
            _control.SetBool(idle, true);
            _control.SetBool(talking, false);
            return;
        }

        if (_currentState == NPCState.TALKING)
        {
            _control.SetBool(idle, false);
            _control.SetBool(talking, true);
            return;
        }
    }
}

public enum NPCState
{
    IDLE,
    TALKING
}
