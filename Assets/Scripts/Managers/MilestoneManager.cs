using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilestoneManager : MonoBehaviour
{
    public int currentMilestone;
    public int currentXP;
    public int nextLevelXPRequirement;

    Dictionary<int, int> LevelRequirements = new Dictionary<int, int>()
    {
        //holds XP needed for each milestone
        //{ key, XP value needed for LevelUP }
        { 0, 10 },
        { 1, 25 },
        { 2, 40 },
        { 3, 55 },
    };

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
        currentXP = 0;
        currentMilestone = GameManager.Level;
      
        //sets the next lvl xp
        nextLevelXPRequirement = LevelRequirements[currentMilestone];

        EventManager.OnDelegateEvent AddXPDelegate = AddXP;
        EventManager.Instance.AddListener(EventManager.EVENT_TYPE.ADD_XP, AddXPDelegate);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Equals))
        {
            LevelUp();
        }
    }

    public void AddXP(EventManager.EVENT_TYPE eventType, Component sender, object Params = null)
    {
        PollutantRecycler recycler = (PollutantRecycler)Params;

        //adds the amount in the players inventory as XP
        currentXP += 1 * Inventory.Instance.PollutantInventory[recycler.recyclerType];

        //if the player has enough xp to level
        if(currentXP >= nextLevelXPRequirement)
        {
            //level up
            LevelUp();
        }

    }

    private void LevelUp()
    {
        //increases the milestone number
        currentMilestone++;
        print("Leveled UP");

        //sets the XP amount to the levels requirement
        nextLevelXPRequirement = LevelRequirements[currentMilestone];

        //triggers the level up event for UI etc
        EventManager.Instance.PostEventNotification(EventManager.EVENT_TYPE.LEVEL_UP, this, currentMilestone);
    }
}
