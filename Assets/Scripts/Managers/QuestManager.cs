using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    private QuestObject currentQuest;
    public bool questActive;
    // Start is called before the first frame update
    void Start()
    {
        //subscribes to event
        EventManager.OnDelegateEvent StartQuestDelegate = StartQuest;
        EventManager.Instance.AddListener(EventManager.EVENT_TYPE.START_QUEST, StartQuestDelegate);
        //EventManager.OnDelegateEvent CompleteQuestDelegate = CompleteQuest;
       // EventManager.Instance.AddListener(EventManager.EVENT_TYPE.COMPLETE_QUEST, StartQuestDelegate);
        EventManager.OnDelegateEvent ProgressDelegate = CheckQuestProgress;
        EventManager.Instance.AddListener(EventManager.EVENT_TYPE.PICKUP_UI, ProgressDelegate);
    }

    public void StartQuest(EventManager.EVENT_TYPE eventType, Component sender, object Params = null)
    {
        QuestGiver giver = (QuestGiver)Params;
        currentQuest = giver.questObj;
        questActive = true;
    }

    public void CompleteQuest()
    {
        print("Quest Complete");
        questActive = false;
        //trigger the milestone
        EventManager.Instance.PostEventNotification(EventManager.EVENT_TYPE.COMPLETE_QUEST, this, GameManager.Level + 1);
    }

    public void CheckQuestProgress(EventManager.EVENT_TYPE eventType, Component sender, object Params = null)
    {
        //check the progress if there is a quest active
        if (questActive)
        {
            //checks if the number in the inventory is equal or more than the number required for the quest
            bool glassComplete = CheckAmount(PollutantType.type.Glass, currentQuest.GlassRequirement);
            bool gwComplete = CheckAmount(PollutantType.type.GeneralWaste, currentQuest.GWRequirement);
            bool plasticComplete = CheckAmount(PollutantType.type.Plastic, currentQuest.PlasticRequirement);

            if (glassComplete == gwComplete && gwComplete == plasticComplete && plasticComplete == true)
            {
                CompleteQuest();
            }
        }
    }

    private bool CheckAmount(PollutantType.type PolType, int requirement)
    {
        //checks if it meets the min amount for the requirement
        if (Inventory.Instance.PollutantInventory[PolType] >= requirement)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
