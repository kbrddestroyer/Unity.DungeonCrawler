using System.Collections;
using UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public abstract class TalkerBase : MonoBehaviour
{    
    public static bool GlobalLock { get; protected set; }

    [Header("Settings")] 
    [SerializeField, Range(0f, 1f)] private float characterDelay;
    [SerializeField] protected GameObject guiFX;

    [SerializeField] protected UnityEvent onStartDisplay;
    [SerializeField] protected UnityEvent onStopDisplay;
    [SerializeField] protected UnityEvent onFinishDialogue;

    [SerializeField] protected InputActionAsset mappedInput;

    protected CameraMover Mover;

    private bool _skip;

    protected bool Skip {
        get
        {
            var val = _skip;
            if (_skip)
                _skip = false;
            return val;
        }
        private set => _skip = value;
    }

    private void ProcessSkip(InputAction.CallbackContext context)
    {
        if (context.ReadValueAsButton())
            Skip = true;
    }

    private void Awake() => mappedInput["Skip"].performed += ProcessSkip;

    private void Start()
    {
        if (!Camera.main) return;
    
        Mover = Camera.main.GetComponent<CameraMover>();
    }

    protected IEnumerator Output(TalkSequence text)
    {
        onStartDisplay.Invoke();
        _skip = false;

        if (Mover)
            Mover.FocusPoint = text.focusObject;
    
        yield return TalkerTextDisplayTools.DisplayTest(text, characterDelay, () => Skip, text.onCharacterDisplay);

        onStopDisplay.Invoke();
    }

    public abstract void TriggerTextOutput();
    
#if UNITY_EDITOR
    protected virtual void OnValidate()
    {
        Debug.Log("Validate!");
        
        if (!guiFX)
            guiFX = GameObject.FindGameObjectWithTag("Bounds");
    }    
#endif
}
