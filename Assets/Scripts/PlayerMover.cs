using System;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMover : MonoBehaviour
{
#if UNITY_6000_0_OR_NEWER
    [Header("Input System")]
    [SerializeField] private InputAction playerInputAction;
#endif
    [Header("Preferences")]
    [SerializeField, Range(0f, 10f)] private float fSpeed = 1.0f;

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
    }
}