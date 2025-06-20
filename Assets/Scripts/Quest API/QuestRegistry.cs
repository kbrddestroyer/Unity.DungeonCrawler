using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestRegistry", menuName = "Scriptable Objects/QuestRegistry")]
public class QuestRegistry : ScriptableObject
{
    [SerializeField] private List<Quest> quests = new();

    public int Length => quests.Count;
    public Quest this[int index] => quests[index];
    
#if UNITY_EDITOR
    private void OnValidate()
    {
        foreach (var quest in quests)
            quest.UniqueID = (uint) quests.IndexOf(quest);
    }
#endif
}
