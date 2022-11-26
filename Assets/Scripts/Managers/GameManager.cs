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
    int currentBoat;
    [SerializeField] GameObject[] BoatSelection;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        currentBoat = 0;
        audioManager = GetComponent<AudioManager>();
        audioManager.Play("WaveAmbience");
        currentBoat = 0;
        SetBoat(currentBoat);

        EventManager.OnDelegateEvent GameStartBoatDelegate = GameStartBoat; 
        EventManager.OnDelegateEvent NewBoatDelegate = ChangeBoat;
        EventManager.OnDelegateEvent GameEndDelegate = GameEnd;
        EventManager.Instance.AddListener(EventManager.EVENT_TYPE.UPGRADE_BOAT, NewBoatDelegate);
        EventManager.Instance.AddListener(EventManager.EVENT_TYPE.GAME_START, GameStartBoatDelegate);
    }

    public void ChangeBoat(EventManager.EVENT_TYPE eventType, Component sender, object Params = null)
    {
        if (currentBoat < 2)
        {
            currentBoat++;
        }
        //sets prev boat to inactive
        BoatSelection[currentBoat - 1].SetActive(false);
        SetBoat(currentBoat);
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


    }

    public void GameStartBoat(EventManager.EVENT_TYPE eventType, Component sender, object Params = null)
    {
        SetBoat(currentBoat);
        
    }

    public void GameEnd(EventManager.EVENT_TYPE eventType, Component sender, object Params = null)
    {
        BoatSelection[currentBoat].SetActive(false);
    }
}
