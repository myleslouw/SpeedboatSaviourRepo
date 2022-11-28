using System.Collections;
using System.Collections.Generic;
using Polyperfect.Animals;
using Polyperfect.Common;
using UnityEngine;


//HANDLES PLAYER COLLISION
public class PlayerScript : MonoBehaviour
{
    private AudioManager audioManager;
    public SoundObj impactSoundObj, recycleSoundObj, pickupObj;
    private Boat currentBoat;
    bool colliding;
    //used for doing damage every x seconds
    bool timerOn = false;
    float curTime = 0.0f;
    float timeNeeded = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        currentBoat = GetComponent<Boat>();
        EventManager.OnDelegateEvent BoatDetailsDelegate = GetBoatDetails;
        EventManager.Instance.AddListener(EventManager.EVENT_TYPE.UPGRADE_BOAT, BoatDetailsDelegate);

        //gets the audio manager
        audioManager = GetComponent<BoatController>().audioManager;

        //adding the impact and recycle sounds to the list
        impactSoundObj.source = transform.GetChild(3).GetComponent<AudioSource>();
        audioManager.AddSoundToList(impactSoundObj);

        recycleSoundObj.source = transform.GetChild(3).GetComponent<AudioSource>();
        audioManager.AddSoundToList(recycleSoundObj);

        //the audio source is on the "TrashCollected" GO
        pickupObj.source = transform.GetChild(3).GetComponent<AudioSource>();
        //adding pickup sound to the list
        audioManager.AddSoundToList(pickupObj);
    }

    public void GetBoatDetails(EventManager.EVENT_TYPE eventType, Component sender, object Params = null)
    {
        //gets the boat number on upgrade
        currentBoat = GetComponentInParent<GameManager>().currentBoat;
    }

    //TRIGGER ONCE
    private void OnTriggerEnter(Collider other)
    {
        if (other != null)
        {
            //stores the gameobject it collides with
            GameObject collisionObj = other.gameObject;

            //checks collision with pollutant
            if (collisionObj.gameObject.CompareTag("Pollutant"))
            {
                //Posts the event to all listeners of the POLLUTANT_PICKUP event and sends the pollutant for listeners to use
                EventManager.Instance.PostEventNotification(EventManager.EVENT_TYPE.POLLUTANT_PICKUP, this, collisionObj.GetComponent<Pollutant>());
                //play the pickup sound 
                audioManager.Play("Pickup");
                //plays the animation and destroys the obj
                collisionObj.GetComponent<Pollutant>().PickUpAnimation();
            }

            //collision with hazard has 2 different outcomes
            if (collisionObj.gameObject.CompareTag("Hazard"))
            {
                //if it has the oil pickup upgrade
                if (currentBoat.OilPickup)
                {
                    //now the player has the oilpickup it will collect the oil
                    EventManager.Instance.PostEventNotification(EventManager.EVENT_TYPE.OIL_PICKUP, this, null);
                }
                else
                {
                    //if it doesnt have the upgrade
                    //damage the boat every second
                    InvokeRepeating("DamageBoat", 0.0f, 1.0f);
                }
            }


            //checks collision with recycler
            if (collisionObj.gameObject.CompareTag("PollutantRecycler"))
            {
                //if the player collides with a recycler, it will trigger the recycle event
                EventManager.Instance.PostEventNotification(EventManager.EVENT_TYPE.RECYCLE_POLLUTANT, this, collisionObj.GetComponent<PollutantRecycler>());
            }

            //collision with NPC collider
            if (collisionObj.CompareTag("NPC"))
            {

                //if player collides with NPC collider it will trigger the NPC talk event 
                EventManager.Instance.PostEventNotification(EventManager.EVENT_TYPE.NPC_TALK, this, collisionObj.GetComponent<NPC>());

                //switch NPC to talking animation
                NPCAnim.SwitchState(NPCState.TALKING);
            }

            if (collisionObj.CompareTag("FuelRefill"))
            {
                InvokeRepeating("FuelBoat", 0.0f, 0.5f);
            }

            if (collisionObj.CompareTag("BuyingStation"))
            {
                print("Shopping...");
                //triggers the shopping method to check if there is enough to buy
                EventManager.Instance.PostEventNotification(EventManager.EVENT_TYPE.BOAT_SHOPPING, this, collisionObj.GetComponent<BuyingStation>().BoatPrice);
            }

        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Minus))
        {
            DamageBoat();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //otherwise it has collided with something solid and must make a sound
        //based off the speed it hits, it will change the pitch
        //sets the pitch to the speed of the impact
        //collision.relativeVelocity.magnitude / 3
        impactSoundObj.volume = collision.relativeVelocity.magnitude / 3;
        audioManager.Play("Impact");

        if (collision.gameObject.CompareTag("Animal"))
        {

            Animal_WanderScript wanderScript = collision.gameObject.GetComponent<Animal_WanderScript>();

            Debug.Log("Player bumped into " + collision.gameObject.name);

            //wanderScript.TakeDamage(1000);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("NPC"))
        {
            //when the player leaves the NPC talking area
            EventManager.Instance.PostEventNotification(EventManager.EVENT_TYPE.NPC_LEAVE, this, this.transform);

            //switch NPC to IDLE animation
            NPCAnim.SwitchState(NPCState.IDLE);

            //if the NPC has a quest it will start
            if (other.gameObject.TryGetComponent(out QuestGiver questGiver))
            {
                //if the quest hasnt been done
                if (!questGiver.completed)
                {
                    //trigger sthe event
                    EventManager.Instance.PostEventNotification(EventManager.EVENT_TYPE.START_QUEST, this, questGiver);
                }

            }
        }

        //stop damaging boat
        if (other.gameObject.CompareTag("Hazard"))
        {
            CancelInvoke("DamageBoat");
        }
        //stop refueling
        if (other.gameObject.CompareTag("FuelRefill"))
        {
            CancelInvoke("FuelBoat");
        }
    }

    private void DamageBoat()
    {
        print("Damageing boat");
        currentBoat.TakeDamage();
    }

    private void FuelBoat()
    {
        currentBoat.RefuelBoat();
    }
}
