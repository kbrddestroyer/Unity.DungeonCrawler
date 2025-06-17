public interface IDamagable
{
    float Health { get; set; }

    void Die();
    void OnDeath();
    void OnDamaged(float fDamageApplied);
}
