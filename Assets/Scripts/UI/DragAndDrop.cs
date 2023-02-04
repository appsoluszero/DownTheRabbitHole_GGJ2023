using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class DragAndDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private Canvas canvas;

    // override anchoredPosition control
    [HideInInspector] public bool isOverriden = false;
    [HideInInspector] public Vector2 overridePosition = Vector2.zero;

    // public accessible
    public bool isDragged = false;

    // private variables
    private RectTransform rectTransform;
    [HideInInspector] public Vector2 targetAnchoredPosition = Vector2.zero;

    // event
    public delegate void EndDragEvent();
    public EndDragEvent endDragEvent;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Debug.Log("OnBeginDrag");
        isDragged = true;

        targetAnchoredPosition = rectTransform.anchoredPosition;

        // bring to front
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Debug.Log("OnDrag");
        targetAnchoredPosition += eventData.delta / canvas.scaleFactor;
        rectTransform.anchoredPosition = (isOverriden ? overridePosition : targetAnchoredPosition);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Debug.Log("OnEndDrag");
        isDragged = false;

        // event
        endDragEvent?.Invoke();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Debug.Log("OnPointerDown");
    }
}
