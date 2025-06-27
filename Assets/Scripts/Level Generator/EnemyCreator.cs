using UnityEngine;

public class EnemyCreator : MonoBehaviour
{
    [SerializeField, Range(0f, 10f)] private float spawnRadius;
    [SerializeField, Range(0f, 10f)] private float spawnSafeZoneRadius;
    [Tooltip("Spawn random range (min - max)")]
    [SerializeField] private Vector2Int spawnCountRate;
    [SerializeField] private GameObject spawnable;
    
    protected virtual void SpawnEnemy(Vector3 position) => Instantiate(spawnable, position, Quaternion.identity);

    private Vector3 GenerateRandomPosition(int iter)
    {
        var offset = Random.Range(spawnSafeZoneRadius, spawnRadius);
        var angle = Random.Range(90 * iter, 90 * (iter + 1));
        
        return transform.position + Vector3.up * offset * Mathf.Sin(angle) + Vector3.right * offset * Mathf.Cos(angle);
    }
    
    public void SpawnAll()
    {
        var count = Random.Range(spawnCountRate.x, spawnCountRate.y);

        for (var iter = 0; iter < count; iter++)
        {
            SpawnEnemy(GenerateRandomPosition(iter));
        }
    }
    
#if UNITY_EDITOR
    private void OnValidate()
    {
        if (!spawnable)
            Debug.LogWarning($"Spawner {gameObject.name} has no spawnable prefab assigned.");
        if (spawnRadius == 0)
            Debug.LogWarning($"Spawner {gameObject.name} has no spawn radius set.");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
        
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawSphere(transform.position, spawnSafeZoneRadius);
    }
#endif
}
