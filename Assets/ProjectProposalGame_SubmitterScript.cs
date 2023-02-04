using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ProjectProposalGame_SubmitterScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool IsWaitingForSubmit;

    public void OnPointerEnter(PointerEventData ped) 
    {
        if(ProjectProposalGame_StateHandler._instance.currentGameState != ProjectProposalGame_StateHandler.GameState.DraggingPaper) 
            return;
        IsWaitingForSubmit = true;
    }

    public void OnPointerExit(PointerEventData ped) 
    {
        IsWaitingForSubmit = false;
    } 
}
