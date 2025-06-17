using UnityEngine;

[RequireComponent(typeof(PlayerMover))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class Player : MonoBehaviour, IAttacker
{
    // Animator cached keys
    private static readonly int ID = Animator.StringToHash("attackID");

    [Header("Basic Settings")]
    [SerializeField, Range(0f, 10f)] private float baseHealth;
    [SerializeField, Range(0f, 10f)] private float timeToResetAttackID;
    [Header("Combat API")]
    [SerializeField] private Attack[] attacksByID;
    [SerializeField] private LayerMask enemy;
    [SerializeField] private Animator animator;
    
    public bool InWeapon { get; private set; }
    public bool InRoll { get; set; }

    public bool Flipped { get; set; }

    public bool LockAttack { get; set; }
    
    public void ToggleWeapon() => InWeapon = !InWeapon;
    public void StopRoll() => InRoll = false;
    
    private uint _attackID = 0;
    private float _resetAttackTime = 0;

    private uint AttackID
    {
        get => _attackID;
        set
        {
            _attackID = value % (uint) attacksByID.Length;
            animator.SetInteger(ID, (int) _attackID);

            if (value <= 0) return;
            ResetAttackCombo();
        }
    }

    private void ResetAttackCombo() => _resetAttackTime = Time.fixedTime + timeToResetAttackID;
    private float CalculateDamage() => attacksByID[AttackID].Damage;
    public void UnlockAttack() => LockAttack = false;
    
    public void Attack()
    {
        var results = Physics2D.OverlapCircleAll(transform.position, attacksByID[AttackID].Distance + attacksByID[AttackID].Dispersion, enemy);
        
        foreach (var enemyCollider in results)
        {
            var enemyScriptRef = enemyCollider.gameObject.GetComponent<EnemyBase>();

            if (!enemyScriptRef) continue;
            
            if (attacksByID[AttackID].ValidatePosition(transform.position, enemyScriptRef.Correction, enemyCollider.transform.position, Flipped))
                enemyScriptRef.ApplyDamage(CalculateDamage());
        }

        AttackID++;
    }

    private void FixedUpdate()
    {
        if (AttackID > 0 && _resetAttackTime <= Time.fixedTime)
            AttackID = 0;
    }
    
#if UNITY_EDITOR
    private void OnValidate()
    {
        if (!animator)
            animator = GetComponent<Animator>();
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        foreach (var attack in attacksByID)
        {
            attack.DrawGizmo(transform.position, Flipped);
        }
    }
#endif
}
