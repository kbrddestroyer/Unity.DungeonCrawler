using UnityEngine;

public class DynamicGUIElement : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private Vector2 initialSize;
    [SerializeField] private Vector2 boundPoints;

    private void DynamicScale()
    {
        
    }
    
    private void Update()
    {
        
    }
    
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        
        Gizmos.DrawWireCube(transform.position, new Vector3(
            canvas.pixelRect.width - boundPoints.x, canvas.pixelRect.height - boundPoints.y, 0
            ));
    }
#endif
}
