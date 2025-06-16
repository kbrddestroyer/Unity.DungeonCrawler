using TMPro;
using UnityEngine;

[System.Serializable]
public struct TalkSequence
{
    [SerializeField] public string text;
    [SerializeField] public GameObject focusObject;
    [SerializeField] public TMP_Text textObject;
}
