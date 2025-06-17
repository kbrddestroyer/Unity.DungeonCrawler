using UnityEngine;

[System.Serializable]
public class Attack
{
    [SerializeField, Range(0f, 10f)] private float fDamage;
    [SerializeField, Range(0f, 10f)] private float fDamageDistance;
    [SerializeField, Range(0f, 10f)] private float fYDispersion;
    
    [SerializeField] private AttackType type;
    [SerializeField] private AudioClip sfx;
    
    public float Damage => fDamage;
    public float Dispersion => fYDispersion;
    public float Distance => fDamageDistance;
    public AudioClip Sfx => sfx;

    public bool ValidatePosition(Vector3 position, float correction, Vector3 playerPosition, bool flipped)
    {
        switch (type)
        {
            case AttackType.AttackForward:
                return (
                    Mathf.Abs(playerPosition.x - position.x) - correction / 2 <= fDamageDistance &&
                    Mathf.Abs(playerPosition.y - position.y) - correction / 2 <= fYDispersion / 2 &&
                    (flipped ^ (playerPosition.x > position.x))
                );
            case AttackType.AttackRange:
                return Vector3.Distance(position, playerPosition) <= fDamageDistance + correction;
            case AttackType.AttackProjectile:
                return false;
            default:
                Debug.LogWarning($"Not implemented {type} in getRootPosition");
                return false;
        }
    }

#if UNITY_EDITOR
    private Vector3 GetRootPosition(bool flipped)
    {
        switch (type)
        {
            case AttackType.AttackForward:
                return new Vector3(
                    fDamageDistance / 2 * (flipped ? -1 : 1),
                    0,
                    0
                    );
            case AttackType.AttackRange:
                return Vector3.zero;
            case AttackType.AttackProjectile:
            default:
                Debug.LogWarning($"Not implemented {type} in getRootPosition");
                return Vector3.zero;
        }
    }

    public void DrawGizmo(Vector3 position, bool flipped)
    {
        switch (type)
        {
            case AttackType.AttackForward:
                Gizmos.DrawWireCube(position + GetRootPosition(flipped), new Vector3(fDamageDistance, fYDispersion / 2, 0));
                break;
            case AttackType.AttackRange:
                Gizmos.DrawWireSphere(position, fDamageDistance);
                break;
            case AttackType.AttackProjectile:
            default:
                Debug.LogWarning($"Not implemented {type} in getRootPosition");
                return;
        }
    }
#endif
}
