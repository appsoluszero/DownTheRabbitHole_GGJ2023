using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CarrotCheckGame_CarrotPotScript : MonoBehaviour, IPointerClickHandler
{
    public RectTransform barTransform, barRightTransform;
    public RectTransform arrowTransform;
    public float arrowTimePerSide;
    public float checkingSize;
    private float rightPercent;
    private bool waitingForCare;

    public void ActivateNeedAttentionMode() 
    {
        barTransform.gameObject.SetActive(true);
        arrowTransform.anchoredPosition = new Vector2(0, arrowTransform.anchoredPosition.y);
        barRightTransform.sizeDelta = new Vector2(checkingSize, barRightTransform.sizeDelta.y);
        LeanTween.moveX(arrowTransform, barTransform.sizeDelta.x, arrowTimePerSide).setLoopPingPong();
        rightPercent = Random.Range(0, 100)/100f * (barTransform.sizeDelta.x - checkingSize/2f);
        barRightTransform.anchoredPosition = new Vector2(rightPercent, barRightTransform.anchoredPosition.y);
        waitingForCare = true;
    }

    public void OnPointerClick(PointerEventData ped) 
    {
        if(!waitingForCare) return;
        var currPos = arrowTransform.anchoredPosition.x;
        if(currPos >= rightPercent-checkingSize/2f && currPos <= rightPercent+checkingSize/2f) 
        {
            barTransform.gameObject.SetActive(false);
        }
        else print("no");
    }
}
