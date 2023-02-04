using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ProjectProposalGame_PaperScript : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    private RectTransform rectTransform;
    private Vector2 mousePos, mouseOffset;

    void Start() 
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData ped) 
    {
        mousePos = ped.position;
        mousePos.x *= 1280f/Screen.width;
        mousePos.y *= 720f/Screen.height;
        mouseOffset = mousePos - rectTransform.anchoredPosition;
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData ped) 
    {
        mousePos = ped.position;
        mousePos.x *= 1280f/Screen.width;
        mousePos.y *= 720f/Screen.height;
        mousePos -= mouseOffset;
        GetComponent<RectTransform>().anchoredPosition = mousePos;
    }
}
