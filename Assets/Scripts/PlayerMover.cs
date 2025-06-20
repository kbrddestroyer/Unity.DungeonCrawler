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
    private static readonly int Roll = Animator.StringToHash("roll");
    private static readonly int IsUp = Animator.StringToHash("isUp");

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
    [Header("Requirements")]
    [SerializeField] private Player player;
    
    private float _fCurrentSpeed;
    
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

        player.ToggleWeapon();
        animator.SetBool(Weapon, player.InWeapon);
    }

    private void ProcessAttack(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() == 0) return;
        if (!player.InWeapon || player.LockAttack) return;
        
        player.LockAttack = true;
        animator.SetTrigger(Attack);
    }

    private void ProcessRoll(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() == 0 || player.InRoll) return;
        
        player.InRoll = true;
        animator.SetBool(Roll, player.InRoll);
    }
#endif
    
    private void Awake()
    {
#if UNITY_6000_0_OR_NEWER
        playerInputAction["Sprint"].performed += ProcessSprint;
        playerInputAction["Draw Weapon"].performed += ProcessDrawWeapon;
        playerInputAction["Attack"].performed += ProcessAttack;
        playerInputAction["Roll"].performed += ProcessRoll;
#endif
        _fCurrentSpeed = fSpeed;
    }

    private void FixedUpdate()
    {
        if (Talker.GlobalLock)
        {
            animator.SetFloat(Speed,  0);
        }
    }

    private void Update()
    {
        if (Talker.GlobalLock)
        {
            return;
        }
        
#if UNITY_6000_0_OR_NEWER
        var vMovement2D = playerInputAction["Player Move"].ReadValue<Vector2>();
#else
        var vMovement2D = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));          // Legacy input system, compatible with older versions of Unity Editor
#endif
        if (vMovement2D.x != 0)
        {
            player.Flipped = spriteRenderer.flipX = vMovement2D.x < 0;
        }

        if (vMovement2D.magnitude != 0)
        {
            animator.SetBool(UseVerticalLayout, Math.Abs(vMovement2D.y) > Math.Abs(vMovement2D.x));
            animator.SetBool(IsUp, vMovement2D.y > 0);
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
        if (!player)
            player = GetComponent<Player>();
    }    
#endif
}