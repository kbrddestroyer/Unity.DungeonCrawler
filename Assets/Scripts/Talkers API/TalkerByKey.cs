using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(TalkerBase))]
public class TalkerByKey : MonoBehaviour
{
    [SerializeField] private TalkerBase talkerRef;
    [SerializeField] private InputActionAsset playerInput;

    private bool _canEnter;
    
    private void ProcessTriggerKey(InputAction.CallbackContext context)
    {
        if (!context.ReadValueAsButton()) return;
     
        if (_canEnter)
        {
            talkerRef.TriggerTextOutput();
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Player>())
        {
            _canEnter = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<Player>())
        {
            _canEnter = false;
        }
    }

    private void Start() => playerInput["Interact"].performed += ProcessTriggerKey;
    
#if UNITY_EDITOR
    private void OnValidate()
    {
        if (talkerRef)
            talkerRef = GetComponent<TalkerBase>();
    }
#endif
}
