using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DTRH_GameManager : MonoBehaviour
{
    public static DTRH_GameManager _instance;
    [Header("Intro sequence")]
    public RectTransform introObject;
    public float timeToStay, timeToMoveOut;
    public LeanTweenType easeType;
    private Action OnIntroEnd;
    public AudioSource audioPlayer;
    public AudioClip introClip;


    [Header("Lobby")]
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

    [Header("Score system")]
    public int cultistScore = 0;
    public int cultistHappyThreshold = 7;
    public int royScore = 0;
    public int royHappyThreshold = 9;
    public int rodyScore = 0;
    public int rodyHappyThreshold = 5;

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
        audioPlayer.PlayOneShot(introClip);
        yield return new WaitForSeconds(timeToStay);
        LeanTween.moveY(introObject, -720, timeToMoveOut).setEase(easeType).setOnComplete(OnIntroEnd);
    }

    void IntroEnd() 
    {
        introObject.gameObject.SetActive(false);
        audioPlayer.Play();
    }

    public void EnterRoom() 
    {
        // assuming EnterRoom() is only used for entering minigame
        if (gamePhase != GamePhase.Work) return;
        switch(currentRoomType) 
        {
            case RoomType.AdminOffice:
                AdminOfficeUI.SetActive(true);
                ProjectProposalGame_StateHandler._instance.SetupToStartMinigame();
                inMinigame = true;
                break;
            case RoomType.Garden:
                CarrotGardenUI.SetActive(true);
                CarrotCheckGame_MainManager._instance.SetupToStartMinigame();
                inMinigame = true;
                break;
            case RoomType.RandD_Gun:
                WeaponDevUI.SetActive(true);
                GunMinigame.StartMinigameStatic();
                inMinigame = true;
                break;
            case RoomType.RandD_DNA:
                DNAOfficeUI.SetActive(true);
                DNAMinigame.StartMinigameStatic();
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

    public void FinProposalGame(bool success) {
        gameTimer.proposalGameTimerReset();

        //TODO: add score if success or something
    }
    public void FinCarrotGame(bool success) {
        gameTimer.carrotGameTimerReset();

        //TODO: add score if success or something
    }
    public void FinDNAGame(bool success) {
        gameTimer.dnaGameTimerReset();

        //TODO: add score if success or something
    }
    public void FinGunGame(bool success) {
        gameTimer.gunGameTimerReset();

        //TODO: add score if success or something
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
        gameTimer.dayTimerReset();
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
        if (inMinigame && gameTimer.dayTimer <= 0) {
            ExitRoom();
        }
    }

    void UpdateGameTimer() {
        gameTimer.dayTimer -= Time.deltaTime;
        gameTimer.proposalGameTimer -= Time.deltaTime;
        gameTimer.carrotGameTimer -= Time.deltaTime;
        gameTimer.dnaGameTimer -= Time.deltaTime;
        gameTimer.gunGameTimer -= Time.deltaTime;
        if (gameTimer.dayTimer < 0) gameTimer.dayTimer = 0;
        if (gameTimer.proposalGameTimer < 0) gameTimer.proposalGameTimer = 0;
        if (gameTimer.carrotGameTimer < 0) gameTimer.carrotGameTimer = 0;
        if (gameTimer.dnaGameTimer < 0) gameTimer.dnaGameTimer = 0;
        if (gameTimer.gunGameTimer < 0) gameTimer.gunGameTimer = 0;
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
   public float proposalGameTimer = 0;
   public float carrotGameTimerInit = 40;
   public float carrotGameTimer = 0;
   public float dnaGameTimerInit = 40;
   public float dnaGameTimer = 0;
   public float gunGameTimerInit = 40;
   public float gunGameTimer = 0;

   public void dayTimerReset() => dayTimer = dayTimerInit;
   public void proposalGameTimerReset() => proposalGameTimer = proposalGameTimerInit;
   public void carrotGameTimerReset() => carrotGameTimer = carrotGameTimerInit;
   public void dnaGameTimerReset() => dnaGameTimer = dnaGameTimerInit;
   public void gunGameTimerReset() => gunGameTimer = gunGameTimerInit;
}
