using UnityEngine;

public class AttackingEnemy : EnemyBase
{
    private static readonly int AttackKey = Animator.StringToHash("attack");
    
    [SerializeField] private Attack attack;
    [SerializeField] private LayerMask player;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private float CalculateDamage() => attack.Damage;
    
    public void Attack()
    {
        var result = AttackSolver.Attack(transform.position, attack, player, spriteRenderer.flipX);
        result?.ApplyDamage(CalculateDamage());
    }

    public void ProcessAttack()
    {
        animator.SetBool(AttackKey, true);
    }

    public void StopAttack()
    {
        animator.SetBool(AttackKey, false);
    }
    
#if UNITY_EDITOR
    private void OnValidate()
    {
        if (!spriteRenderer)
            spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        attack.DrawGizmo(transform.position, spriteRenderer.flipX);
    }
#endif
}
