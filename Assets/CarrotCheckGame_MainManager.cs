using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CarrotCheckGame_MainManager : MonoBehaviour
{
    public static CarrotCheckGame_MainManager _instance;

    public List<CarrotCheckGame_CarrotPotScript> carrotPotList;

    public int toBeCompleted = 3;
    public UnityEvent gameSuccessEvent;

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

    //Start the minigame here
    [ContextMenu("Starting minigame")]
    public void SetupToStartMinigame() 
    {
        toBeCompleted = 3;
        foreach(var p in carrotPotList) {
            p.ActivateNeedAttentionMode();
        }
    }


}
