using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShineMovement : MonoBehaviour
{
    public bool shining = true;
    public bool repeat_shine = true;

    public bool apply_delay = false;
    public float delay = 0.5f;
    
    [SerializeField] private float shine_speed;
    [SerializeField] private float leap = 100;
    [SerializeField] private float shine_duration = 1.0f;
    
    private RectTransform rect_transform;
    private Vector2 initial_pos;
    private Vector2 final_pos;
    
    private float shine_timer = 0;
    private float delay_timer = 0;

    void Awake()
    {
        rect_transform = this.GetComponent<RectTransform>();
    }

    void Start()
    {
        initial_pos = rect_transform.anchoredPosition;
        final_pos = new Vector2(initial_pos.x + leap, initial_pos.y);
    }
    
    void FixedUpdate()
    {
        if (!shining) return;

        if (apply_delay && delay_timer < delay)
        {
            delay_timer += Time.fixedDeltaTime;
            return;
        }

        shine_timer += Time.fixedDeltaTime;
        if (shine_timer > shine_duration)
        {
            rect_transform.anchoredPosition = initial_pos;
            shine_timer = 0;

            if (!repeat_shine && shining)
            { 
                shining = false;
            }
        }

        if (rect_transform.anchoredPosition != final_pos)
        {
            rect_transform.anchoredPosition = Vector2.Lerp(rect_transform.anchoredPosition, final_pos, shine_speed);
        }
    }
}
