using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarrotCheckGame_MainManager : MonoBehaviour
{
    public List<CarrotCheckGame_CarrotPotScript> carrotPotList;

    //Start the minigame here
    [ContextMenu("Starting minigame")]
    public void SetupToStartMinigame() 
    {
        foreach(var p in carrotPotList) {
            p.ActivateNeedAttentionMode();
        }
    }
}
