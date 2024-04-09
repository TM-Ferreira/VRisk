using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Camera))]
public class TopDownCamMove : MonoBehaviour
{
    public float panning_speed;
    public float zoom_speed;
    public float zoom_increase_step = 32.0f;

    private Camera cam;
    private Input.DataVisualiserInputMapActions action_map;
    
    void Start()
    {
        cam = GetComponent<Camera>();
        action_map = GameManager.Instance.InputHandler.input_asset.DataVisualiserInputMap;
    }
    
    void Update()
    {
        if (action_map.PanView.IsPressed())
        {
            Vector2 input = action_map.MouseDelta.ReadValue<Vector2>();
          
            Vector3 pan_direction = new Vector3(input.x * -1.0f, input.y * -1.0f, 0) * (panning_speed * Time.deltaTime);
          
            cam.transform.Translate(pan_direction);
        }

        if (action_map.ZoomView.IsPressed())
        {
            float direction = action_map.ZoomView.ReadValue<float>() / 120;
            float size = cam.orthographicSize;
            float multiplier = size > zoom_increase_step ? 2 : 1;

            cam.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - direction * (zoom_speed * multiplier), 0.5f, 80f);
        }
    }
}
