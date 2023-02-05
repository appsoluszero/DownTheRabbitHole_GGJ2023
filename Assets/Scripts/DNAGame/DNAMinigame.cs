using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class DNAMinigame : MonoBehaviour
{
    // singleton
    public static DNAMinigame instance;

    [Header("Minigame Settings")]
    [SerializeField] private bool playGameOnStart = false;
    [SerializeField] private UnityEvent gameSuccessEvent;
    [SerializeField] private UnityEvent gameFailEvent;

    [Header("Open Minigame Setting")]
    [SerializeField] private GameObject minigameContainer;

    [Header("Components")]
    public Transform dnaPartContainer;
    public DNASlot[] slots;
    private List<DNAPart> dnaParts = new List<DNAPart> ();
    private List<Vector2> initialAnchors = new List<Vector2> ();

    [Header("Display")]
    public Slider pointSlider;

    // minigame var
    private float point = 0;

    void Awake() {
        // singleton
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
        SetDNAParts();
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
    }

    void SetInitialAnchors() {
        initialAnchors.Clear();

        // get all anchor positions
        for (int i = 0; i < dnaParts.Count; i++) {
            initialAnchors.Add(dnaParts[i].GetComponent<RectTransform>().anchoredPosition);
        }
    }

    void SetDNAParts() {
        dnaParts.Clear();
        // get all dnaParts in dnaPartContainer
        foreach (Transform t in dnaPartContainer) {
            DNAPart p = t.GetComponent<DNAPart> ();
            if (p == null) continue;
            dnaParts.Add(p);
        }
    }

    void ResetGame() {
        point = 0;
        int n = 3;

        // check num of dnaPart
        if (dnaParts.Count < n) {
            Debug.Log("[Error] DNAMinigame: not enough DNA parts");
            return;
        }
        
        // choose 3
        List<DNAPart> pool = new List<DNAPart> (dnaParts);
        List<DNAPart> chosen = new List<DNAPart> ();
        for (int i = 0; i < n; i++) {
            int rand = Random.Range(0, pool.Count);
            chosen.Add(pool[rand]);
            pool.RemoveAt(rand);
        }

        // rand value added to 100 to chosen 3
        int sum = 0;
        for (int i = 1; i < n; i++) {
            chosen[i].value = (int)Random.Range((100f/n)/2, 3*(100f/n)/2);
            sum += chosen[i].value;
        }
        chosen[0].value = Mathf.Abs(100 - sum);
        // Debug.Log(chosen[0].value + "," + chosen[1].value + "," + chosen[2].value);

        // rand value to other
        foreach (DNAPart p in pool) {
            p.value = (int)Random.Range((100f/n)/2, 2*(100f/n)/2);
        }

        foreach (DNAPart p in dnaParts) {
            p.equipTo = null;
        }

        foreach (DNASlot s in slots) {
            s.dnaPart = null;
        }

        // put dna part back to place
        for (int i = 0; i < dnaParts.Count; i++) {
            dnaParts[i].GetComponent<RectTransform> ().anchoredPosition = initialAnchors[i];
        }
    }
    #endregion
    //==============================Check Game==============================
    #region Check Game
    public void UpdatePoint() {
        point = 0;
        foreach (DNASlot s in slots) {
            if (s.dnaPart == null) continue;
            point += s.dnaPart.value;
        }
        pointSlider.value = point;
    }

    public void Check() {
        if (point >= 98) {
            Debug.Log("[Debug] DNAMinigame: done, success");
            DTRH_GameManager._instance.rodyScore++;
            gameSuccessEvent.Invoke();
        }
        /*
        else {
            Debug.Log("[Debug] DNAMinigame: done, fail");
            gameFailEvent.Invoke();
        }
        */
    }
    #endregion
    

    [System.Serializable]
    public class DNASlot {
        public RectTransform rectTransform;
        [HideInInspector] public DNAPart dnaPart;
    }
}
