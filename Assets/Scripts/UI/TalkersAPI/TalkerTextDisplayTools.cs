using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class TalkerTextDisplayTools
{
    public delegate bool ShouldSkip();
    
    public static IEnumerator DisplayTest(TalkSequence sequence, float characterDelay, ShouldSkip skip = null, UnityEvent onCharacterDisplay = null)
    {
        skip ??= () => false;
        sequence.textObject.text = "";
        
        foreach (var c in sequence.text)
        {
            if (skip())
            {
                sequence.textObject.text = sequence.text;
                break;
            }
            sequence.textObject.text += c;
            onCharacterDisplay?.Invoke();
            yield return new WaitForSeconds(characterDelay);
        }
    }
}
