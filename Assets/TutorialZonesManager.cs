using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialZonesManager : MonoBehaviour
{
    public enum ZoneID
    {
        ZONE1,
        ZONE2,
        ZONE3
    }
    
    public GameData data;
    public TransitionManager transitionManager;
    public SettingsRefresher settingsRefresher;
    public GameObject celebration_effects;
    [SerializeField] private float transitionDelay = 2.0f;

    public GameObject referenceArrow;
    public GameObject zone1;
    public GameObject zone2;
    public GameObject zone3;

    public GameObject groupZone1;
    public GameObject groupZone2;
    public GameObject groupZone3;

    public SettingsTrigger settingsTrigger;
    
    void Start()
    {
        zone2.SetActive(false);
        zone3.SetActive(false);
        groupZone2.SetActive(false);
        groupZone3.SetActive(false);
    }

    public void ZoneEntered(ZoneID id)
    {
        switch (id)
        {
            case ZoneID.ZONE1:
                zone2.SetActive(true);
                referenceArrow.transform.position = zone2.transform.position;
                groupZone2.SetActive(true);
                data.movementType = GameData.MovementType.GESTURE;
                break;
            
            case ZoneID.ZONE2:
                zone3.SetActive(true);
                referenceArrow.transform.position = zone3.transform.position;
                groupZone3.SetActive(true);
                settingsTrigger.enabled = true;
                settingsRefresher.RefreshAll();
                break;
            
            case ZoneID.ZONE3:
                StartCoroutine(DelayToTransition());
                break;
        }
        
        StartCoroutine(DisableEffects());
    }

    private IEnumerator DelayToTransition()
    {
        yield return new WaitForSeconds(transitionDelay);

        data.NextScene = (int) GameData.SceneIndex.SIMULATION;
        transitionManager.LoadNextScene();
    }
    
    private IEnumerator DisableEffects()
    {
        yield return new WaitForSeconds(2.0f);
        celebration_effects.SetActive(false);
    }
}
