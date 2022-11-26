using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaLifeRotate : MonoBehaviour
{
    public Transform Center;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(Center.position, Vector3.up, 10 * Time.deltaTime);
    }
}
