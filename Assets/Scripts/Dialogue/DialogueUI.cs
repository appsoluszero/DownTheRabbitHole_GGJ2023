using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    // singleton
    public static DialogueUI instance;

    [SerializeField] private Transform dialoguePanel;
    public TMP_Text nameText;
    public TMP_Text sentenceText;

    void Awake() {
        // singleton
        if (instance != null && instance != this) {
            Destroy(this);
        } else {
            instance = this;
        }
    }

    public void ShowUI() => dialoguePanel.gameObject.SetActive(true);
    public void HideUI() => dialoguePanel.gameObject.SetActive(false);
}
