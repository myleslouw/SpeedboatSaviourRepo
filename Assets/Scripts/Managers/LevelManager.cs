
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LevelManager : MonoBehaviour
{
    public GameObject[] LevelItems;          //holds all the items in the leve (the index is the level num)

    // Start is called before the first frame update
    void Start()
    {
        LevelItems[0].SetActive(true);
        EventManager.OnDelegateEvent NextLevelDelegate = NextLevel;
        EventManager.Instance.AddListener(EventManager.EVENT_TYPE.COMPLETE_QUEST,NextLevelDelegate);
    }

    public void NextLevel(EventManager.EVENT_TYPE eventType, Component sender, object Params = null)
    {
        //when a level is loaded
        //hides the relevant NPCs for completed level
        LevelItems[GameManager.Level].SetActive(false);
        //increments the level counter
        GameManager.Level++;
        //sets the Level 2 GameObject which holds the levels NPC and the sea life of the previously completed area
        LevelItems[GameManager.Level].SetActive(true);

    }
}
