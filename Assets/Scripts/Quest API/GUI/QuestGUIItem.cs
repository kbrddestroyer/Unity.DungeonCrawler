using System;
using TMPro;
using UnityEngine;

public class QuestGUIItem : MonoBehaviour
{
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text description;

    private Quest _quest;
    
    public Quest AssociatedQuest
    {
        get => _quest;

        set
        {
            _quest = value;

            if (!_quest)
                return;

            title.text = _quest.QuestName;
            description.text = _quest.Description;
        }
    }
}
