using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PollutantRecycler : MonoBehaviour
{
    public PollutantType.type recyclerType;
    public int Reward;

    public PollutantRecycler()
    {
        switch (recyclerType)
        {
            case PollutantType.type.Glass:
                Reward = 10;
                break;
            case PollutantType.type.Plastic:
                Reward = 25;
                break;
            case PollutantType.type.GeneralWaste:
                Reward = 50;
                break;
            default:
                break;
        }
    }
}
