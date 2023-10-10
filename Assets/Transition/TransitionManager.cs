using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{
    public GameData data;
    
    public FadeScreen fade_screen;
    public bool transitioning = false;

    public void LoadNextScene()
    {
        //Goes into the loading scene which will load the next scene
        StartCoroutine(AsyncLoadSceneRoutine((int)GameData.SceneIndex.LOADING_SCENE));
    }

    public void AsyncLoadNextScene()
    {
        StartCoroutine(AsyncLoadSceneRoutine(data.NextScene));
    }

    IEnumerator AsyncLoadSceneRoutine(int scene_index)
    {
        transitioning = true;
        fade_screen.FadeOut();

        AsyncOperation operation = SceneManager.LoadSceneAsync(scene_index);
        operation.allowSceneActivation = false;

        float timer = 0;
        while(timer <= fade_screen.fade_duration && !operation.isDone)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        operation.allowSceneActivation = true;
        transitioning = false;
    }
}
