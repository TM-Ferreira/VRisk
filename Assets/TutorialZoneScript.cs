using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialZoneScript : MonoBehaviour
{
    public GameObject head;
    public GameObject celebration_effects;
    [SerializeField] private TutorialZonesManager.ZoneID zoneID = TutorialZonesManager.ZoneID.ZONE1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Celebrate
            GameManager.Instance.AudioManager.PlaySound(false, false, head.transform.position, AudioManager.SoundID.WIN);
            celebration_effects.SetActive(true);

            GetComponentInParent<TutorialZonesManager>().ZoneEntered(zoneID);
            gameObject.SetActive(false);
        }
    }
}
