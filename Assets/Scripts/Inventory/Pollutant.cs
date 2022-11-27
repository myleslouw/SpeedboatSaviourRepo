using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pollutant : MonoBehaviour
{
    public PollutantObject pollutantObj;        //the pollutant scriptable object
    public PollutantType.subType subType;
    
    void Start()
    {   //GW must be sideways at start others dont use this
        transform.Rotate(pollutantObj.startOffset);

    }
    private void Update()
    {
        //rotation of the object based on type
        transform.Rotate(pollutantObj.pollutantRotation);
    }

    public void PickUpAnimation()
    {
        StartCoroutine(ScaleOverTime(0.6f));
    }

    IEnumerator ScaleOverTime(float time)
    {

        //Stack overflow answer by dmg0600 https://answers.unity.com/questions/805199/how-do-i-scale-a-gameobject-over-time.html
        //accessed 26 Sept 2022

        Vector3 originalScale = transform.localScale;                       //starting size
        Vector3 destinationScale = new Vector3(0f, 0f, 0f);                 //final size (zero)

        float currentTime = 0.0f;

        do
        {
            //scales the game object down over time
            this.transform.localScale = Vector3.Lerp(originalScale, destinationScale, currentTime / time);
            currentTime += Time.deltaTime;
            yield return null;
        } while (currentTime <= time);

        Destroy(gameObject);
    }

}
