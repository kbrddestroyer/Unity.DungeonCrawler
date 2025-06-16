using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(PlayerMover))]
[RequireComponent(typeof(Collider2D))]
public class Player : MonoBehaviour
{
    [Header("Basic Settings")] 
    [SerializeField, Range(0f, 10f)] private float baseDamage;

    [SerializeField, Range(0f, 10f)] private float baseHealth;
    [SerializeField, Range(0f, 10f)] private float baseAttackBaseRadius;
    [SerializeField, Range(0f, 10f)] private float baseAttackBaseOffset;
    [SerializeField, Range(0f, 10f)] private float splashAttackBaseRadius;
    
    public bool InWeapon { get; private set; }
    public bool InRoll { get; set; }

    public void ToggleWeapon() => InWeapon = !InWeapon;
    public void StopRoll() => InRoll = false;

    public void ProcessAttack()
    {
        
    }
    
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, splashAttackBaseRadius);
    }
#endif
}
