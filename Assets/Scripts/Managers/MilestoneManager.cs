using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilestoneManager : MonoBehaviour
{
    public int currentMilestone;

    public static MilestoneManager Instance
    {
        get { return instance; }
        set { }
    }

    private static MilestoneManager instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            DestroyImmediate(gameObject);
        }
    }

    private void Start()
    {
        currentMilestone = GameManager.Level;
      
       
        EventManager.OnDelegateEvent LevelUpDelegate = LevelUp;
        EventManager.Instance.AddListener(EventManager.EVENT_TYPE.COMPLETE_QUEST, LevelUpDelegate);
    }

    private void Update()
    {
        //for testing
        if (Input.GetKeyDown(KeyCode.Equals))
        {
            EventManager.Instance.PostEventNotification(EventManager.EVENT_TYPE.COMPLETE_QUEST, this, GameManager.Level + 1);
        }
    }

    private void LevelUp(EventManager.EVENT_TYPE eventType, Component sender, object Params = null)
    {
        //increases the milestone number
        currentMilestone++;
        print("Leveled UP");

    }
}
