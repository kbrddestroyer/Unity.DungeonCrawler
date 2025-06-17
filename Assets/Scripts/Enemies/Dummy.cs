using UnityEngine;


namespace Enemies
{
    public class Dummy : EnemyBase
    {
        [Header("Requirements")] 
        [SerializeField]
        private GameObject root;

        [SerializeField] private Popup popupPrefab;
        
        public override void OnDamaged(float fDamageApplied)
        {
            var popup = Instantiate(popupPrefab, root.transform);
            popup.SetText($"{fDamageApplied}");
        }
    }
}
