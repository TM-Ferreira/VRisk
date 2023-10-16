using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogoAnchorTextSelector : MonoBehaviour
{
    public GameData data;
    public GameObject successLogo;
    public GameObject successText;
    public GameObject failLogo;
    public GameObject failText;
    
    void Start()
    {
        if (data.survived)
        {
            successLogo.SetActive(true);   
            successText.SetActive(true);
        }
        else
        {
            failLogo.SetActive(true);
            failText.SetActive(true);
        }
    }
}
