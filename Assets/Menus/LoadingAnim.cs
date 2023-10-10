using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoadingAnim : MonoBehaviour
{
    private TextMeshProUGUI text_mesh;
    
    [SerializeField] private int section_amount = 3;
    
    private float timer;
    private int section;

    private void Awake()
    {
        text_mesh = GetComponent<TextMeshProUGUI>();
    }

    private void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;

        if (timer > 1.0f)
        {
            text_mesh.text += ".";
            section++;
            timer = 0.0f;
        }

        if (section >= section_amount)
        {
            text_mesh.text = text_mesh.text.Substring(0, text_mesh.text.Length - section_amount);
            section = 0;
        }
    }
}
