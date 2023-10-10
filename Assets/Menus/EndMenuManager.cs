using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndMenuManager : MonoBehaviour
{
    public GameData data;
    public TransitionManager transitionManager;

    public void ExitApplication()
    {
        Application.Quit();
    }

    public void BackToMainMenu()
    {
        data.NextScene = (int)GameData.SceneIndex.MIAN_MENU;
        transitionManager.LoadNextScene();
    }
}
