using TMPro;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public struct TalkSequence
{
    [SerializeField] public string text;
    [SerializeField] public GameObject focusObject;
    [SerializeField] public TMP_Text textObject;
    [SerializeField] public UnityEvent onCharacterDisplay;
    [SerializeField] public AudioClip audioClip;
}
