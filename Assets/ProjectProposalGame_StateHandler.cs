using System;
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
    public bool GameInSession;

    public Action OnPaperSubmitted;

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
    }
}
