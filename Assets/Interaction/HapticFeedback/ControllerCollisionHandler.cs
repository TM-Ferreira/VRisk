using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public class ControllerCollisionHandler : MonoBehaviour
{
    public HapticFeedbackHandler feedback_handler;

    [Range(0,1)]
    public float haptic_intensity;
    public float haptic_duration;

    public HapticFeedbackControllerID id;

    public void OnCollisionEnter(Collision other)
    {
        if (!other.transform.CompareTag(transform.tag) && !other.transform.CompareTag("Player"))
        {
            feedback_handler.triggerHapticFeedback(id, haptic_intensity, haptic_duration);
        }
    }
}
