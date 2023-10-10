using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageBlink : MonoBehaviour
{
    [SerializeField] private int interval = 1;
    private float timer = 0;
    private Image image;
    
    private void Awake()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;

        if (!(timer >= interval)) return;
        
        timer = 0;
        image.enabled = !image.enabled;
    }
}
