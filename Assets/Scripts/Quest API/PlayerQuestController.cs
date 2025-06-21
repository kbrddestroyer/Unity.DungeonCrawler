using System.Collections.Generic;
using UnityEngine;


public class PlayerQuestController : MonoBehaviour
{
    public static PlayerQuestController Instance { get; private set; }

    [SerializeField] private List<Quest> activeQuests = new();
    [SerializeField] private QuestRegistry registry;
    [SerializeField] private GameObject questGUIPrefab;
    [SerializeField] private Transform questGuiRoot;
    
    private Dictionary<Quest, GameObject> questsGui = new();
    
    public void AddQuest(Quest quest)
    {
        if (!quest || activeQuests.Contains(quest))
            return;
        
        var questGUI = Instantiate(questGUIPrefab, questGuiRoot);
        questGUI.GetComponent<QuestGUIItem>().AssociatedQuest = quest;
        
        questsGui[quest] = questGUI;
        activeQuests.Add(quest);
    }

    public bool TryComplete(Quest quest)
    {
        if (!quest || !activeQuests.Contains(quest))
            return false;

        var completed = quest.TryComplete();
        
        if (!completed || !questsGui.TryGetValue(quest, out var questGUI)) return completed;
        Destroy(questGUI);
        return true;
    }

    public void TryCompleteAll()
    {
        foreach (var quest in activeQuests)
            quest?.TryComplete();
    }

    private void OnEnable() => Instance = this;
    private void OnDisable() => Instance = null;
    
#if UNITY_EDITOR
    private void OnValidate()
    {
        if (!questGuiRoot)
            questGuiRoot = GameObject.FindGameObjectWithTag("QuestGUIRoot").transform;
    }
#endif
}
