using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelsFinished : MonoBehaviour
{
    public GameObject[] finishedLevels;
    private int currentLevel = 0;
    
    private void OnEnable()
    {
        QuestManager.OnLevelActive += ActivateLevel;
    }

    private void OnDisable()
    {
        QuestManager.OnLevelActive -= ActivateLevel;
    }

    private void Start()
    {
        for (int i = 0; i < finishedLevels.Length; i++)
        {
            finishedLevels[i].SetActive(false);
        }
    }

    private void ActivateLevel()
    {
        finishedLevels[currentLevel].SetActive(true);
        currentLevel++;
    }
}
