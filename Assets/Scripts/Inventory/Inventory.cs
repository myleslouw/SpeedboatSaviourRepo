using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    //key is the type and the value is the type amount
    public Dictionary<PollutantType.type, int> PollutantInventory;
    public int Credits, Oil;

 
    private void Start()
    {
        //creates the inventory dictionary
        CreateInventory();

        //creates the delegates and the methods to it
        EventManager.OnDelegateEvent AddPollutantDelegate = AddPollutantToInventory;
        EventManager.OnDelegateEvent AddOilDelegate = AddOilToInventory;
        EventManager.OnDelegateEvent RecyclePollutantDelegate = RecycleType;
        EventManager.OnDelegateEvent GameEndDelegate = GameEndInventory;
        EventManager.OnDelegateEvent BuyingDelegate = TryBuyBoat;

        //becomes a listener for the POLLUTANT_PICKUP event
        EventManager.Instance.AddListener(EventManager.EVENT_TYPE.POLLUTANT_PICKUP, AddPollutantDelegate);
        EventManager.Instance.AddListener(EventManager.EVENT_TYPE.OIL_PICKUP, AddOilDelegate);
        EventManager.Instance.AddListener(EventManager.EVENT_TYPE.RECYCLE_POLLUTANT, RecyclePollutantDelegate);
        EventManager.Instance.AddListener(EventManager.EVENT_TYPE.BOAT_SHOPPING, BuyingDelegate);
    }

    private void CreateInventory()
    {
        //create the inv dictionary
        PollutantInventory = new Dictionary<PollutantType.type, int>();

        //adds the types and starts them at 0
        PollutantInventory.Add(PollutantType.type.Glass, 0);
        PollutantInventory.Add(PollutantType.type.Plastic, 0);
        PollutantInventory.Add(PollutantType.type.GeneralWaste, 0);

        //sets currencies to 0
        Credits = 0;
        Oil = 0;

    }

    private void AddPollutantToInventory(EventManager.EVENT_TYPE eventType, Component sender, object Params = null)
    {
        //casts the obj recieved from the event to a pollutant and then increments the count in inv based on type
        if (Params != null)
        {
            Pollutant pickedUpPollutant = (Pollutant)Params;
            //increments the value of the type in the dictionary
            int count;
            count = PollutantInventory[pickedUpPollutant.pollutantObj.pollutantType];
            PollutantInventory[pickedUpPollutant.pollutantObj.pollutantType] = count + 1;

            //UI event becuase the UI would pudate before it was added to inv
            EventManager.Instance.PostEventNotification(EventManager.EVENT_TYPE.PICKUP_UI, this, pickedUpPollutant);
        }
    }

    private void AddCreditsToInventory(PollutantRecycler recycler)
    {
        //adds credits (typeReward x the amount of that type)
        Credits += (recycler.Reward * PollutantInventory[recycler.recyclerType]);
    }

    private void TryBuyBoat(EventManager.EVENT_TYPE eventType, Component sender, object Params = null)
    {
        //gets the price of the boat
        int boatPrice = (int)Params;

        if (Credits >= boatPrice)
        {
            //if the player has enough
            EventManager.Instance.PostEventNotification(EventManager.EVENT_TYPE.UPGRADE_BOAT, this, null);
        }
    }

    private void AddOilToInventory(EventManager.EVENT_TYPE eventType, Component sender, object Params = null)
    {
        //Adds oil to inventory
        Oil += 1;
    }

    private void RecycleType(EventManager.EVENT_TYPE eventType, Component sender, object Params = null)
    {
        //gets the type calls ADD xp event and then sets the inventory count of that type to 0
        //gets the event obj data

        if (Params != null)
        {
            PollutantRecycler recycler = (PollutantRecycler)Params;

            if (PollutantInventory[recycler.recyclerType] != 0)
            {
                //if the inventory isnt empty it should play a sound when recycling
                GetComponent<AudioManager>().Play("Recycle");
            }

            //ADD CREDITS TO THE INVENTORY
            AddCreditsToInventory(recycler);

            //sets the types amount in inv to 0
            PollutantInventory[recycler.recyclerType] = 0;

            //it then updaes the UI to show the 0 in invetory
            EventManager.Instance.PostEventNotification(EventManager.EVENT_TYPE.RECYCLE_UI, this, recycler);
        }
    }

    private void GameEndInventory(EventManager.EVENT_TYPE eventType, Component sender, object Params = null)
    {
        //sets inv to 0
        PollutantInventory[PollutantType.type.Glass] = 0;
        PollutantInventory[PollutantType.type.Plastic] = 0;
        PollutantInventory[PollutantType.type.GeneralWaste] = 0;
    }

}
