using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ProjectProposalGame_PaperScript : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public RectTransform rectTransform;
    public bool AbleToSubmit;

    [Header("Paper component")]
    public Image logo;
    public TextMeshProUGUI motto;
    public Image signature;

    [Header("Paper database")]
    public Sprite logoRight, logoWrong;
    public string rightMotto;
    public List<string> wrongMotto;
    public List<Sprite> signatureDb;

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
            ProjectProposalGame_StateHandler._instance.SubmitPaper(this);
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

    public void GenerateTextElement()
    {
        IsCorrect = (Random.Range(0, 2) == 0) ? false : true;
        if(!IsCorrect) 
        {
            var WhatToWrong = Random.Range(0,2);
            if(WhatToWrong == 0) {
                logo.sprite = logoWrong;
                motto.text = rightMotto;
            }
            else if(WhatToWrong == 1) {
                logo.sprite = logoRight;
                motto.text = wrongMotto[Random.Range(0, wrongMotto.Count)];
            }
        }
        else {
            logo.sprite = logoRight;
            motto.text = rightMotto;
        }
        signature.sprite = signatureDb[Random.Range(0, signatureDb.Count)];
    }

    #endregion
}
