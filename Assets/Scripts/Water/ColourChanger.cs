using System.Collections;
using System.Collections.Generic;
using Polyperfect.Animals;
using UnityEngine;

public class ColourChanger : MonoBehaviour
{
    public MeshRenderer Mesh;
    Color ShallowStartColour = new Color(0.081f, 0.281f, 0.459f, 0.247f);
    Color DeepStartColour = new Color(0.000f, 0.533f, 0.035f, 0.537f);

    Color ShallowMidColour = new Color(0.081f, 0.281f, 0.459f, 0.247f);
    Color DeepMidColour = new Color(0.000f, 0.533f, 0.401f, 0.537f);

    Color ShallowEndColour = new Color(0.118f, 0.925f, 0.840f, 0.808f);
    Color DeepEndColour = new Color(0.087f, 0.452f, 0.594f, 0.694f);

    private Animal_WanderScript wanderScript;

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        //listens for level up to change the water
        EventManager.OnDelegateEvent WaterChangeDelegate = ChangeWater;
        EventManager.Instance.AddListener(EventManager.EVENT_TYPE.LEVEL_UP, WaterChangeDelegate);
        Mesh.material.SetColor("ShallowWater", ShallowStartColour);
        Mesh.material.SetColor("DeepWater", DeepStartColour);
        
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Mesh.material.SetColor("ShallowWater", ShallowEndColour);
            Mesh.material.SetColor("DeepWater", DeepEndColour);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            print("Shallow: " + Mesh.material.GetColor("ShallowWater"));
            print("Deep: " + Mesh.material.GetColor("DeepWater"));
        }
    }

    private void ChangeWater(EventManager.EVENT_TYPE eventType, Component sender, object Params = null)
    {
        //gets the current milestone
        int milestone = (int)Params;

        //changes the water colour depending on the milestone 
        switch (milestone)
        {
            case 0:
                Mesh.material.SetColor("ShallowWater", ShallowStartColour);
                Mesh.material.SetColor("DeepWater", DeepStartColour);
                break;
            case 1:
                Mesh.material.SetColor("ShallowWater", ShallowMidColour);
                Mesh.material.SetColor("DeepWater", DeepMidColour);
                break;
            default:
                Mesh.material.SetColor("ShallowWater", ShallowEndColour);
                Mesh.material.SetColor("DeepWater", DeepEndColour);
                break;
        }
       
    }

}
