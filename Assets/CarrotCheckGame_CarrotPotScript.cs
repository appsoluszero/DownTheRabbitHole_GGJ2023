using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CarrotCheckGame_CarrotPotScript : MonoBehaviour, IPointerClickHandler
{
    public RectTransform barTransform, barRightTransform;
    public RectTransform arrowTransform;
    public float arrowTimePerSide;
    public float checkingSize;
    private float rightPercent;
    private bool waitingForCare;

    [Header("Sprite database")]
    public Sprite goodCarrotSprite;
    public List<Sprite> badCarrotSpriteList;

    [Header("UI element")]
    public Image carrotImage;

    public void ActivateNeedAttentionMode() 
    {
        barTransform.gameObject.SetActive(true);
        arrowTransform.anchoredPosition = new Vector2(0, arrowTransform.anchoredPosition.y);
        barRightTransform.sizeDelta = new Vector2(checkingSize, barRightTransform.sizeDelta.y);
        LeanTween.moveX(arrowTransform, barTransform.sizeDelta.x, arrowTimePerSide).setLoopPingPong();
        rightPercent = Random.Range(0, 100)/100f * (barTransform.sizeDelta.x - checkingSize/2f);
        barRightTransform.anchoredPosition = new Vector2(rightPercent, barRightTransform.anchoredPosition.y);
        waitingForCare = true;
        carrotImage.sprite = badCarrotSpriteList[Random.Range(0, badCarrotSpriteList.Count)];
    }

    public void OnPointerClick(PointerEventData ped) 
    {
        if(!waitingForCare) return;
        var currPos = arrowTransform.anchoredPosition.x;
        if(currPos >= rightPercent-checkingSize/2f && currPos <= rightPercent+checkingSize/2f) 
        {
            barTransform.gameObject.SetActive(false);
            carrotImage.sprite = goodCarrotSprite;
            waitingForCare = false;
            CarrotCheckGame_MainManager._instance.toBeCompleted--;
            if (CarrotCheckGame_MainManager._instance.toBeCompleted == 0) CarrotCheckGame_MainManager._instance.gameSuccessEvent.Invoke();
        }
        else print("no");
    }
}
