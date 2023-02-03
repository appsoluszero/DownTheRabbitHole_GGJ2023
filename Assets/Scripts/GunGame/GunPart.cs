using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(DragAndDrop))]
public class GunPart : MonoBehaviour
{
    // check if there's checker
    bool designated = false;

    // ref
    DragAndDrop dnd;
    Image image;

    // part setting
    public GunMinigame.GunPartType gunPart;
    [SerializeField] private float radius;
    [SerializeField] private Vector2 anchorOffset;

    void Awake() {
        dnd = GetComponent<DragAndDrop> ();
        image = GetComponent<Image> ();
    }

    void Update() {
        if (dnd.isDragged) UpdateDetectChecker();
    }

    // detect if there's checker near by, if yes, snap to it
    void UpdateDetectChecker() {
        // detect
        GunPartChecker c = DetectChecker();

        if (c != null) {
            // if detected, snap
            dnd.isOverriden = true;
            dnd.overridePosition = GetOverridePosition(c);

            designated = true;
        } else {
            dnd.isOverriden = false;

            designated = false;
        }
    }

    GunPartChecker DetectChecker() {
        // prepare to find the closest checker that's within radius
        GunPartChecker checker = null;
        float dist = float.MaxValue;

        // get closest checker
        foreach (GunPartChecker c in GunPartChecker.gunPartCheckers) {
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
        // Rect image1rect = this.transform.GetComponent<RectTransform>().rect;
        // Rect image2rect = c.transform.GetComponent<RectTransform>().rect;
        // RectTransform image1rt = this.transform.GetComponent<RectTransform>();
        // RectTransform image2rt = c.transform.GetComponent<RectTransform>();

        // switch (c.snapOrientation) {
        //     case GunPartChecker.SnapOrientation.Top:
        //         c.gameObject.
        //         break;
        //     case GunPartChecker.SnapOrientation.Bottom:

        //         break;
        //     case GunPartChecker.SnapOrientation.Left:

        //         break;
        //     case GunPartChecker.SnapOrientation.Right:

        //         break;
        // }

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
}
