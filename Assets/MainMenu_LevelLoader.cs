using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu_LevelLoader : MonoBehaviour
{
    public GameObject loadScreen;
    public TextMeshProUGUI loadProgress;

    public void LoadScene() 
    {
        if(loadScreen != null)
            loadScreen.SetActive(true);
        StartCoroutine(loadSceneAsync(1));
    }

    public void LoadScene(int i) 
    {
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
        op.allowSceneActivation = true;

        if(i == 1) 
        {
            DTRH_GameManager.cultistTotalScore = 0;
            DTRH_GameManager.royTotalScore = 0;
            DTRH_GameManager.rodyTotalScore = 0;
        }
    }
}
