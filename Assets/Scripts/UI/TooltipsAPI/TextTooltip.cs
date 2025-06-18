using UnityEngine;

public class TextTooltip : TooltipBase
{
    [SerializeField] private TalkerBase talker;

    protected override void Show(bool value)
    {
        if (value)
        {
            talker.TriggerTextOutput();
        }
    }
    
#if UNITY_EDITOR
    private void OnValidate()
    {
        if (!talker)
            talker = GetComponent<TalkerBase>();
    }
#endif
}
