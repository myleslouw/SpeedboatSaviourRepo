using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;

public class UIManager : MonoBehaviour
{
    [SerializeField] Text glassCounter, plasticCounter, generalWasteCounter;
    [SerializeField] Text QuestglassCounter, QuestplasticCounter, QuestgeneralWasteCounter;
    private Dictionary<PollutantType.type, Text> TypeCounters;
    public GameObject Milestone;
    AudioManager audioManager;
    public Slider durabiltySlider;
    public Slider fuelSlider;
    [SerializeField] GameObject InventoryUI;
    [SerializeField] GameObject LevelUI;
    [SerializeField] GameObject questBox;
    [SerializeField] GameObject NewMilestoneNotification;

    // Start is called before the first frame update

    private void Start()
    {
        audioManager = GetComponent<AudioManager>();
        Milestone.SetActive(false);
        //sets lvl num

        //puts the counters in the dictionary
        CreateCounters();

        //the listener for the pickup event
        EventManager.OnDelegateEvent IncrementDelegate = IncrementPollutantUI;
        EventManager.OnDelegateEvent ResetDelegate = ResetPollutantUI;
        EventManager.OnDelegateEvent GameStartDelegate = GameStartUI;
        EventManager.OnDelegateEvent GameOverDelegate = GameOverUI;
        EventManager.OnDelegateEvent ShowQuestDelegate = ShowQuest;
        EventManager.OnDelegateEvent CompleteQuestDelegate = CompleteQuestUI;
        EventManager.Instance.AddListener(EventManager.EVENT_TYPE.PICKUP_UI, IncrementDelegate);
        EventManager.Instance.AddListener(EventManager.EVENT_TYPE.RECYCLE_UI, ResetDelegate);
        EventManager.Instance.AddListener(EventManager.EVENT_TYPE.GAME_START, GameStartDelegate);
        EventManager.Instance.AddListener(EventManager.EVENT_TYPE.GAME_END, GameOverDelegate);
        EventManager.Instance.AddListener(EventManager.EVENT_TYPE.START_QUEST, ShowQuestDelegate);
        EventManager.Instance.AddListener(EventManager.EVENT_TYPE.COMPLETE_QUEST, CompleteQuestDelegate);

        questBox.SetActive(false);
        NewMilestoneNotification.SetActive(false);

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

    private void CompleteQuestUI(EventManager.EVENT_TYPE eventType, Component sender, object Params = null)
    {
        //change the UI to show new level

        questBox.SetActive(false);
        //play sound
        audioManager.Play("MilestoneSound");

        //show Milestone notification
        NewMilestoneNotification.SetActive(true);

        //waits a bit and then plays the waves
        //PlayWavesAfterMilestone();
        StartCoroutine(WaitTillPlay());
    }

    IEnumerator WaitTillPlay()
    {
        yield return new WaitForSeconds(0.5f);
        audioManager.Play("WaveAmbience");
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

    public void ShowQuest(EventManager.EVENT_TYPE eventType, Component sender, object Params = null)
    {
        //get quiest info from event
        QuestGiver questGiver = (QuestGiver)Params;

        questBox.SetActive(true);
        //set the counters to the amount need for the quest
        QuestglassCounter.text = questGiver.questObj.GlassRequirement.ToString();
        QuestplasticCounter.text = questGiver.questObj.PlasticRequirement.ToString();
        QuestgeneralWasteCounter.text = questGiver.questObj.GWRequirement.ToString();
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
