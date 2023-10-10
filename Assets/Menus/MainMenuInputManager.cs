using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class MainMenuInputManager : MonoBehaviour
{
    public GameData data;
    public TransitionManager transitionManager;
    public UIAnchor uiAnchor;
    public TutorialManager tutorialManager;

    public GameObject warningText;
    public TextMeshProUGUI inputField;
    
    public GameObject mainMenu;
    public GameObject settingsMenu;
    public GameObject tutorialMenu;
    public GameObject keyboardMenu;

    private MenuController mainMenuController;
    private MenuController pauseMenuController;
    private MenuController tutorialMenuController;
    private MenuController keyboardMenuController;

    [SerializeField] private Vector3 mainMenuDefaultPos;
    [SerializeField] private Vector3 mainMenuSlidePos;
    [SerializeField] private Vector3 keyboardSlideOffset;

    [SerializeField] private float animationDuration = 0.5f;
    [SerializeField] private float delay = 0.5f;

    private bool settingsOpen = false;
    private bool keyboardOpen = false;
    private bool tutorialMenuOpen = false;

    private bool busy = false;

    private void Awake()
    {
        mainMenuController = mainMenu.GetComponent<MenuController>();
        pauseMenuController = settingsMenu.GetComponent<MenuController>();
        tutorialMenuController = tutorialMenu.GetComponent<MenuController>();
        keyboardMenuController = keyboardMenu.GetComponent<MenuController>();
    }

    private void Start()
    {
        mainMenuDefaultPos = mainMenu.transform.localPosition;
    }

    // Settings Controls -------------------------------------------------

    public void OpenCloseSettings()
    {
        //Ignore input if animation is not finished
        if (busy) return;
        
        StartCoroutine(OpenCloseSettingsCoroutine());
    }

    //Overload of the previous function to specifically close or open 
    public void OpenCloseSettings(bool state)
    {
        if (busy) return;

        settingsOpen = !state;
        StartCoroutine(OpenCloseSettingsCoroutine());
    }
    
    // Tutorial / keyboard controls ---------------------------------------

    public void OpenCloseTutorial()
    {
        if (inputField.text.Length < 2)
        {
            warningText.SetActive(true);
            return;
        }

        if (busy) return;

        warningText.SetActive(false);
        StartCoroutine(OpenCloseTutorialCoroutine());
    }

    public void OpenCloseKeyboard(bool state)
    {
        if (busy) return;

        keyboardOpen = !state;
        StartCoroutine(OpenCloseKeyboardCoroutine());
    }

    // Tutorial Window -------------------------------------------------------

    public void PlayTutorial(bool skip)
    {
        if (busy) return;

        if (skip)
        {
            uiAnchor.PopOut();
            data.NextScene = (int)GameData.SceneIndex.SIMULATION;
            transitionManager.LoadNextScene();
        }
        else
        {
            uiAnchor.PopOut();
            tutorialManager.StartTutorial();
        }
    }
    
    public void GameStarter(bool start)
    {
        if (busy) return;

        if (start)
        {
            data.NextScene = (int)GameData.SceneIndex.SIMULATION;
            transitionManager.LoadNextScene();
        }
        else
        {
            data.NextScene = (int)GameData.SceneIndex.MIAN_MENU;
            transitionManager.LoadNextScene();
        }
    }

    // Close application -------------------------------------------------
    
    public void CloseApplication()
    {
        Application.Quit();
    }
    
    // -------------------------------------------------------------------
    
    private IEnumerator OpenCloseSettingsCoroutine()
    {
        busy = true;
        
        //Closes/opens menu depending on the last action, uses easing for smooth motion
        if (settingsOpen)
        {
            pauseMenuController.CloseMenu();
            yield return new WaitForSeconds(delay);
            mainMenuController.OpenMenu();
        }
        else
        {
            mainMenuController.CloseMenu();
            yield return new WaitForSeconds(delay);
            pauseMenuController.OpenMenu();
        }

        yield return new WaitForSeconds(delay);
        settingsOpen = !settingsOpen;
        busy = false;
    }

    private IEnumerator OpenCloseKeyboardCoroutine()
    {
        if (!keyboardOpen && tutorialMenuOpen)
        {
            tutorialMenuController.CloseMenu();
        }

        if (keyboardOpen)
        {
            mainMenuController.CloseMenu();
            pauseMenuController.CloseMenu();
            
            yield return new WaitForSeconds(mainMenuController.animationTime);
            
            keyboardMenuController.OpenMenu();
            busy = true;
        }
        else
        {
            busy = true;
            keyboardMenuController.CloseMenu();
            yield return new WaitForSeconds(keyboardMenuController.animationTime);
            mainMenuController.OpenMenu();
        }

        yield return new WaitForSeconds(delay);
        keyboardOpen = !keyboardOpen;
        busy = false;
    }

    private IEnumerator OpenCloseTutorialCoroutine()
    {
        busy = true;
        
        if (!keyboardOpen)
        {
            if (tutorialMenuOpen)
            {
                tutorialMenuController.CloseMenu();
                yield return new WaitForSeconds(tutorialMenuController.animationTime);
                keyboardMenuController.OpenMenu();
            }
            else
            {
                var targetPos = tutorialMenu.transform.localPosition + keyboardSlideOffset;

                keyboardMenuController.CloseMenu();
                yield return new WaitForSeconds(keyboardMenuController.animationTime);
                tutorialMenuController.OpenMenu();
            }
        }

        yield return new WaitForSeconds(delay);
        tutorialMenuOpen = !tutorialMenuOpen;
        busy = false;
    }
}
