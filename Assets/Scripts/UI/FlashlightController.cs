using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI
{
    public class FlashlightController : MonoBehaviour
    {
        [SerializeField] private InputActionAsset inputActionAsset; 
        
        private Camera _mainCamera;

        private void Start()
        {
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            if (!_mainCamera)
                return;
            
            var mousePosition = inputActionAsset["Flashlight Mouse"].ReadValue<Vector2>();
            var direction = mousePosition - (Vector2) _mainCamera.WorldToScreenPoint(transform.position);
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
            
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
}