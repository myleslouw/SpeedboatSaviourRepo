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
    
    // Start is called before the first frame update
    void Start()
    {
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
            if (collisionObj.GetComponent<Pollutant>())
            {

                //Posts the event to all listeners of the POLLUTANT_PICKUP event and sends the pollutant for listeners to use
                EventManager.Instance.PostEventNotification(EventManager.EVENT_TYPE.POLLUTANT_PICKUP, this, collisionObj.GetComponent<Pollutant>());
                //play the pickup sound 
                audioManager.Play("Pickup");
                //plays the animation and destroys the obj
                collisionObj.GetComponent<Pollutant>().PickUpAnimation();
            }

            if (currentBoat.OilPickup)
            {
                if (other.gameObject.GetComponent<Hazard>())
                {
                    //now the player has the oilpickup it will collect the oil
                    EventManager.Instance.PostEventNotification(EventManager.EVENT_TYPE.OIL_PICKUP, this, null);
                }
            }

            //checks collision with recycler
            if (collisionObj.GetComponent<PollutantRecycler>())
            {
                //if the player collides with a recycler, it will trigger the recycle event
                EventManager.Instance.PostEventNotification(EventManager.EVENT_TYPE.RECYCLE_POLLUTANT, this, collisionObj.GetComponent<PollutantRecycler>());
            }

            //collision with NPC collider
            if (collisionObj.GetComponent<NPC>())
            {
                //if player collides with NPC collider it will trigger the NPC talk event 
                EventManager.Instance.PostEventNotification(EventManager.EVENT_TYPE.NPC_TALK, this, collisionObj.GetComponent<NPC>());
            }

            
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
        if (other.gameObject.GetComponent<NPC>())
        {
            //when the player leaves the NPC talking area
            EventManager.Instance.PostEventNotification(EventManager.EVENT_TYPE.NPC_LEAVE, this, this.transform);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //TRIGGER WHILE TOUCHING

        //Hazard - does damage to the current boat if it doesnt have the oil pickup
        //FuelRefill - refills the current boat
        if (!currentBoat.OilPickup)
        {
            if (other.gameObject.GetComponent<Hazard>())
            {
                //if the player touches a hazard, it will damage WHILE the player touches it
                currentBoat.TakeDamage();
            }
        }
        if (other.gameObject.GetComponent<FuelRefill>())
        {
            //if the player collides with a Fuel game object, it will refuel the current boat WHILE it touches the refill point
            EventManager.Instance.PostEventNotification(EventManager.EVENT_TYPE.REFUEL, this, other.gameObject.GetComponent<FuelRefill>().RefillAmount);
        }
    }
}
