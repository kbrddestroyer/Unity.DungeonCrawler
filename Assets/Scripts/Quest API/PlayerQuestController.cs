using System.Collections.Generic;
using UnityEngine;


public class PlayerQuestController : MonoBehaviour
{
    public static PlayerQuestController Instance { get; private set; }

    [SerializeField] private List<Quest> activeQuests = new();
    [SerializeField] private QuestRegistry registry;

    public void AddQuest(Quest quest)
    {
        if (!quest || activeQuests.Contains(quest))
            return;
        
        activeQuests.Add(quest);
    }

    public bool TryComplete(Quest quest)
    {
        if (!quest || !activeQuests.Contains(quest))
            return false;
        
        return quest.TryComplete();
    }

    public void TryCompleteAll()
    {
        foreach (var quest in activeQuests)
            quest?.TryComplete();
    }

    private void OnEnable() => Instance = this;
    private void OnDisable() => Instance = null;
}
