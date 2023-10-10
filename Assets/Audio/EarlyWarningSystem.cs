using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EarlyWarningSystem : MonoBehaviour
{
    public List<AudioSource> sources;

    private void Awake()
    {
        sources.AddRange(transform.GetComponentsInChildren<AudioSource>());
    }

    private void Start()
    {
        GameManager.Instance.InputHandler.input_asset.VRiskExperienceInputMap.Debug.started += test;
    }

    public void test(InputAction.CallbackContext _context)
    {
        triggerWarningSiren(10);
    }

    public void triggerWarningSiren(float _duration)
    {
        foreach (var source in sources)
        {
            //GameManager.Instance.AudioManager.PlaySound(source, true, false, source.transform.position, AudioManager.SoundID.WARNING_SIREN, _duration);
        }
    }
}
