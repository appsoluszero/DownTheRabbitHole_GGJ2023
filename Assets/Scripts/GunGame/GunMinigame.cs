using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GunMinigame : MonoBehaviour
{
    // singleton
    public static GunMinigame instance;

    public enum GunPartType {Grip, Magazine, Barrel, Handle, Stock}

    [Header("Minigame Settings")]
    [SerializeField] private bool playGameOnStart = false;
    [SerializeField] private bool randomStartingPosition = true;
    [SerializeField] private UnityEvent gameSuccessEvent;
    [SerializeField] private UnityEvent gameFailEvent;

    [Header("Open Minigame Setting")]
    [SerializeField] private GameObject minigameContainer;

    [Header("Components")]
    [SerializeField] private GunPartChecker[] checkers;
    [SerializeField] private GunPart[] gunParts;
    private List<Vector2> initialAnchors = new List<Vector2> ();

    void Awake() {
        // singleton setup
        if (instance != null && instance != this) {
            Destroy(this);
        } else {
            instance = this;
        }
    }

    //==============================Open Game==============================
    #region Open Game
    void OpenContainer() => minigameContainer.SetActive(true);
    public void CloseContainer() => minigameContainer.SetActive(false);
    #endregion
    //==============================Start Game==============================
    #region Start Game
    void Start() {
        SetInitialAnchors();
        if (playGameOnStart) StartMinigame();
        else CloseContainer();
    }

    public static void StartMinigameStatic() => instance.StartMinigame();

    public void StartMinigame() {
        // open
        OpenContainer();

        // prep
        ResetGame();
        if (randomStartingPosition) RandomGunPartPositions();
    }
    
    void SetInitialAnchors() {
        initialAnchors.Clear();

        // get all anchor positions
        for (int i = 0; i < gunParts.Length; i++) {
            initialAnchors.Add(gunParts[i].GetComponent<RectTransform>().anchoredPosition);
        }
    }

    void ResetGame() {
        foreach (GunPartChecker c in checkers) {
            c.Reset();
        }

        foreach (GunPart p in gunParts) {
            p.Reset();
        }
    }

    void RandomGunPartPositions() {
        // get all anchor positions
        List<Vector2> anchors = new List<Vector2> (initialAnchors);

        // random mixing anchor position
        foreach (GunPart p in gunParts) {
            int rand = Random.Range(0, anchors.Count);
            p.GetComponent<RectTransform>().anchoredPosition = anchors[rand];
            anchors.RemoveAt(rand);
        }
    }
    #endregion
    //==============================Check Game==============================
    #region Check Game
    public void Check() {
        // check for checkers being empty
        if (checkers.Length < 1) {
            Debug.Log("[Error] GunMinigame: no checker assigned");
            return;
        }

        // check for completion
        bool isComplete = true;
        foreach (GunPartChecker c in checkers) {
            if (c.equippedGunPart == null) {
                isComplete = false;
                break;
            }
        }
        if (!isComplete) return;

        // check for correction
        bool isCorrect = true;
        foreach (GunPartChecker c in checkers) {
            if (c.equippedGunPart.gunPartType != c.gunPartType) {
                isCorrect = false;
                break;
            }
        }
        if (isCorrect) {
            Debug.Log("[Debug] GunMinigame: done, success");
            DTRH_GameManager._instance.rodyScore++;
            gameSuccessEvent.Invoke();
        } else {
            Debug.Log("[Debug] GunMinigame: done, fail");
            gameFailEvent.Invoke();
        }
    }
    #endregion
}
