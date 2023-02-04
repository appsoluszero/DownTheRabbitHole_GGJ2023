using UnityEngine;
using UnityEngine.EventSystems;

public class ProjectProposalGame_PaperScript : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    private RectTransform rectTransform;
    public bool AbleToSubmit;

    void Start() 
    {
        rectTransform = GetComponent<RectTransform>();
    }

    #region Dragging
    private Vector2 mouseOffset;
    public void OnPointerDown(PointerEventData ped) 
    {
        if(ProjectProposalGame_StateHandler._instance.currentGameState != ProjectProposalGame_StateHandler.GameState.Free) 
            return;
        ProjectProposalGame_StateHandler._instance.currentGameState = ProjectProposalGame_StateHandler.GameState.DraggingPaper;
        mouseOffset = ConvertTo720pRef(ped.position) - rectTransform.anchoredPosition;
        transform.SetAsLastSibling();
    }

    public void OnPointerUp(PointerEventData ped) 
    {
        if(ProjectProposalGame_StateHandler._instance.currentGameState != ProjectProposalGame_StateHandler.GameState.DraggingPaper) 
            return;
        ProjectProposalGame_StateHandler._instance.currentGameState = ProjectProposalGame_StateHandler.GameState.Free;
        if(AbleToSubmit)
            ProjectProposalGame_StateHandler._instance.SubmitPaper(IsCorrect);
    }

    public void OnDrag(PointerEventData ped) 
    {
        rectTransform.anchoredPosition = ConvertTo720pRef(ped.position) - mouseOffset;
    }

    private Vector2 ConvertTo720pRef(Vector2 pos) 
    {
        var modifiedPos = pos;
        modifiedPos.x *= 1280f/Screen.width;
        modifiedPos.y *= 720f/Screen.height;
        return modifiedPos;
    }
    #endregion

    #region Text generation/checking
    public bool IsCorrect;
    #endregion
}
