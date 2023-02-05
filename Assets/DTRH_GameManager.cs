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
    [Header("Player")]
    public GameObject playerChar;

    [Header("Room UI")]
    public GameObject AdminOfficeUI;
    public GameObject DNAOfficeUI;
    public GameObject WeaponDevUI;
    public GameObject CarrotGardenUI;

    [Header("Game Loop")]
    public static int day = 1;
    public bool gameTimerRunnning = false;

    [Header("Game Loop Timer")]
    public GameTimer gameTimer;
    public enum GamePhase {Work, Afterwork};
    public GamePhase gamePhase = GamePhase.Work;
    public bool inMinigame = false;

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

        // Start game loop
        OnIntroEnd += StartGameLoop;
    }

    void Update() {
        if (gameTimerRunnning) {
            UpdateGameLoop();
        }
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

    public void EnterRoom() 
    {
        // assuming EnterRoom() is only used for entering minigame
        if (gamePhase != GamePhase.Work) return;
        switch(currentRoomType) 
        {
            case RoomType.AdminOffice:
                AdminOfficeUI.SetActive(true);
                gameTimer.proposalGameTimerStart();
                inMinigame = true;
                break;
            case RoomType.Garden:
                CarrotGardenUI.SetActive(true);
                gameTimer.carrotGameTimerStart();
                inMinigame = true;
                break;
            case RoomType.RandD_Gun:
                WeaponDevUI.SetActive(true);
                //GunMinigame.instance.StartMinigame();
                gameTimer.gunGameTimerStart();
                inMinigame = true;
                break;
            case RoomType.RandD_DNA:
                DNAOfficeUI.SetActive(true);
                //DNAMinigame.instance.StartMinigame();
                gameTimer.dnaGameTimerStart();
                inMinigame = true;
                break;
                
        }
        playerChar.SetActive(false);
    }

    // call when exit minigame room
    public void ExitRoom() {
        AdminOfficeUI.SetActive(false);
        CarrotGardenUI.SetActive(false);
        WeaponDevUI.SetActive(false);
        DNAOfficeUI.SetActive(false);
        inMinigame = false;

        playerChar.SetActive(true);
    }

    public void ProceedNextDay() {
        day++;
        // TODO: change scene appropriately
    }

    // ================================Game Loop===============================
    public void StartGameLoop() {
        // set current gamephase
        gamePhase = GamePhase.Work;
        gameTimerRunnning = true;

        // start dat timer
        gameTimer.dayTimerStart();
    }

    void UpdateGameLoop() {
        // check timer done
        if (gameTimer.dayTimer <= 0 && gamePhase == GamePhase.Work) {
            // time's up for the day
            gamePhase = GamePhase.Afterwork;
            // TODO: change stuff to after work
            // TODO: close all minigame
        }

        // update the timer
        UpdateGameTimer();

        // quit minigame when timers are done
        if (inMinigame && (gameTimer.minigameTimer <= 0 || gameTimer.dayTimer <= 0)) {
            ExitRoom();
        }
    }

    void UpdateGameTimer() {
        gameTimer.dayTimer -= Time.deltaTime;
        gameTimer.minigameTimer -= Time.deltaTime;
        if (gameTimer.dayTimer < 0) gameTimer.dayTimer = 0;
        if (gameTimer.minigameTimer < 0) gameTimer.minigameTimer = 0;
    }
}

public enum RoomType
{
    None,
    AdminOffice,
    RandD_DNA,
    RandD_Gun,
    Garden
}

[System.Serializable]
public class GameTimer {
   public float dayTimerInit = 180;
   public float dayTimer = 0;
   public float proposalGameTimerInit = 40;
   public float carrotGameTimerInit = 40;
   public float dnaGameTimerInit = 40;
   public float gunGameTimerInit = 40;
   public float minigameTimer;

   public void dayTimerStart() => dayTimer = dayTimerInit;
   public void proposalGameTimerStart() => minigameTimer = proposalGameTimerInit;
   public void carrotGameTimerStart() => minigameTimer = carrotGameTimerInit;
   public void dnaGameTimerStart() => minigameTimer = dnaGameTimerInit;
   public void gunGameTimerStart() => minigameTimer = gunGameTimerInit;
}
