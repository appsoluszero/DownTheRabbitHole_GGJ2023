using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class GunPartChecker : MonoBehaviour
{
    public static List<GunPartChecker> gunPartCheckers = new List<GunPartChecker> ();

    // checker setting
    public enum SnapOrientation {Top, Bottom, Left, Right};
    public SnapOrientation snapOrientation;
    public GunMinigame.GunPartType part;

    // public accessible image component
    [HideInInspector] public Image image;

    void Awake() {
        gunPartCheckers.Add(this);
    }
}
