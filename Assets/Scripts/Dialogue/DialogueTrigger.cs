using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    // static
    public static List<DialogueTrigger> activeDialogueTrigger = new List<DialogueTrigger> ();

    [Header("Settings")]
    public float radius = 1;

    [Header("Dialogues")]
    public DialogueByDay[] dialogues;

    void OnEnable() {
        activeDialogueTrigger.Add(this);
    }

    void OnDisable() {
        activeDialogueTrigger.Remove(this);
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, radius);
    }


    [System.Serializable]
    public class DialogueByDay {
        public Dialogue dialogueGood;
        public Dialogue dialogueBad;
    }
}
