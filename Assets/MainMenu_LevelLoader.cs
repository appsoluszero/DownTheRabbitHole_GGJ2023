using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu_LevelLoader : MonoBehaviour
{
    public bool isLoading;
    public GameObject loadScreen;
    public TextMeshProUGUI loadProgress;

    public void LoadScene() 
    {
        if(isLoading) return;
        if(loadScreen != null)
            loadScreen.SetActive(true);
        StartCoroutine(loadSceneAsync(1));
    }

    public void LoadScene(int i) 
    {
        if(isLoading) return;
        if(loadScreen != null)
            loadScreen.SetActive(true);
        StartCoroutine(loadSceneAsync(i));
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    IEnumerator loadSceneAsync(int i) 
    {
        isLoading = true;
        GunPartChecker.gunPartCheckers.Clear();
        var op = SceneManager.LoadSceneAsync(i, LoadSceneMode.Single);
        op.allowSceneActivation = false;
        while(op.progress < 0.9f) {
            if(loadProgress != null)
                loadProgress.text = "Initializing world domination... " + (int)(op.progress/0.9f * 100) + "%";
            yield return null;
        }
        if(loadProgress != null)
            loadProgress.text = "Initializing world domination... 100%";
        yield return new WaitForSeconds(1.5f);
        isLoading = false;

        if(i == 1) 
        {
            DTRH_GameManager.cultistTotalScore = 0;
            DTRH_GameManager.royTotalScore = 0;
            DTRH_GameManager.rodyTotalScore = 0;
            DTRH_GameManager.day = 1;
        }

        op.allowSceneActivation = true;
    }
}
