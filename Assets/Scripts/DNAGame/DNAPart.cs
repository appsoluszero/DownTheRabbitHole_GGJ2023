using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DragAndDrop))]
public class DNAPart : MonoBehaviour
{
    // equip setting
    [HideInInspector] public DNAMinigame.DNASlot equipTo;

    // part setting
    [SerializeField] private float radius;
    [HideInInspector] public int value;

    // ref
    DragAndDrop dnd;
    DNAMinigame dnaMinigame;

    void Awake() {
        dnd = GetComponent<DragAndDrop> ();
    }

    void Start() {
        dnaMinigame = DNAMinigame.instance;
        if (dnaMinigame == null) Debug.Log("[Error] DNAPart: no DnaMinigame's instance is found!");
    }

    void Update() {
        if (dnd.isDragged) UpdateDetectSlot();
    }

    // detect if there's a checkers nearby, if yes, snap to it
    void UpdateDetectSlot() {
        // get compatible
        DNAMinigame.DNASlot s = GetCompatibleSlot();

        if (s != null) {
            // snap
            dnd.isOverriden = true;
            dnd.overridePosition = s.rectTransform.anchoredPosition;

            // establish pairing
            s.dnaPart = this;
            equipTo = s;
        } else {
            dnd.isOverriden = false;

            // remove pairing
            if (equipTo != null) equipTo.dnaPart = null;
            equipTo = null;
        }

        // update point
        dnaMinigame.UpdatePoint();
    }

    DNAMinigame.DNASlot GetCompatibleSlot() {
        // get all possible slots
        List<DNAMinigame.DNASlot> slots = new List<DNAMinigame.DNASlot> (dnaMinigame.slots);
        
        // prepare to find the closest slot that's within radius
        DNAMinigame.DNASlot slot = null;
        float dist = float.MaxValue;

        // get closest slot
        foreach (DNAMinigame.DNASlot s in slots) {
            // if already equipped (that is not this), skip
            if (s.dnaPart != null && s.dnaPart != this) continue;

            float newDist = Vector2.Distance(dnd.targetAnchoredPosition, s.rectTransform.anchoredPosition);
            if (newDist < dist) {
                dist = newDist;
                slot = s;
            }
        }

        // if closest slot is furthur than radius, set to null
        if (dist > radius) slot = null;

        return slot;
    }
}
