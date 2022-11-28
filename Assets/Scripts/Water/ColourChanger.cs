using System.Collections;
using System.Collections.Generic;
using Polyperfect.Animals;
using UnityEngine;

public class ColourChanger : MonoBehaviour
{
    public MeshRenderer Mesh;
    Color ShallowStartColour = new Color(0.081f, 0.281f, 0.459f, 0.247f);
    Color DeepStartColour = new Color(0.006f, 0.236f, 0.021f, 0.537f);

    Color Shallow2ndColour = new Color(0.081f, 0.281f, 0.459f, 0.247f);
    Color Deep2ndColour = new Color(0.013f, 0.387f, 0.032f, 0.537f);

    Color Shallow3rdColour = new Color(0.081f, 0.281f, 0.459f, 0.247f);
    Color Deep3rdColour = new Color(0.177f, 0.462f, 0.352f, 0.537f);

    Color Shallow4thColour = new Color(0.081f, 0.281f, 0.459f, 0.247f);
    Color Deep4thColour = new Color(0.033f, 0.408f, 0.472f, 0.537f);

    Color ShallowEndColour = new Color(0.118f, 0.925f, 0.840f, 0.808f);
    Color DeepEndColour = new Color(0.027f, 0.564f, 0.811f, 0.537f);

    private Animal_WanderScript wanderScript;

    // Start is called before the first frame update
    void Start()
    {
        //listens for level up to change the water
        EventManager.OnDelegateEvent WaterChangeDelegate = ChangeWater;
        EventManager.Instance.AddListener(EventManager.EVENT_TYPE.COMPLETE_QUEST, WaterChangeDelegate);
        Mesh.material.SetColor("ShallowWater", ShallowStartColour);
        Mesh.material.SetColor("DeepWater", DeepStartColour);
        
        
    }

    // Update is called once per frame
    void Update()
    {

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
                Mesh.material.SetColor("ShallowWater", Shallow2ndColour);
                Mesh.material.SetColor("DeepWater", Deep2ndColour);
                break;
            case 2:
                Mesh.material.SetColor("ShallowWater", Shallow3rdColour);
                Mesh.material.SetColor("DeepWater", Deep3rdColour);
                break;
            case 4:
                Mesh.material.SetColor("ShallowWater", Shallow4thColour);
                Mesh.material.SetColor("DeepWater", Deep4thColour);
                break;
            default:
                Mesh.material.SetColor("ShallowWater", ShallowEndColour);
                Mesh.material.SetColor("DeepWater", DeepEndColour);
                break;
        }
       
    }

}
