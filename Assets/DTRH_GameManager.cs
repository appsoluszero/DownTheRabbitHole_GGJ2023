using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DTRH_GameManager : MonoBehaviour
{
    public static DTRH_GameManager _instance;
    public RectTransform introObject;
    public float timeToStay, timeToMoveOut;
    public LeanTweenType easeType;
    private Action OnIntroEnd;
    public RoomType currentRoomType;

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
        OnIntroEnd += IntroEnd;
        StartCoroutine(StartIntroSequence());
    }

    IEnumerator StartIntroSequence() 
    {
        yield return new WaitForSeconds(timeToStay);
        LeanTween.moveY(introObject, -720, timeToMoveOut).setEase(easeType).setOnComplete(OnIntroEnd);
    }

    void IntroEnd() 
    {
        introObject.gameObject.SetActive(false);
    }
}

public enum RoomType
{
    None,
    AdminOffice,
    RandD,
    Garden
}
