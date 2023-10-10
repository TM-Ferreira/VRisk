using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebrisSelector : MonoBehaviour
{
    public enum DebrisLevel : int
    {
        NONE,
        LOW,
        MID,
        HIGH
    }
    
    public GameObject debrisG1;
    public GameObject debrisG2;
    public GameObject debrisG3;

    public DebrisLevel maxDebrisLevel = DebrisLevel.HIGH;
    public DebrisLevel currentDebrisLevel = DebrisLevel.NONE;

    public void IncreaseDebrisAmount()
    {
        if (currentDebrisLevel == maxDebrisLevel || currentDebrisLevel == DebrisLevel.HIGH) return;
        
        currentDebrisLevel++;
        SetDebrisLevel();
    }

    public void SetDebrisLevel()
    {
        DeactivateAll();
        
        switch (currentDebrisLevel)
        {
            case DebrisLevel.NONE:
                break;
            case DebrisLevel.LOW:
                debrisG1.SetActive(true);
                break;
            case DebrisLevel.MID:
                debrisG2.SetActive(true);
                break;
            case DebrisLevel.HIGH:
                debrisG3.SetActive(true);
                break;
        }
    }

    private void DeactivateAll()
    {
        debrisG1.SetActive(false);
        debrisG2.SetActive(false);
        debrisG3.SetActive(false);
    }
}
