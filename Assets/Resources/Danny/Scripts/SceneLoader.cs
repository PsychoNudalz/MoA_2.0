using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Slider loadingBar;
    [SerializeField] private TMP_Text loadingText;
    [SerializeField] private Image crosshair;
 
    private void OnEnable()
    {
        StartCoroutine(SpinLogo());
    }

    IEnumerator SpinLogo()
    {
        while (this.isActiveAndEnabled)
        {
            crosshair.transform.Rotate(0, 2f, 0);
            yield return null;
        }
    }

    public void LoadWithLoadingScreen(string sceneName, bool sliderActive = false)
    {
        loadingBar.gameObject.SetActive(sliderActive);
        if (!sliderActive)
        {
            crosshair.gameObject.SetActive(true);
        }
        StartCoroutine(LoadAsyncronously(sceneName));
        Debug.Log("Loading with loading screen");

        //Anson: set time scale to 1
        Time.timeScale = 1f;
    }

    IEnumerator LoadAsyncronously (string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        loadingScreen.SetActive(true);
        
        while (!operation.isDone)
        {

            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            //Debug.LogError("Load" + sceneName + " Progress - " + progress);
            loadingBar.value = operation.progress;
            
            yield return null;
        }
        
    }
}
