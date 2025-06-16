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
    [SerializeField] private LayerMask enemy;
    
    public bool InWeapon { get; private set; }
    public bool InRoll { get; set; }

    public void ToggleWeapon() => InWeapon = !InWeapon;
    public void StopRoll() => InRoll = false;

    public void Attack()
    {
        var results = Physics2D.OverlapCircleAll(transform.position, baseAttackBaseRadius, enemy);
        
        foreach (var enemyCollider in results)
        {
            var enemyScriptRef = enemyCollider.gameObject.GetComponent<EnemyBase>();

            if (enemyScriptRef)
            {
                enemyScriptRef.ApplyDamage(baseDamage);
            }
        }
    }
    
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, splashAttackBaseRadius);
    }
#endif
}
