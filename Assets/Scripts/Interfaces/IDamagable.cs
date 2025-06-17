public interface IDamagable
{
    float Health { get; set; }
    float Correction => 0;

    void Die();
    void OnDeath();
    void OnDamaged(float fDamageApplied);
    void ApplyDamage(float fDamageApplied);
}
