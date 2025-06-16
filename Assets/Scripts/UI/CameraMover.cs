using System;
using Unity.VisualScripting;
using UnityEngine;

namespace UI
{
    public class CameraMover : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private Player player;

        [Header("Camera preferences")] 
        [SerializeField, Range(0f, 10f)] private float fSmoothness;
        [SerializeField, Range(0f, 5f)] private float fBoundRadius;
        [SerializeField, Range(0f, 5f)] private float fStopRadius;
        
        private bool _shouldMoveSelf;

        public GameObject FocusPoint { get; set; }

        private void Start()
        {
            FocusPoint = player.gameObject;
        }

        private void Update()
        {
            var fDistance = Vector2.Distance(FocusPoint.transform.position, transform.position);
            if (!_shouldMoveSelf)
            {
                _shouldMoveSelf = fDistance > fBoundRadius;
            }
            else
            {
                var vPrecomputed2D = Vector2.Lerp(transform.position, FocusPoint.transform.position, Time.deltaTime * fSmoothness);
                transform.position = new Vector3(vPrecomputed2D.x, vPrecomputed2D.y, transform.position.z);
                _shouldMoveSelf = fDistance > fStopRadius;
            }
        }
        
#if UNITY_EDITOR
        private void OnValidate()
        {
            if (!player)
                player = FindAnyObjectByType<Player>();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, fBoundRadius);
            
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, fStopRadius);
        }
#endif
    }
}