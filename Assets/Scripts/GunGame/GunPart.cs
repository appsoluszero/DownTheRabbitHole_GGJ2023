using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(DragAndDrop))]
public class GunPart : MonoBehaviour
{
    // check if there's checker
    [HideInInspector] public GunPartChecker equipTo;

    // part setting
    public GunMinigame.GunPartType gunPartType;
    [SerializeField] private float radius;

    // ref
    DragAndDrop dnd;
    Image image;
    GunMinigame gunMinigame;

    void Awake() {
        dnd = GetComponent<DragAndDrop> ();
        image = GetComponent<Image> ();
    }

    void Start() {
        gunMinigame = GunMinigame.instance;
        if (gunMinigame == null) Debug.Log("[Error] GunPart: no GunMinigame's instance is found!");
    }

    void Update() {
        if (dnd.isDragged) UpdateDetectChecker();
    }

    // detect if there's a checkers nearby, if yes, snap to it
    void UpdateDetectChecker() {
        // detect
        GunPartChecker c = DetectChecker();

        if (c != null) {
            // if detected, snap
            dnd.isOverriden = true;
            dnd.overridePosition = GetOverridePosition(c);

            // establish pairing
            c.equippedGunPart = this;
            equipTo = c;
        } else {
            dnd.isOverriden = false;

            // remove pairing
            if (equipTo != null) equipTo.equippedGunPart = null;
            equipTo = null;
        }
    }

    GunPartChecker DetectChecker() {
        // prepare to find the closest checker that's within radius
        GunPartChecker checker = null;
        float dist = float.MaxValue;

        // get closest checker
        foreach (GunPartChecker c in GunPartChecker.gunPartCheckers) {
            // if already equipped with GunPart (that is not this), skip
            if (c.equippedGunPart != null && c.equippedGunPart != this) continue;
            
            // could have use SqrMagnitube, but ummmmm no
            float newDist = Vector2.Distance(dnd.targetAnchoredPosition, c.GetComponent<RectTransform> ().anchoredPosition - GetAnchorOffset(c));
            if (newDist < dist) {
                dist = newDist;
                checker = c;
            }
        }

        // if closest checker is further than radius, set to null
        if (dist > radius) checker = null;

        return checker;
    }


    Vector2 GetOverridePosition(GunPartChecker c) {
        return c.GetComponent<RectTransform> ().anchoredPosition - GetAnchorOffset(c);
    }

    Vector2 GetAnchorOffset(GunPartChecker c) {
        Vector2 offset = Vector2.zero;
        RectTransform rt = image.GetComponent<RectTransform> ();

        switch (c.snapOrientation) {
            case GunPartChecker.SnapOrientation.Top:
                offset.y = -rt.rect.height/2;
                break;
            case GunPartChecker.SnapOrientation.Bottom:
                offset.y = rt.rect.height/2;
                break;
            case GunPartChecker.SnapOrientation.Left:
                offset.x = rt.rect.width/2;
                break;
            case GunPartChecker.SnapOrientation.Right:
                offset.x = -rt.rect.width/2;
                break;
        }

        return offset;
    }

    public void Reset() {
        equipTo = null;
        dnd.isOverriden = false;
    }

    // just incase you want to check when the last part is assembled
    /*
    void OnEnable() {
        dnd.endDragEvent += GunMinigameCheck;
    }

    void OnDisable() {
        dnd.endDragEvent -= GunMinigameCheck;
    }

    void GunMinigameCheck() {
        // just incase you want to check when the last part is assembled
        gunMinigame.Check();
    }
    */
}
