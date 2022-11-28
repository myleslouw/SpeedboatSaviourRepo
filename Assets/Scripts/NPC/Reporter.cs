using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reporter : MonoBehaviour
{
    //HANDLES SHOWING THE MILESTONES AFTER A QUEST
    public Sprite[] NewspaperArticles;
    public bool MilestoneShown;
    public bool MilestoneReady;

    private void Start()
    {
        EventManager.OnDelegateEvent SetMilestoneDelegate = SetMilestone;
        EventManager.OnDelegateEvent ResetMilestoneDelegate = ResetMilestone;
        EventManager.Instance.AddListener(EventManager.EVENT_TYPE.COMPLETE_QUEST, SetMilestoneDelegate);
        EventManager.Instance.AddListener(EventManager.EVENT_TYPE.SHOW_MILESTONE, ResetMilestoneDelegate);
    }


    public void SetMilestone(EventManager.EVENT_TYPE eventType, Component sender, object Params = null)
    {
        MilestoneReady = true;
        MilestoneShown = false;
    }

    public void ResetMilestone(EventManager.EVENT_TYPE eventType, Component sender, object Params = null)
    {
        MilestoneReady = false;
        MilestoneShown = true;
    }
}
