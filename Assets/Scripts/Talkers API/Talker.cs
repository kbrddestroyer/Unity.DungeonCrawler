using System.Collections;
using UnityEngine;

public class Talker : TalkerBase
{
    [SerializeField] private TalkSequence[] sequence;
    
    private GameObject _startFocus;

    private IEnumerator OutputSequence()
    {
        GlobalLock = true;
        _startFocus = Mover.FocusPoint;
        
        guiFX?.SetActive(true);

        foreach (var sq in sequence)
        {
            yield return Output(sq);
            while (!Skip)
            {
                yield return new WaitForEndOfFrame();
            }
            sq.textObject.text = "";
        }
            
        if (Mover)
        {
            Mover.FocusPoint = _startFocus;
        }
        
        guiFX?.SetActive(false);

        onFinishDialogue.Invoke();
        GlobalLock = false;
    }

    public override void TriggerTextOutput()
    {
        if (!GlobalLock)
            StartCoroutine(OutputSequence());
    }
    
#if UNITY_EDITOR
    private new void OnValidate()
    {
        if (!guiFX)
            guiFX = GameObject.FindGameObjectWithTag("Bounds");
    }
#endif
}
