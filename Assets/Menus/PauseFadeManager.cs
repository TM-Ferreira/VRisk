using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PauseFadeManager : MonoBehaviour
{
    public RawImage fader;
    
    [SerializeField] private float lerpSpeed = 2.5f;
    [SerializeField] private float startAlpha = 0f;
    [SerializeField] private float endAlpha = 0.9f;
    [SerializeField] private float targetAlpha = 0;

    public void FadeIn()
    {
        targetAlpha = endAlpha;
    }

    public void FadeOut()
    {
        targetAlpha = startAlpha;
    }

    private void Update()
    {
        Color faderTintColor = fader.color;
        
        if (!Mathf.Approximately(faderTintColor.a, targetAlpha))
        {
            faderTintColor.a = Mathf.Lerp(faderTintColor.a, targetAlpha, lerpSpeed * Time.deltaTime);
        }
        
        fader.color = faderTintColor;
    }
}
