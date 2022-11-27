using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    //MAIN MENU AND DEATH SCREEN 

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //if its in the starting menu screen
        if ((Input.anyKeyDown) && SceneManager.GetActiveScene().name == "Menu")
        {
            //if the user presses any button or mouse button it will start the game
            SceneManager.LoadScene(1);
            EventManager.Instance.PostEventNotification(EventManager.EVENT_TYPE.GAME_START, this, null);
        }
    }
}
