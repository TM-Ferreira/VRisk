using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class SafeZoneScript : MonoBehaviour
{
    public GameObject head;
    public NavigationArrow nav_arrow;
    public GameObject celebration_effects;

    public GameData data;
    public TransitionManager transitionManager;

    [SerializeField] private float transitionDelay = 2.0f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Celebrate
            GameManager.Instance.AudioManager.PlaySound(false, false, head.transform.position, AudioManager.SoundID.WIN);
            celebration_effects.SetActive(true);

            nav_arrow.navigating = false;
            GameManager.Instance.DataTracker.recordTime(true);
            StartCoroutine(DelayToTransition());

            Debug.Log("transition");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            nav_arrow.navigating = true;
        }
    }

    private IEnumerator DelayToTransition()
    {
        yield return new WaitForSeconds(transitionDelay);

        data.NextScene = (int) GameData.SceneIndex.END_MENU;
        transitionManager.LoadNextScene();
    }
}
