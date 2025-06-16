using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [Header("Enemy Base Settings")]
    [SerializeField, Range(0f, 100f)] protected float baseHealth;
    [SerializeField] protected bool isInvincible;

    private float Health
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

    protected virtual void Die()
    {
        OnDeath();
    }

    protected virtual void OnDeath()
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

    protected virtual void OnDamaged(float fDamageApplied)
    {
        Debug.Log($"Damaged with {fDamageApplied} damage");
    }
}
