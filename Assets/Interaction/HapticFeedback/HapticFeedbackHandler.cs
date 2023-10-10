using System;
using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public enum HapticFeedbackControllerID
{
    LEFT, 
    RIGHT
}
public class HapticFeedbackHandler : MonoBehaviour
{
    public XRBaseController left_controller;
    public XRBaseController right_controller;

    public void triggerHapticFeedback(HapticFeedbackControllerID _controller, float _intensity, float _duration)
    {
        switch (_controller)
        {
            case HapticFeedbackControllerID.LEFT:
            {
                left_controller.SendHapticImpulse(_intensity, _duration);
                break;
            }
            case HapticFeedbackControllerID.RIGHT:
            {
                right_controller.SendHapticImpulse(_intensity, _duration);
                break;
            }

            default:
            {
                throw new ArgumentOutOfRangeException(nameof(_controller), _controller, null);
            }
        }
    }

    public void triggerSeismicHapticFeedback(float _intensity, float _duration)
    {
        StartCoroutine(SeismicHapticFeedback(_intensity, _duration, 0.03f));
    }

    public void triggerCosIntensityHapticFeedback(float _intensity, float _duration)
    {
        StartCoroutine(CosHapticFeedback(_intensity, _duration, 0.03f));
    }
    
    private IEnumerator SeismicHapticFeedback(float _max_intensity, float _duration, float _alter_intensity_interval)
    {
        float elapsed = 0.0f;

        while (elapsed < _duration)
        {
            float progress = elapsed / _duration;
            float intensity = _max_intensity * GameManager.earthquakeIntensityCurve(progress);

            left_controller.SendHapticImpulse(intensity, 1);
            right_controller.SendHapticImpulse(intensity, 1);

            yield return new WaitForSeconds(_alter_intensity_interval);
            elapsed += Time.deltaTime;
        }
    }
    
    private IEnumerator CosHapticFeedback(float _max_intensity, float _duration, float _alter_intensity_interval)
    {
        float elapsed = 0.0f;

        while (elapsed < _duration)
        {
            float progress = elapsed / _duration;
            float intensity = _max_intensity * Mathf.Cos(progress * Mathf.PI);

            left_controller.SendHapticImpulse(intensity, 1);
            right_controller.SendHapticImpulse(intensity, 1);


            yield return new WaitForSeconds(_alter_intensity_interval);
            elapsed += Time.deltaTime;
        }
    }
}