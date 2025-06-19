using System.Collections;
using System.Linq;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class TalkerBase : MonoBehaviour
{
    public static bool GlobalLock { get; private set; }

    [Header("Settings")] 
    [SerializeField, Range(0f, 1f)] private float characterDelay;

    [SerializeField] private TalkSequence[] sequence;

    [SerializeField] private GameObject guiFX;
    
    [SerializeField] private UnityEvent onStartDisplay;
    [SerializeField] private UnityEvent onStopDisplay;
    [SerializeField] private UnityEvent onFinishDialogue;
    
    [SerializeField] private InputActionAsset mappedInput;
    
    private CameraMover _mover;
    private GameObject _startFocus;

    private bool _skip;
    
    private bool Skip {
        get
        {
            var val = _skip;
            if (_skip)
                _skip = false;
            return val;
        }
        set => _skip = value;
    }

    private void ProcessSkip(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() > 0)
            Skip = true;
    }

    private void Awake() => mappedInput["Skip"].performed += ProcessSkip;
    
    private void Start()
    {
        if (!Camera.main) return;
        
        _mover = Camera.main.GetComponent<CameraMover>();
    }
    
    private IEnumerator Output(TalkSequence text)
    {
        onStartDisplay.Invoke();

        if (_mover)
            _mover.FocusPoint = text.focusObject;
        
        yield return TalkerTextDisplayTools.DisplayTest(text, characterDelay, () => Skip, text.onCharacterDisplay);
        onStopDisplay.Invoke();
    }

    private IEnumerator OutputSequence()
    {
        GlobalLock = true;
        _startFocus = _mover.FocusPoint;
        if (guiFX)
            guiFX.SetActive(true);

        foreach (var sq in sequence)
        {
            yield return Output(sq);
            while (!Skip)
            {
                yield return new WaitForEndOfFrame();
            }
            sq.textObject.text = "";
        }
            
        if (_mover)
        {
            _mover.FocusPoint = _startFocus;
        }
        
        if (guiFX)
            guiFX.SetActive(false);

        onFinishDialogue.Invoke();
        GlobalLock = false;
    }

    public void TriggerTextOutput()
    {
        if (!GlobalLock)
            StartCoroutine(OutputSequence());
    }
}
