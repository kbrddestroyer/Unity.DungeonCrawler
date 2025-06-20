

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(QuestGiver))]
public class QuestTalker : TalkerBase
{
    [SerializeField] private QuestGiver questGiver;
    
    [SerializeField] private TalkSequence[] starting;
    [SerializeField] private TalkSequence[] progress;
    [SerializeField] private TalkSequence[] finishing;
    [SerializeField] private TalkSequence[] finished;

    private readonly Dictionary<QuestGiver.State, TalkSequence[]> _states = new();

    private GameObject _startFocus;
    
    private IEnumerator OutputSequence(TalkSequence[] sequence)
    {
        GlobalLock = true;
        _startFocus = Mover.FocusPoint;
        
        if (guiFX)
            guiFX.SetActive(true);

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
        
        if (guiFX)
            guiFX.SetActive(false);

        onFinishDialogue.Invoke();
        GlobalLock = false;
    }
    
    public override void TriggerTextOutput()
    {
        StartCoroutine(OutputSequence(_states[questGiver.CurrentState]));
    }
    
#if UNITY_EDITOR
    private void OnValidate()
    {
        if (!questGiver)
            questGiver = GetComponent<QuestGiver>();
        
        _states[QuestGiver.State.Starting] = starting;
        _states[QuestGiver.State.Progress] = progress;
        _states[QuestGiver.State.Finishing] = finishing;
        _states[QuestGiver.State.Finished] = finished;
    }
#endif
}
