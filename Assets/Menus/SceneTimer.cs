using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTimer : MonoBehaviour
{
    public TransitionManager trans_manager;
    public GameData data;
    
    //In seconds
    [SerializeField] private float hold_time = 3.0f;
    private float timer = 0.0f;
    private bool doOnce = false;
    
    private void FixedUpdate()
    {
        if (trans_manager.transitioning) return;
            
        timer += Time.fixedDeltaTime;

        if (timer > hold_time && !doOnce)
        {
            doOnce = true;
            trans_manager.AsyncLoadNextScene();
        }
    }
}
