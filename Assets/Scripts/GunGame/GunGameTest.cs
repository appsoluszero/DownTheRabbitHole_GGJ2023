using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunGameTest : MonoBehaviour
{
    public void OpenGunMinigame() {
        GunMinigame.instance.StartMinigame();
    }
}
