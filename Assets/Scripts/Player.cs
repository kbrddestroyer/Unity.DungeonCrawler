using UnityEngine;

[RequireComponent(typeof(PlayerMover))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class Player : MonoBehaviour, IAttacker, IDamagable
{
    // Animator cached keys
    private static readonly int ID = Animator.StringToHash("attackID");
    private static readonly int Damage = Animator.StringToHash("damage");

    [Header("Basic Settings")]
    [SerializeField, Range(0f, 10f)] private float baseHealth;
    [SerializeField, Range(0f, 10f)] private float timeToResetAttackID;
    [Header("Combat API")]
    [SerializeField] private Attack[] attacksByID;
    [SerializeField] private LayerMask enemy;
    [SerializeField] private Animator animator;
    [SerializeField] private Animator guiFx;
    [SerializeField] private AudioSource audioSource;

    public FloatDynamicProperty HealthMul { get; private set; }
    public FloatDynamicProperty DamageMul { get; private set; }

    public float Health 
    { 
        get => HealthMul.Value;
        set
        {
            HealthMul.Value = (value > 0) ? value : 0;
            if (HealthMul.Value == 0)
                Die();
        }
    }

    public void Awake()
    {
        HealthMul = new FloatDynamicProperty(baseHealth, PlayerGUIAggregator.Instance.SetHealthValue);
        DamageMul = new FloatDynamicProperty(1, PlayerGUIAggregator.Instance.SetAttackValue);
    }
    
    public void Die()
    {
        OnDeath();
    }

    public void OnDeath()
    {
    }

    public void OnDamaged(float fDamageApplied)
    {
    }

    public void ApplyDamage(float fDamageApplied)
    {
        Health -= fDamageApplied;
        guiFx?.SetTrigger(Damage);
        
        OnDamaged(fDamageApplied);
    }
    
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
    private float CalculateDamage() => attacksByID[AttackID].Damage * DamageMul.Value;
    public void UnlockAttack() => LockAttack = false;
    
    public void Attack()
    {
        audioSource.PlayOneShot(attacksByID[AttackID].Sfx);
        
        var result = AttackSolver.AttackAll(transform.position, attacksByID[AttackID], enemy, Flipped);

        foreach (var enemyScriptRef in result)
        {
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
        
        if (!guiFx)
            guiFx = GameObject.FindGameObjectWithTag("GUIEffect")?.GetComponent<Animator>();
        
        if (!audioSource)
            audioSource = GetComponent<AudioSource>();
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
