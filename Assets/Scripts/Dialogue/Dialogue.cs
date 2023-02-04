using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue", menuName = "ScriptableObjects/Dialogue")]
public class Dialogue : ScriptableObject
{
    public DialogueText[] texts;
}

[System.Serializable]
public class DialogueText {
    public string name;

    [TextArea(3, 10)]
    public string sentence = "";
}

