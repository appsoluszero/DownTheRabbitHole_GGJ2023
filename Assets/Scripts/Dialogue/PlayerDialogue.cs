using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDialogue : MonoBehaviour
{
    [Header("Detection Settings")]
    public float radius = 1;
    public GameObject icon;

    // current dialogue
    Dialogue dialogue;

    //==============================Detect Dialogue==============================
    void Update() {
        if (dialogue == null) UpdateDetectDialogueTrigger();
    }

    void UpdateDetectDialogueTrigger() {
        // get closest dialogueTrigger
        DialogueTrigger closest = null;
        float dist = float.MaxValue;
        foreach (DialogueTrigger t in DialogueTrigger.activeDialogueTrigger) {
            float newDist = Vector2.Distance(this.transform.position, t.transform.position);
            if (newDist < dist) {
                dist = newDist;
                closest = t;
            }
        }

        // get condition
        
    }

    //==============================Play Dialogue==============================
    
    //==============================Gizmos==============================
    void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
