using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class BoatController : MonoBehaviour
{
    Rigidbody rb;
    public Transform Director;
    public float speed = 3;        //speed for game, 
    public AudioManager audioManager;
    public SoundObj soundObj, milestoneCloseSound;
    bool moving;
    public GameObject propeller;
    public Boat Boat;
    private float coolDown, coolDownDuration;
    private float boostDuration = 2;


    //OLD BOAT HEIGHT WAS 3.9 for row boat
    //3.689 for sail
    //3.652 for speedboat

    //NEW BOAT HEIGHT 0.7
   
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Boat = GetComponent<Boat>();
        //sets the source of the sound to this game object
        soundObj.source = GetComponent<AudioSource>();
        //adds the sound to the list of sounds
        audioManager.AddSoundToList(soundObj);
        audioManager.AddSoundToList(milestoneCloseSound);

        coolDownDuration = 4;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        coolDown -= Time.deltaTime;


        if (!Input.anyKey)
        {
            //IF THERE IS NO MOVEMENT
            moving = false;
        }

        //adding force to the rigidbody at position in the front of the gameobject to give it the right feel

        if (Input.GetKey(KeyCode.W))
        {
            //move forward
            MoveBoat(-transform.up);
        }

        if (Input.GetKey(KeyCode.S))
        {
            //move back
            MoveBoat(transform.up);
        }

        if (Input.GetKey(KeyCode.A))
        {
            //turn left
            TurnBoat(-transform.right);
        }

        if (Input.GetKey(KeyCode.D))
        {
            //turn right
            TurnBoat(transform.right);
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            //speed boost?
            if (coolDown <= 0)
            {
                StartCoroutine(SpeedBoost());
            }
        }

        //boat sounds
        if (moving && !audioManager.getSoundStatus("BoatSound"))
        {
            //if the boat is moving and the sound isnt playing, it will play the sound
            audioManager.Play("BoatSound");
        }

        if (!moving && audioManager.getSoundStatus("BoatSound"))
        {
            //if the boat isnt moving and a sound is playing, it will stop
            audioManager.StopPlaying("BoatSound");
        }

        if (moving)
        {
            //if the boat is moving, it will consume fuel per second
            Boat.UseFuel();
        }

        //closing milestone manager with ENTER
        if (Input.GetKey(KeyCode.Return))
        {
            //closing the milestone
            UIManager uiManager = GetComponentInParent<UIManager>();
            if (uiManager.Milestone.active)
            {
                uiManager.Milestone.SetActive(false);
                //playing the milestone closing sound
                GetComponentInParent<AudioManager>().Play("CloseMilestone");
                StartCoroutine(WaitTillPlay());
            }
        }
    }
    IEnumerator WaitTillPlay()
    {
        yield return new WaitForSeconds(0.5f);
        audioManager.Play("WaveAmbience");
    }

    private void MoveBoat(Vector3 direction)
    {
        //Moving the boat
        moving = true;

        //add force to the boats director transform
        rb.AddForceAtPosition(direction * speed, Director.position, ForceMode.Force);


        //spin the propeller if it has one
        if (propeller != null)
        {
            propeller.transform.Rotate(new Vector3(0, 30, 0));
        }
    }

    private void TurnBoat(Vector3 direction)
    {

        //same as the moving but it is half the speed for turning
        moving = true;

        rb.AddForceAtPosition(direction * speed / 2, Director.position, ForceMode.Force);

        //spin the propeller if it has one
        if (propeller != null)
        {
            propeller.transform.Rotate(new Vector3(0, 30, 0));
        }
    }

    private IEnumerator SpeedBoost()
    {
        speed = 6;

        print("speedBoost");

        yield return new WaitForSeconds(boostDuration);

        coolDown = coolDownDuration;

        speed = 3;
    }
}
