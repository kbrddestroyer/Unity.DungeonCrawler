using System;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerMover : MonoBehaviour
{
    // Animator pre-hashed keys
    private static readonly int Speed = Animator.StringToHash("speed");
    private static readonly int IsRunning = Animator.StringToHash("isRunning");
    private static readonly int UseVerticalLayout = Animator.StringToHash("useVerticalLayout");
    private static readonly int Weapon = Animator.StringToHash("inWeaponMode");
    private static readonly int Attack = Animator.StringToHash("attack");

#if UNITY_6000_0_OR_NEWER
    [Header("Input System")]
    [SerializeField] private InputActionAsset playerInputAction;
#endif
    [Header("Preferences")]
    [SerializeField, Range(0f, 10f)] private float fSpeed = 1.0f;
    [SerializeField, Range(0f, 10f)] private float fRunSpeed = 1.0f;
    [Header("Animation")]
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    
    private float _fCurrentSpeed;
    private bool _weaponEnabled;
    
    #if UNITY_6000_0_OR_NEWER
    private void OnEnable() => playerInputAction.Enable();
    private void OnDisable() => playerInputAction.Disable();
    #endif
    
#if UNITY_6000_0_OR_NEWER
    private void ProcessSprint(InputAction.CallbackContext context)
    {
        var isInSprintState = context.ReadValue<float>() > 0;
        _fCurrentSpeed = isInSprintState ? fRunSpeed : fSpeed;
        
        animator.SetBool(IsRunning, isInSprintState);
    }

    private void ProcessDrawWeapon(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() == 0) return;
        _weaponEnabled = !_weaponEnabled;
        animator.SetBool(Weapon, _weaponEnabled);
        
        
        Debug.Log(_weaponEnabled);
    }

    private void ProcessAttack(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() == 0) return;
        
        animator.SetTrigger(Attack);
    }
#endif
    
    private void Awake()
    {
#if UNITY_6000_0_OR_NEWER
        playerInputAction["Sprint"].performed += ProcessSprint;
        playerInputAction["Draw Weapon"].performed += ProcessDrawWeapon;
        playerInputAction["Attack"].performed += ProcessAttack;
#endif
        _fCurrentSpeed = fSpeed;
    }

    private void Update()
    {
#if UNITY_6000_0_OR_NEWER
        var vMovement2D = playerInputAction["Player Move"].ReadValue<Vector2>();
#else
        var vMovement2D = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));          // Legacy input system, compatible with older versions of Unity Editor
#endif
        if (vMovement2D.x != 0)
        {
            spriteRenderer.flipX = vMovement2D.x < 0;
        }

        if (vMovement2D.magnitude != 0)
        {
            animator.SetBool(UseVerticalLayout, vMovement2D.y > 0);
        }
        
        transform.Translate(vMovement2D * (_fCurrentSpeed * Time.deltaTime), Space.World);
        animator.SetFloat(Speed,  vMovement2D.magnitude);
    }
    
#if UNITY_EDITOR
    private void OnValidate()
    {
        if (!animator) 
            animator = GetComponent<Animator>();
        if (!spriteRenderer)
            spriteRenderer = GetComponent<SpriteRenderer>();
    }    
#endif
}