using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PresetManager : MonoBehaviour
{
    public GameData data;
    public Button joystickBT;
    public Button gestureBT;

    private void Start()
    {
        Refresh();
    }

    public void JoystickSelected()
    {
        joystickBT.interactable = false;
        gestureBT.interactable = true;
        data.movementType = GameData.MovementType.JOYSTICK;
    }

    public void GestureSelected()
    {
        joystickBT.interactable = true;
        gestureBT.interactable = false;
        data.movementType = GameData.MovementType.GESTURE;
    }

    public void Refresh()
    {
        if (data.movementType == GameData.MovementType.GESTURE)
        {
            GestureSelected();
        }
        else
        {
            JoystickSelected();
        }
    }
}
