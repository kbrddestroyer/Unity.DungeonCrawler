using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Quest", menuName = "Scriptable Objects/Quest")]
public class Quest : ScriptableObject
{
    [SerializeField] private string questName;
    [SerializeField, Multiline] private string description;

    [SerializeField] private IValidator validate;
    [SerializeField] private UnityEvent onQuestStarted;
    [SerializeField] private UnityEvent onQuestCompleted;
    [SerializeField] private uint uniqueID;

    public uint UniqueID { get => uniqueID; set => uniqueID = value; }

    public string QuestName => questName;
    public string Description => validate ? validate.DisplayGoal : description;
    
    public bool Started { get; private set; }
    public bool Completed { get; private set; }

    public void StartQuest()
    {
        onQuestStarted.Invoke();
        Started = true;
    }

    public bool TryComplete()
    {
        if (!validate.Validate()) return false;
        Complete();
        return true;
    }

    private void Complete()
    {
        onQuestCompleted.Invoke();
        Started = false;
        Completed = true;
    }
}
