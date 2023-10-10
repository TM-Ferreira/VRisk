using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogoFader : MonoBehaviour
{
    [SerializeField] private float delay = 0.5f;
    [SerializeField] private float scaleSpeed = 0.8f;

    private bool doOnce = true;
    private float timer = 0;

    private void Update()
    {
        if (timer < delay)
        {
            timer += Time.deltaTime;
        }
        else
        {
            if (doOnce)
            {
                transform.LeanScale(Vector3.zero, scaleSpeed).setEaseInBack();
                doOnce = false;
            }
        }
    }
}
