using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerDialogue : MonoBehaviour
{
    // singleton
    public static PlayerDialogue instance;

    [Header("Detection Settings")]
    public float radius = 1;
    public GameObject icon;
    public bool iconAtTrigger = false;
    private Vector3 iconOffset;

    [Header("Dialogue Options")]
    public int day = 1;
    public bool isBad = false;

    // current dialogue
    DialogueTrigger trigger;
    Dialogue dialogue;

    // play dialogue
    Queue<DialogueText> texts;
    bool isTyping = false;
    float textLetterDelay = 0.02f;
    bool inputImpulse = false;

    // ref
    DialogueUI ui;
    
    void Start() {
        ui = DialogueUI.instance;
        if (ui == null) Debug.Log("[Error] DialogueUI instance not found!");

        ui.HideUI();

        iconOffset = icon.transform.localPosition;
    }

    //==============================Detect Dialogue==============================
    #region Detect
    void Update() {
        // if not playing dialogue update detect
        if (dialogue == null) UpdateDetectTrigger();
        
        // show icon
        if (icon != null) {
            if (trigger != null) {
                icon.SetActive(true);
                if (iconAtTrigger) {
                    icon.transform.position = trigger.gameObject.transform.position + iconOffset;
                } else {
                    icon.transform.position = transform.position + iconOffset;
                }
            }
            else icon.SetActive(false);
        }

        // if not typing, inputImpulse is false
        if (!isTyping) inputImpulse = false;
    }

    void UpdateDetectTrigger() {
        // get closest dialogueTrigger
        DialogueTrigger closest = null;
        float dist = float.MaxValue;
        foreach (DialogueTrigger t in DialogueTrigger.activeDialogueTrigger) {
            float newDist = Vector2.Distance(this.transform.position, t.transform.position);
            if (newDist > radius + t.radius) continue;
            if (newDist < dist) {
                dist = newDist;
                closest = t;
            }
        }
        trigger = closest;

        // get condition
        if (trigger == null) return;
    }
    #endregion
    //==============================Play Dialogue==============================
    #region Play
    public void OnDialogueNextInput() {
        // case already playing dialogue
        if (dialogue != null) {
            if (isTyping) inputImpulse = true;
            else StartCoroutine(NextText());
            return;
        }

        // case not playing dialogue
        // check null
        if (trigger == null) return;

        // get dialogue
        dialogue = GetDialogue(trigger, day, isBad);
        if (dialogue == null) return;

        // start dialogue
        StartDialogue();
    }

    void StartDialogue() {
        texts = new Queue<DialogueText> (dialogue.texts);

        ui.ShowUI();

        StartCoroutine(NextText());
    }

    Dialogue GetDialogue(DialogueTrigger trigger, int day, bool isBad) {
        day -= 1; //this is only for when day start with 1, so that I can start with 0
        if (trigger.dialogues.Length < 1) {
            Debug.Log($"[Error] PlayerDialogue: no dialogue for (DialogueTrigger){trigger.name} on day #{day+1}, isBad = {isBad}");
            return null;
        }
        if (!isBad) {
            return trigger.dialogues[day].dialogueGood;
        }
        return trigger.dialogues[day].dialogueBad;
    }

    IEnumerator NextText() {
        // check null
        if (texts == null) yield break;

        // reached the end
        if (texts.Count <= 0) {
            ui.HideUI();
            dialogue = null;
            yield break;
        }

        // show next
        DialogueText newText = texts.Dequeue();
        ui.nameText.text = newText.name;

        // type sentence
        yield return StartCoroutine(TypeSentence(newText.sentence, ui.sentenceText));
    }

    IEnumerator TypeSentence (string text, TMP_Text destination) {
        isTyping = true;
        destination.text = "";
        foreach (char letter in text.ToCharArray()) {
            destination.text += letter;
            yield return new WaitForSeconds(textLetterDelay);
            if (inputImpulse) { //if input is registered, instantly complete the sentense
                inputImpulse = false;
                isTyping = false;
                destination.text = text;
                break;
            }
        }
        isTyping = false;
        inputImpulse = false;
    }

    #endregion
    //==============================Gizmos==============================
    void OnDrawGizmos() {
        Gizmos.color = (trigger == null) ? Color.blue : Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
