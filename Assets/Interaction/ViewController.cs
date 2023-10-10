using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class ViewController : MonoBehaviour
{
    private InputAction view_action;
    public GameData data;

    // Don't need if we remove view rotation by controller input on X axis.
    /*
    public Transform cam_trans;
    public Transform rotation_parent_trans;
    */

    private void Start()
    {
        view_action = GameManager.Instance.InputHandler.input_asset.VRiskExperienceInputMap.RotateView;
    }

    private void FixedUpdate()
    {
        Vector2 input_vect = view_action.ReadValue<Vector2>();
        
        // Killed because it was way too dizzying.
        /*
        if (input_vect.y != 0)
        {
        Quaternion cam_y_rotation = Quaternion.Euler(0, cam_trans.eulerAngles.y, 0);
        Vector3 rotated_x_axis = cam_y_rotation * Vector3.right;

        rotation_parent_trans.RotateAround(cam_trans.position, rotated_x_axis, input_vect.y * rotate_speed * -1); 
        }
        */

        if (input_vect.x != 0)
        {
           transform.Rotate(0f, input_vect.x * data.turnSpeed, 0f, Space.World);
        }
    }
}
