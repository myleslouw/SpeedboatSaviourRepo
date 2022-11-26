
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LevelManager : MonoBehaviour
{
    public GameObject[] LevelItems;          //holds all the items in the leve (the index is the level num)

    // Start is called before the first frame update
    void Start()
    {
        EventManager.OnDelegateEvent NextLevelDelegate = NextLevel;
        EventManager.Instance.AddListener(EventManager.EVENT_TYPE.LEVEL_UP,NextLevelDelegate);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextLevel(EventManager.EVENT_TYPE eventType, Component sender, object Params = null)
    {
        //when a level is loaded
        //spawn the relevant NPCs for that level
        LevelItems[GameManager.Level].SetActive(false);
        //increments the level counter
        GameManager.Level++;
        //sets the Level 2 GameObject which holds the levels NPCs, trash spawning point and the sea life of the previously completed area
        LevelItems[GameManager.Level].SetActive(true);

    }
}
