using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class EnemyBase : MonoBehaviour, IDamagable
{
    [Header("Enemy Base Settings")]
    [SerializeField, Range(0f, 100f)] protected float baseHealth;
    [SerializeField, Range(0f, 10f)] protected float correction;
    [SerializeField] protected bool isInvincible;
    [SerializeField] private UnityEvent onDeathEvent; 
    
    public float Correction => correction;
    
    
    public float Health
    {
        get => baseHealth;
        set
        {
            baseHealth = (value < baseHealth) ? value : 0;
            if (baseHealth == 0)
                Die();
        }
    }

    protected bool IsDead => Health <= 0;

    public void Die()
    {
        onDeathEvent.Invoke();
        OnDeath();
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
        Debug.Log($"Damaged with {fDamageApplied} damage");
    }
    
#if UNITY_EDITOR
    private void OnValidate()
    {
        var layer = LayerMask.NameToLayer("Enemy");
        
        if (gameObject.layer != layer)
            gameObject.layer = layer;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, correction);
    }
#endif
}
