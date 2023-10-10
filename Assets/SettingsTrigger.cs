using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SettingsTrigger : MonoBehaviour
{
    public MovementController movementController;
    
    private bool open = false;
    public UIAnchor anchor;
    private InputAction pausePressed;

    private void Start()
    {
        pausePressed = GameManager.Instance.InputHandler.input_asset.VRiskExperienceInputMap.Settings;
    }

    private void Update()
    {
        if (pausePressed.triggered)
        {
            if (!open)
            {
                anchor.PopIn();
                open = true;
            }
            else
            {
                anchor.PopOut();
                open = false;
            }
            
            movementController.enabled = !open;
        }
    }
    
    public void Close()
    {
        anchor.PopOut();
        open = false;
        movementController.enabled = true;
    }
}
