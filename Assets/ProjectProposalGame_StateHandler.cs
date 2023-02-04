using System.Collections;
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

    public void SubmitPaper(bool truthValue) 
    {
        if(correctSubmitter.IsWaitingForSubmit && !incorrectSubmitter.IsWaitingForSubmit) 
        {
            if(truthValue) print("correct");
            else print("incorrect");
        }
        else if(!correctSubmitter.IsWaitingForSubmit && incorrectSubmitter.IsWaitingForSubmit) 
        {
            if(!truthValue) print("correct");
            else print("incorrect");
        }
    }
}
