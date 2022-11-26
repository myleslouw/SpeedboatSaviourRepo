using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] Text glassCounter, plasticCounter, generalWasteCounter;
    private Dictionary<PollutantType.type, Text> TypeCounters;
    [SerializeField] TextMeshProUGUI levelNum;
    public GameObject Milestone;
    AudioManager audioManager;
    public Slider durabiltySlider;
    public Slider fuelSlider;
    [SerializeField] GameObject InventoryUI;
    [SerializeField] GameObject LevelUI;

    // Start is called before the first frame update

    private void Start()
    {
        audioManager = GetComponent<AudioManager>();
        Milestone.SetActive(false);
        //sets lvl num
        levelNum.text = MilestoneManager.Instance.currentMilestone.ToString();

        //puts the counters in the dictionary
        CreateCounters();

        //the listener for the pickup event
        EventManager.OnDelegateEvent IncrementDelegate = IncrementPollutantUI;
        EventManager.OnDelegateEvent ResetDelegate = ResetPollutantUI;
        EventManager.OnDelegateEvent LevelUpDelegate = LevelUpUI;
        EventManager.OnDelegateEvent GameStartDelegate = GameStartUI;
        EventManager.OnDelegateEvent GameOverDelegate = GameOverUI;
        EventManager.Instance.AddListener(EventManager.EVENT_TYPE.PICKUP_UI, IncrementDelegate);
        EventManager.Instance.AddListener(EventManager.EVENT_TYPE.RECYCLE_UI, ResetDelegate);
        EventManager.Instance.AddListener(EventManager.EVENT_TYPE.LEVEL_UP, LevelUpDelegate);
        EventManager.Instance.AddListener(EventManager.EVENT_TYPE.GAME_START, GameStartDelegate);
        EventManager.Instance.AddListener(EventManager.EVENT_TYPE.GAME_END, GameOverDelegate);
    }

    private void CreateCounters()
    {
        //create the inv dictionary
        TypeCounters = new Dictionary<PollutantType.type, Text>();

        //adds the types and corresponding counter UI component
        TypeCounters.Add(PollutantType.type.Glass, glassCounter);
        TypeCounters.Add(PollutantType.type.Plastic, plasticCounter);
        TypeCounters.Add(PollutantType.type.GeneralWaste, generalWasteCounter);

    }
    private void IncrementPollutantUI(EventManager.EVENT_TYPE eventType, Component sender, object Params = null)
    {
        //this is be exectured on event trigger,   there a few components it can access ^^
        //gets the pollutant object to see what type to increment

        Pollutant pickedUpObj = (Pollutant)Params;       //gets the object sent and casts it so we can use it

        //runs the method that disaplys the current inv
        UpdatePollutantCount(pickedUpObj.pollutantObj.pollutantType);
    }

    private void ResetPollutantUI(EventManager.EVENT_TYPE eventType, Component sender, object Params = null)
    {
        //upon recycling the inv amount for a type will be 0 so it will be updated
        PollutantRecycler recycler = (PollutantRecycler)Params;
        UpdatePollutantCount(recycler.recyclerType);
    }

    private void UpdatePollutantCount(PollutantType.type polObjType)
    {
        //gets the UI components based on the type and then displays the types inventory count

        TypeCounters[polObjType].text = Inventory.Instance.PollutantInventory[polObjType].ToString();
    }

    private void LevelUpUI(EventManager.EVENT_TYPE eventType, Component sender, object Params = null)
    {
        //change the UI to show new level

        //set lvl num to current milstone
        levelNum.text = MilestoneManager.Instance.currentMilestone.ToString();

        //play sound
        audioManager.Play("MilestoneSound");

        //show Milestone
        OpenMilestoneUI();
    }

    public void OpenMilestoneUI()
    {
        Milestone.SetActive(true);
    }
    public void CloseMilestoneUI()
    {
        Milestone.SetActive(false);

        audioManager.Play("WaveAmbience");
    }

    private void GameStartUI(EventManager.EVENT_TYPE eventType, Component sender, object Params = null)
    {
        //turns on some UI components  on restart so they show in the  scene
        durabiltySlider.gameObject.SetActive(true);
        fuelSlider.gameObject.SetActive(true);
        LevelUI.SetActive(true);
        InventoryUI.SetActive(true);

    }

    private void GameOverUI(EventManager.EVENT_TYPE eventType, Component sender, object Params = null)
    {
        //turns off some UI components after death so they dont show in the next scene
        durabiltySlider.gameObject.SetActive(false);
        fuelSlider.gameObject.SetActive(false);
        LevelUI.SetActive(false);
        InventoryUI.SetActive(false);

    }
}
