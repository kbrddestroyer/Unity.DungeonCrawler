using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class EnemyBase : MonoBehaviour, IDamagable
{
    private static readonly int Damage = Animator.StringToHash("hit");
    private static readonly int Death = Animator.StringToHash("death");

    [Header("Enemy Base Settings")]
    [SerializeField, Range(0f, 100f)] protected float baseHealth;
    [SerializeField, Range(0f, 10f)] protected float correction;
    [SerializeField] protected bool isInvincible;
    [SerializeField] private UnityEvent onDeathEvent; 
    [Header("Dependencies")]
    [SerializeField] protected Animator animator;
    [SerializeField] private InventoryItemData bestiaryRecord;
    [SerializeField] private Inventory bestiary;
    
    public float Correction => correction;
    
    public float Health
    {
        get => baseHealth;
        set
        {
            baseHealth = (value > 0) ? value : 0;
            if (baseHealth == 0)
                Die();
        }
    }

    protected bool IsDead => Health <= 0;

    public void Die()
    {
        onDeathEvent.Invoke();
        animator.SetTrigger(Death);
        bestiary?.AddItem(bestiaryRecord);
    }
    
    public virtual void OnDeath()
    {
        Destroy(gameObject);
    }
    
    public void ApplyDamage(float fDamageApplied)
    {
        OnDamaged(fDamageApplied);
        
        if (isInvincible)
            return;
        
        Health -= fDamageApplied;
    }

    public virtual void OnDamaged(float fDamageApplied)
    {
        if (IsDead) return;
        
        animator.SetTrigger(Damage);
        Debug.Log($"Damaged with {fDamageApplied} damage");
    }
    
#if UNITY_EDITOR
    private void OnValidate()
    {
        var layer = LayerMask.NameToLayer("Enemy");
        
        if (gameObject.layer != layer)
            gameObject.layer = layer; 
        
        if (!animator)
            animator = GetComponent<Animator>();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, correction);
    }
#endif
}
