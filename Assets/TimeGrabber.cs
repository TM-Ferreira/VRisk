using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class TimeGrabber : MonoBehaviour
{
    public GameData data;
    public TextMeshProUGUI textMesh;

    private void Start()
    {
        textMesh.text = TimeSpan.FromSeconds(float.Parse(data.time)).ToString(@"mm\:ss\:fff");
    }
}
