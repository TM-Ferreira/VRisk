using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonAnimator : MonoBehaviour
{
    public Outline outline;

    private void Start()
    {
        outline = this.GetComponent<Outline>();
    }

    public void OnHoverEnter()
    {
        outline.enabled = true;
    }

    public void OnHoverExit()
    {
        outline.enabled = false;
    }
}
