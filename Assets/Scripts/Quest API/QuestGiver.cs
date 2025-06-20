using System;
using UnityEngine;


public class QuestGiver : MonoBehaviour
{
    [SerializeField] private Quest quest;

    public enum State
    {
        Starting,
        Progress,
        Finishing,
        Finished
    }
    
    [SerializeField] private State state;
    public State CurrentState => state;

    public void Process()
    {
        switch (state)
        {
            case State.Starting:
                GiveQuest();
                break;
            case State.Progress:
                TryComplete();
                break;
            case State.Finishing:
                Complete();
                break;
            case State.Finished:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    private void GiveQuest()
    {
        PlayerQuestController.Instance.AddQuest(quest);
        state =  State.Progress;
    }

    private void TryComplete()
    {
        state = PlayerQuestController.Instance.TryComplete(quest) ? State.Finishing : State.Progress;
    }

    private void Complete()
    {
        if (state == State.Finishing)
            state = State.Finished;
    }
}
