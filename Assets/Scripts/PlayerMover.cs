using System;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(Animator))]
public class PlayerMover : MonoBehaviour
{
    private static readonly int Speed = Animator.StringToHash("speed");
    
#if UNITY_6000_0_OR_NEWER
    [Header("Input System")]
    [SerializeField] private InputAction playerInputAction;
#endif
    [Header("Preferences")]
    [SerializeField, Range(0f, 10f)] private float fSpeed = 1.0f;
    [Header("Animation")]
    [SerializeField] private Animator animator;

    #if UNITY_6000_0_OR_NEWER
    private void OnEnable() => playerInputAction.Enable();
    private void OnDisable() => playerInputAction.Disable();
    #endif
    
    private void Awake()
    {
        #if UNITY_6000_0_OR_NEWER
        #endif
    }

    private void Update()
    {
#if UNITY_6000_0_OR_NEWER
        var vMovement2D = playerInputAction.ReadValue<Vector2>();
#else
        var vMovement2D = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));          // Legacy input system, compatible with older versions of Unity Editor
#endif
        
        transform.Translate(vMovement2D * (fSpeed * Time.deltaTime), Space.World);
        animator.SetFloat(Speed,  vMovement2D.magnitude);
    }
    
#if UNITY_EDITOR
    private void OnValidate()
    {
        if (!animator) 
            animator = GetComponent<Animator>();
    }    
#endif
}