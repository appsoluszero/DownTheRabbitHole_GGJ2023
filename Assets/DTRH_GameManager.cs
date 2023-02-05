using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
    [Header("Timeout sequence")]
    public RectTransform timeoutObject;
    public float timeToStayChange, timeToMove;


    [Header("Lobby")]
    public RoomType currentRoomType;
    [Header("Player")]
    public GameObject playerChar;

    [Header("Room UI")]
    public GameObject AdminOfficeUI;
    public GameObject AdminOfficeReadyIcon;
    public GameObject DNAOfficeUI;
    public GameObject DNAOfficeReadyIcon;
    public GameObject WeaponDevUI;
    public GameObject WeaponDevReadyIcon;
    public GameObject CarrotGardenUI;
    public GameObject CarrotGardenReadyIcon;

    [Header("Game Loop")]
    public static int day = 1;
    public bool gameTimerRunnning = false;

    [Header("Game Loop Timer")]
    public GameTimer gameTimer;
    public enum GamePhase {Work, Afterwork};
    public GamePhase gamePhase = GamePhase.Work;
    public bool inMinigame = false;

    [Header("Score system")]
    public static int cultistTotalScore;
    public static int royTotalScore;
    public static int rodyTotalScore;

    public int cultistScore = 0;
    public int cultistHappyThreshold = 7;
    public DialogueTrigger cultistTrigger;
    public int royScore = 0;
    public int royHappyThreshold = 9;
    public DialogueTrigger royTrigger;
    public int rodyScore = 0;
    public int rodyHappyThreshold = 5;
    public DialogueTrigger rodyTrigger;

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

        if (!cultistTrigger) Debug.Log("Cultist's Dialogue Trigger not found");
        if (!royTrigger) Debug.Log("Roy's Dialogue Trigger not found");
        if (!rodyTrigger) Debug.Log("Rody's Dialogue Trigger not found");
    }

    void Update() {
        if (gameTimerRunnning) {
            UpdateGameLoop();
        }

        UpdateReadyIcon();

        UpdateScore();

        if (gamePhase == GamePhase.Afterwork) ExitDoor.SetActive(true);
        else ExitDoor.SetActive(false);
    }

    void UpdateScore() {
        if (gamePhase != GamePhase.Afterwork) {
            if (cultistTrigger) cultistTrigger.gameObject.SetActive(false);
            if (royTrigger) royTrigger.gameObject.SetActive(false);
            if (rodyTrigger) rodyTrigger.gameObject.SetActive(false);
            return;
        }
        if (cultistTrigger) {
            cultistTrigger.gameObject.SetActive(true);
            cultistTrigger.isBad = !(cultistScore >= cultistHappyThreshold);
        }
        if (royTrigger) {
            royTrigger.gameObject.SetActive(true);
            royTrigger.isBad = !(royScore >= royHappyThreshold);
        }
        if (rodyTrigger) {
            rodyTrigger.gameObject.SetActive(true);
            rodyTrigger.isBad = !(rodyScore >= rodyHappyThreshold);
        }
    }

    void UpdateReadyIcon() {
        if (gamePhase == GamePhase.Work && gameTimerRunnning) {
            if (gameTimer.proposalGameTimer <= 0) AdminOfficeReadyIcon.SetActive(true);
            else AdminOfficeReadyIcon.SetActive(false);
            if (gameTimer.carrotGameTimer <= 0) CarrotGardenReadyIcon.SetActive(true);
            else CarrotGardenReadyIcon.SetActive(false);
            if (gameTimer.dnaGameTimer <= 0) DNAOfficeReadyIcon.SetActive(true);
            else DNAOfficeReadyIcon.SetActive(false);
            if (gameTimer.gunGameTimer <= 0) WeaponDevReadyIcon.SetActive(true);
            else WeaponDevReadyIcon.SetActive(false);
        } else {
            AdminOfficeReadyIcon.SetActive(false);
            CarrotGardenReadyIcon.SetActive(false);
            DNAOfficeReadyIcon.SetActive(false);
            WeaponDevReadyIcon.SetActive(false);
        }
    }

    IEnumerator StartIntroSequence() 
    {
        audioPlayer.PlayOneShot(introClip);
        yield return new WaitForSeconds(timeToStay);
        LeanTween.moveY(introObject, -720, timeToMoveOut).setEase(easeType).setOnComplete(OnIntroEnd).setOnStart(StartGameLoop);
    }

    void IntroEnd() 
    {
        introObject.gameObject.SetActive(false);
        audioPlayer.Play();
    }

    public UnityEvent ExitEvent;
    public GameObject ExitDoor;
    public void EnterRoom() 
    {
        // assuming EnterRoom() is only used for entering minigame
        if (gamePhase == GamePhase.Work) {
            switch(currentRoomType) 
            {
                case RoomType.AdminOffice:
                    if (gameTimer.proposalGameTimer > 0) break;
                    AdminOfficeUI.SetActive(true);
                    ProjectProposalGame_StateHandler._instance.SetupToStartMinigame();
                    inMinigame = true;
                    playerChar.SetActive(false);
                    break;
                case RoomType.Garden:
                    if (gameTimer.carrotGameTimer > 0) break;
                    CarrotGardenUI.SetActive(true);
                    CarrotCheckGame_MainManager._instance.SetupToStartMinigame();
                    inMinigame = true;
                    playerChar.SetActive(false);
                    break;
                case RoomType.RandD_Gun:
                    if (gameTimer.gunGameTimer > 0) break;
                    WeaponDevUI.SetActive(true);
                    GunMinigame.StartMinigameStatic();
                    inMinigame = true;
                    playerChar.SetActive(false);
                    break;
                case RoomType.RandD_DNA:
                    if (gameTimer.dnaGameTimer > 0) break;
                    DNAOfficeUI.SetActive(true);
                    DNAMinigame.StartMinigameStatic();
                    inMinigame = true;
                    playerChar.SetActive(false);
                    break;
                case RoomType.Exit:
                    //Do the exit
                    ExitEvent.Invoke();
                    break;
                    
            }
        } else {
            switch(currentRoomType) 
            {
                case RoomType.Exit:
                    //Do the exit
                    ExitEvent.Invoke();
                    break;
                    
            }
        }
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
            timeoutObject.gameObject.SetActive(true);
            // LeanTween.moveY(timeoutObject, 0, timeToMove).setEase(easeType).setOnComplete(_ => StartCoroutine(timeoutSequence()));
            StartCoroutine(timeoutSequence());
        }

        // update the timer
        UpdateGameTimer();

        // quit minigame when timers are done
        if (inMinigame && gameTimer.dayTimer <= 0) {
            ExitRoom();
        }
    }

    IEnumerator timeoutSequence() 
    {
        yield return new WaitForSeconds(timeToStayChange);
        timeoutObject.gameObject.SetActive(false);
        ChangeToAfterWork();
        /*LeanTween.moveY(timeoutObject, -720, timeToMove).setEase(easeType).setOnComplete(_ => {
            
        });*/
        
    }

    void ChangeToAfterWork()
    {
        gamePhase = GamePhase.Afterwork;    
        cultistTotalScore += cultistScore;
        royTotalScore += royScore;
        rodyTotalScore += rodyScore;
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
    Garden,
    Exit
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
