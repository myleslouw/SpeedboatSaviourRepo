using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public GameObject questBox;

    // Start is called before the first frame update
    void Start()
    {
        EventManager.OnDelegateEvent StartQuestDelegate = StartQuest;
        EventManager.Instance.AddListener(EventManager.EVENT_TYPE.START_QUEST, StartQuestDelegate);

    }

    public void StartQuest(EventManager.EVENT_TYPE eventType, Component sender, object Params = null)
    {
        //show the quest in UI
        questBox.SetActive(true);


    }
}
