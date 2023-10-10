using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class UIAnchor : MonoBehaviour
{
    public GameObject cameraGO;
    public float animation_time = 0.8f;

    [SerializeField] private bool start_unactive = false;
    [SerializeField] private bool apply_offset = false;
    [SerializeField] private float Inital_offset = 7000f;

    [SerializeField] private float view_angle = 45.0f;
    [SerializeField] private float walk_area = 0.5f;
    [SerializeField] private float lerp_speed_rot = 5.0f;
    [SerializeField] private float lerp_speed_pos = 3.0f;
    [SerializeField] private float threshold = 0.05f;
    
    private bool reset_angle = true;
    private bool reset_pos = true;

    private Vector3 default_scale;
    private Vector3 default_pos;
    private Vector3 anchor_pos;

    private void Awake()
    {
        default_pos = transform.position;
        default_scale = transform.localScale;
    }

    private void Start()
    {
        if (apply_offset)
        {
            //Doing so makes the menu fall from the sky, its just something easy that adds a nice touch
            transform.position = new Vector3(default_pos.x, default_pos.y + Inital_offset, default_pos.z);
        }

        if (start_unactive)
        {
            transform.localScale = new Vector3(0, 0, 1);
            gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        //Finds out where the anchor should be located based on camera posiion
        Vector3 camera_pos = cameraGO.transform.position;
        anchor_pos = new Vector3(camera_pos.x, default_pos.y + camera_pos.y, camera_pos.z);
        
        //If the player has gotten fairly away from the menu, it gets re-moved in range of the player
        if (reset_pos)
        {
            if (Vector3.Distance(transform.position, anchor_pos) >= threshold)
            {
                transform.position = Vector3.Lerp(transform.position, anchor_pos, lerp_speed_pos * Time.deltaTime);
            }
            else
            {
                reset_pos = false;
            }
        }
        else
        { 
            //Checks if the player is in range
            if (!VectorHelper.ApproximatelyEqual(transform.position, anchor_pos, walk_area)) reset_pos = true;
        }
        
        Quaternion camera_rot = Quaternion.Euler(0, cameraGO.transform.eulerAngles.y, 0);
        Quaternion anchor_rot = Quaternion.Euler(0, transform.eulerAngles.y, 0);

        //Checks if the menu should be moved to face the player again
        if (reset_angle)
        {
            if (Quaternion.Angle(anchor_rot, camera_rot) > threshold)
            {
                anchor_rot = Quaternion.Lerp(anchor_rot, camera_rot, lerp_speed_rot * Time.deltaTime);
            }
            else
            {
                reset_angle = false;
            }
        }
        else
        {
            //Checks if the angle of the camera is facing away from the menu visible angle
            if (Quaternion.Angle(anchor_rot, camera_rot) > view_angle) reset_angle = true;
        }

        transform.rotation = anchor_rot;
    }

    //--------------------------------------------------------------------
    
    //Positions the menu in front of the player and pops it out with an animation
    public void PopIn()
    {
        Quaternion camera_rot = Quaternion.Euler(0, cameraGO.transform.eulerAngles.y, 0);
        transform.rotation = camera_rot;
        
        Vector3 camera_pos = cameraGO.transform.position;
        var new_pos = new Vector3(camera_pos.x, default_pos.y + camera_pos.y, camera_pos.z);
        anchor_pos = new_pos;
        transform.position = new_pos;
        
        gameObject.SetActive(true);
        transform.LeanScale(default_scale, animation_time).setEaseOutBack();
    }

    public void PopOut()
    {
        StartCoroutine(PopOutRoutine());
    }

    //Same thing but popping out the menu
    private IEnumerator PopOutRoutine()
    {
        transform.LeanScale(new Vector3(0,0,default_scale.z), animation_time).setEaseInBack();
        yield return new WaitForSeconds(animation_time + 0.1f);
        gameObject.SetActive(false);
    }
}
