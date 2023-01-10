using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//MASTERING UNITY SCRIPTING - ALAN THORN
//"Alternative with Delegates" pg 141
//The textbook shows a custom event manager

public class EventManager : MonoBehaviour
{
    //possible events that will take place in the game
    public enum EVENT_TYPE
    {
        GAME_START, GAME_END, GAME_PAUSE, POLLUTANT_PICKUP, OIL_PICKUP, ADD_XP, PICKUP_UI,
        RECYCLE_POLLUTANT, RECYCLE_UI, NPC_TALK, NPC_LEAVE, UPGRADE_BOAT, REFUEL, START_QUEST, COMPLETE_QUEST, BOAT_SHOPPING, SHOW_MILESTONE
    }

    public static EventManager Instance
    {
        get { return instance; }
        set { }
    }

    private static EventManager instance = null;

    public delegate void OnDelegateEvent(EVENT_TYPE eventType, Component sender, object Params = null);
    //public delegate void eventDelegate();

    //all objects registered for the event
    private Dictionary<EVENT_TYPE, List<OnDelegateEvent>> eventListeners = new Dictionary<EVENT_TYPE, List<OnDelegateEvent>>();

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

    private void Update()
    {
        if (instance == null)
        {
            print("null dude");
        }
    }
    //eventType = the event to listen for
    //listener = the objects to listen for the event
    //takes the listeners and puts the eventType on it
    //each event has a list of listeners, if there is a list for that eventType it will add, otherwise it will create one
    public void AddListener(EVENT_TYPE eventType, OnDelegateEvent listener)
    {
        //list of listeners for this event
        List<OnDelegateEvent> ListenList = null;


        //check existing event type list, if it exists, add to it
        if (eventListeners.TryGetValue(eventType, out ListenList))
        {
            //list exists so add the listener
            ListenList.Add(listener);
            return;
        }

        //otherwise create a new list as dictionary key for that event type
        ListenList = new List<OnDelegateEvent>();
        ListenList.Add(listener);
        eventListeners.Add(eventType, ListenList);
    }

    public void PostEventNotification(EVENT_TYPE eventType, Component sender, object Param = null)
    {
        //notifys all listeners of that event


        //list of event listeners
        List<OnDelegateEvent> ListenList = null;

        //if no event exists then exit
        if (!eventListeners.TryGetValue(eventType, out ListenList))
        {
            return;
        }

        //if exists, notify the listeners
        for (int i = 0; i < ListenList.Count; i++)
        {
            if (!ListenList[i].Equals(null))
            {
                ListenList[i](eventType, sender, Param);
            }
        }
    }

    public void RemoveEvent(EVENT_TYPE eventType)
    {
        //removes event from the dictionary
        eventListeners.Remove(eventType);
    }

    public void RemoveRedundancies()
    {
        //makes sure events are corrupted during scene changes

        //creates new dictionary
        Dictionary<EVENT_TYPE, List<OnDelegateEvent>> tempListeners = new Dictionary<EVENT_TYPE, List<OnDelegateEvent>>();

        //goes through each dictionary entry
        foreach (KeyValuePair<EVENT_TYPE, List<OnDelegateEvent>> Item in eventListeners)
        {
            //cycles all listeners and removes null objects
            for (int i = Item.Value.Count - 1; i >= 0; i--)
            {
                if (Item.Value[i].Equals(null))
                {
                    Item.Value.RemoveAt(i);
                }
            }

            //add remaining to temp dictionary
            if (Item.Value.Count > 0)
            {
                tempListeners.Add(Item.Key, Item.Value);
            }
        }

        //sets the modified dictionary to be the main one
        eventListeners = tempListeners;
    }

    private void OnLevelWasLoaded()
    {
        RemoveRedundancies();
    }
}
