using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyGame_DoorScript : MonoBehaviour
{
    GameObject interactIndicator;
    GameObject readyIndicator;
    public RoomType thisRoom;
    
    void Start()
    {
        interactIndicator = transform.GetChild(0).gameObject;
        readyIndicator = transform.GetChild(1).gameObject;
    }

    void OnTriggerEnter2D(Collider2D col) 
    {
        interactIndicator.SetActive(true);
        DTRH_GameManager._instance.currentRoomType = thisRoom;
    }

    void OnTriggerExit2D(Collider2D col) 
    {
        interactIndicator.SetActive(false);
        DTRH_GameManager._instance.currentRoomType = RoomType.None;
    }
}
