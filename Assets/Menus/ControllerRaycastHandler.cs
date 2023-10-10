using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class ControllerRaycastHandler : MonoBehaviour
{
    public XRBaseController left_controller;
    public XRBaseController right_controller;

    private XRInteractorLineVisual left_cast;
    private XRInteractorLineVisual right_cast;
    private XRRayInteractor left_ray;
    private XRRayInteractor right_ray;

    private InputAction trigger_left_pressed;
    private InputAction trigger_left_touched;
    
    private InputAction trigger_right_pressed;
    private InputAction trigger_right_touched;

    [SerializeField] private bool start_deactivated = false;

    private enum CastSide
    {
        Left,
        Right,
        None
    }

    private CastSide current = CastSide.None;
    private CastSide previous = CastSide.None;
    
    void Start()
    {
        left_cast = left_controller.GetComponent<XRInteractorLineVisual>();
        right_cast = right_controller.GetComponent<XRInteractorLineVisual>();
        left_ray = left_controller.GetComponent<XRRayInteractor>();
        right_ray = right_controller.GetComponent<XRRayInteractor>();
        
        trigger_left_pressed = GameManager.Instance.InputHandler.input_asset.VRiskExperienceInputMap.InteractLeft_Hand;
        trigger_left_touched = GameManager.Instance.InputHandler.input_asset.VRiskExperienceInputMap.TouchLeft_Hand;
        trigger_right_pressed = GameManager.Instance.InputHandler.input_asset.VRiskExperienceInputMap.InteractRight_Hand;
        trigger_right_touched = GameManager.Instance.InputHandler.input_asset.VRiskExperienceInputMap.TouchRight_Hand;

        if (start_deactivated) ChangeState(false);
    }

    void Update()
    {
        //Uses touch and press to cast a line if the player is using the left controller to point
        if (current != CastSide.Left)
        {
            if (trigger_left_touched.triggered || trigger_left_pressed.triggered)
            {
                current = CastSide.Left;
            }
        }

        //Same but with the right controller instead
        if (current != CastSide.Right)
        {
            if (trigger_right_touched.triggered || trigger_right_pressed.triggered)
            {
                current = CastSide.Right;
            }
        }

        //Sets the correct line to be casted in a menu
        if (current != previous)
        {
            switch (current)
            {
                case CastSide.Left:
                    LeftState(true);
                    RightState(false);
                    break;

                case CastSide.Right:
                    LeftState(false);
                    RightState(true);
                    break;

                case CastSide.None:
                    LeftState(false);
                    RightState(false);
                    break;
            }

            previous = current;
        }
    }

    private void LeftState(bool active)
    {
        left_cast.enabled = active;
        left_ray.enabled = active;
    }
    
    private void RightState(bool active)
    {
        right_cast.enabled = active;
        right_ray.enabled = active;
    }

    public void ChangeState(bool state)
    {
        enabled = state;
        RightState(state);
        LeftState(state);
    }
}
