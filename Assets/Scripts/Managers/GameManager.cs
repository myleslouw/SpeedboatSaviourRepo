using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    public static int Level = 0;
    AudioManager audioManager;
    public Slider DurabilitySlider;
    public Slider FuelSlider;
    public int currentBoatIndex;
    [SerializeField] GameObject[] BoatSelection;
    [SerializeField] GameObject[] DisplayBoats;
    [SerializeField] GameObject[] LockedIcons;
    [SerializeField] GameObject[] UnlockedIcons;

    public Boat currentBoat;
    public bool OilPickupObtained;

    public Transform PlayerRespawnPoint;     //the 2 places the player respawns after death/out of fuel  (starting spot, pete)

    public bool ActiveQuest = false;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        currentBoatIndex = 0;
        audioManager = GetComponent<AudioManager>();
        audioManager.Play("WaveAmbience");
        currentBoatIndex = 0;
        SetBoat(currentBoatIndex);

        EventManager.OnDelegateEvent GameStartBoatDelegate = GameStartBoat;
        EventManager.OnDelegateEvent NewBoatDelegate = ChangeBoat;
        EventManager.OnDelegateEvent GameEndDelegate = GameEnd;
        EventManager.Instance.AddListener(EventManager.EVENT_TYPE.UPGRADE_BOAT, NewBoatDelegate);
        EventManager.Instance.AddListener(EventManager.EVENT_TYPE.GAME_START, GameStartBoatDelegate);
        EventManager.Instance.AddListener(EventManager.EVENT_TYPE.GAME_END, GameEndDelegate);
        LockedIcons[0].SetActive(true);
        LockedIcons[1].SetActive(true);
        UnlockedIcons[0].SetActive(false);
        UnlockedIcons[1].SetActive(false);
        
    }

    public void ChangeBoat(EventManager.EVENT_TYPE eventType, Component sender, object Params = null)
    {
        if (currentBoatIndex < 2)
        {
            currentBoatIndex++;
        }
        //sets prev boat to inactive
        BoatSelection[currentBoatIndex - 1].SetActive(false);
        //hides the display version
        DisplayBoats[0].SetActive(false);
        LockedIcons[currentBoatIndex - 1].SetActive(false);
        UnlockedIcons[currentBoatIndex - 1].SetActive(true);

        SetBoat(currentBoatIndex);
    }

    private void SetBoat(int currentBoatIndex)
    {
        //gets durability slider from UI manager
        DurabilitySlider = GetComponent<UIManager>().durabiltySlider;
        //gets Fuel slider from UI manager
        FuelSlider = GetComponent<UIManager>().fuelSlider;
        //sets new boat to active
        BoatSelection[currentBoatIndex].SetActive(true);
        //sets the boats audio manager to the main audio manager
        BoatSelection[currentBoatIndex].GetComponent<BoatController>().audioManager = audioManager;
        //sets the durability slider to show the new boats durability
        BoatSelection[currentBoatIndex].GetComponent<Boat>().durabiltySlider = DurabilitySlider;
        //sets the Fuel slider to show the new boats Fuel
        BoatSelection[currentBoatIndex].GetComponent<Boat>().fuelSlider = FuelSlider;

        //sets the current boat to the active boat
        currentBoat = BoatSelection[currentBoatIndex].GetComponent<Boat>();
        //if the oilpickup is obtained, it will be active
        currentBoat.OilPickup = OilPickupObtained;

    }

    public void GameStartBoat(EventManager.EVENT_TYPE eventType, Component sender, object Params = null)
    {
        SetBoat(currentBoatIndex);

    }

    public void GameEnd(EventManager.EVENT_TYPE eventType, Component sender, object Params = null)
    {
        //sets the boats position to pete dock
        currentBoat.gameObject.transform.position = PlayerRespawnPoint.position;
        currentBoat.gameObject.transform.rotation = PlayerRespawnPoint.rotation;


    }
}
