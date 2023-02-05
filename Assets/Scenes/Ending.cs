using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Ending : MonoBehaviour
{
    public Sprite e1;
    public Sprite e2;
    public Sprite e3;
    public Sprite e4;
    public Sprite e5;

    public Image image;

    public TMP_Text t1;

    void Start() {
        bool cultist = DTRH_GameManager.cultistTotalScore >= 23;
        bool roy = DTRH_GameManager.royTotalScore >= 27;
        bool rody = DTRH_GameManager.rodyTotalScore >= 17;

        if (!cultist) image.sprite = e3;
        else if (cultist && !roy && !rody) image.sprite = e2;
        else if (cultist && roy && rody) image.sprite = e1;
        else if (cultist && roy && !rody) image.sprite = e4;
        else image.sprite = e5;

        t1.text = $"cultist score: {DTRH_GameManager.cultistTotalScore} \nroy score: {DTRH_GameManager.royTotalScore} \nrody score: {DTRH_GameManager.rodyTotalScore}"; 

        StartCoroutine(EndTimer()); 
    }

    IEnumerator EndTimer() {
        yield return new WaitForSeconds(6);
        SceneManager.LoadScene(0);
    }
}
