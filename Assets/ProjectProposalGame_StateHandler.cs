using System;
using System.Collections.Generic;
using UnityEngine;

public class ProjectProposalGame_StateHandler : MonoBehaviour
{
    public static ProjectProposalGame_StateHandler _instance;

    public enum GameState
    {
        Free,
        DraggingPaper,
    }

    public GameState currentGameState;

    public ProjectProposalGame_SubmitterScript correctSubmitter, incorrectSubmitter;

    public Action OnPaperSubmitted;

    public List<RectTransform> paperList;
    public List<RectTransform> paperLocList;

    public int submitCount = 3;

    void Awake() 
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;//Avoid doing anything else
        }

        if (_instance == null)
        {
            _instance = this;
        }
    }

    void Start()
    {
        //SetupToStartMinigame();
    }

    //Start the minigame here
    [ContextMenu("Starting minigame")]
    public void SetupToStartMinigame() 
    {
        for(var i = 0 ; i < paperList.Count ; ++i) {
            paperList[i].anchoredPosition = paperLocList[i].anchoredPosition;
            paperList[i].GetComponent<ProjectProposalGame_PaperScript>().GenerateTextElement();
        }
        submitCount = 3;
    }

    public void SubmitPaper(ProjectProposalGame_PaperScript paper) 
    {
        if(correctSubmitter.IsWaitingForSubmit && !incorrectSubmitter.IsWaitingForSubmit) 
        {
            if(paper.IsCorrect) {
                print("correct");
                LeanTween.move(paper.rectTransform, new Vector3(-500, 300, 0), 1).setOnStart(OnPaperSubmitted);
            } 
            else {
                print("incorrect");
                LeanTween.move(paper.rectTransform, new Vector3(-500, 300, 0), 1).setOnStart(OnPaperSubmitted);
            } 
        }
        else if(!correctSubmitter.IsWaitingForSubmit && incorrectSubmitter.IsWaitingForSubmit) 
        {
            if(!paper.IsCorrect) {
                print("correct");
                LeanTween.move(paper.rectTransform, new Vector3(500 + 1280, 300, 0), 1).setOnStart(OnPaperSubmitted);
            } 
            else {
                print("incorrect");
                LeanTween.move(paper.rectTransform, new Vector3(500 + 1280, 300, 0), 1).setOnStart(OnPaperSubmitted);
            } 
        }
        submitCount--;
        if(submitCount == 0) {
            //minigame done this round, do whateve
        }
    }
}
