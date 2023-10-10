using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class MenuManager : MonoBehaviour
{
    [FormerlySerializedAs("camera")] public Camera scene_camera;

    public float distance_from_player = 2.0f;
    public float start_delay = 0.0f;

    private bool do_once = true;

    private void LateUpdate()
    {
        if (start_delay > 3.0f)
        {
            if (do_once)
            {
                this.transform.position = scene_camera.transform.position +
                                          new Vector3(scene_camera.transform.forward.x, 0, scene_camera.transform.forward.z)
                                              .normalized *
                                          distance_from_player;

                this.transform.LookAt(new Vector3(scene_camera.transform.position.x, scene_camera.transform.position.y,
                    scene_camera.transform.position.z));
                this.transform.forward *= -1;

                do_once = false;
            }
        }
        else
        {
            start_delay += Time.deltaTime;
        }
    }
}
