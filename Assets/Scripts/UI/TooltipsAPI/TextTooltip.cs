using UnityEngine;

public class TextTooltip : TooltipBase
{
    [SerializeField] private Talker talker;

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
            talker = GetComponent<Talker>();
    }
#endif
}
