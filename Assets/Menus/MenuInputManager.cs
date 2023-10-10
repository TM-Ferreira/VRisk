using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MenuInputManager : MonoBehaviour
{
    public GameData data;
    public TransitionManager transitionManager;
    public PauseMenuManager pauseManager;
    
    public GameObject menuOne;

    private MenuController menuOneController;
    //private MenuController menuTwoController;

    [SerializeField] private Vector3 defaultPos;
    [SerializeField] private Vector3 slidePos;

    [SerializeField] private float animationDuration = 0.5f;
    [SerializeField] private float delay = 0.5f;

    private bool isOpen = false;
    private bool busy = false;
    
    private void Awake()
    {
        menuOneController = menuOne.GetComponent<MenuController>();
    }

    private void Start()
    {
        defaultPos = menuOne.transform.localPosition;
    }

    // ------------------------------------------------------------------

    public void OpenCloseSettings()
    {
        //Ignore input if animation is not finished
        if (busy) return;

        StartCoroutine(OpenCloseCoroutine());
    }

    public void CloseSettings()
    {
        if (busy) return;

        isOpen = true;
        StartCoroutine(OpenCloseCoroutine());
    }
    
    public void StartSimulation()
    {
        data.NextScene = (int)GameData.SceneIndex.SIMULATION;
        transitionManager.LoadNextScene();
    }

    public void CloseSimulation()
    {
        //Simply closes the game
        Application.Quit();
    }

    public void ResumeSimulation()
    {
        CloseSettings();

        if (pauseManager != null)
        {
            pauseManager.CloseMenu();
        }
    }

    public void GoBackToMainMenu()
    {
        Time.timeScale = 1;
        data.NextScene = (int)GameData.SceneIndex.MIAN_MENU;
        transitionManager.LoadNextScene();
    }
    
    // -------------------------------------------------------------------
    
    private IEnumerator OpenCloseCoroutine()
    {
        busy = true;
        
        //Closes/opens menu depending on the last action, uses easing for smooth motion
        if (isOpen)
        {
            //menuTwoController.CloseMenu();
            yield return new WaitForSeconds(delay);
            menuOneController.OpenMenu();
        }
        else
        {
            menuOneController.CloseMenu();
            yield return new WaitForSeconds(delay);
            //menuTwoController.OpenMenu();
        }

        yield return new WaitForSeconds(delay);
        isOpen = !isOpen;
        busy = false;
    }
}
