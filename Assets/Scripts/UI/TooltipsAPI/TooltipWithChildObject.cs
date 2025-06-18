using UnityEngine;

public class TooltipWithChildObject : TooltipBase
{
    [Header("Subobject")] [SerializeField] private GameObject subobject;

    protected override void Show(bool value)
    {
        subobject.SetActive(value);
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (!subobject)
            subobject = transform.GetChild(0).gameObject;
    }
#endif
}
