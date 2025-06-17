using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AttackSolver
{
    public static IDamagable Attack(Vector3 position, Attack attackType, LayerMask layerMask, bool flipped)
    {
        var result = Physics2D.OverlapCircle(position, attackType.Distance + attackType.Dispersion, layerMask);

        if (!result)
            return null;
        
        var enemyScriptRef = result.GetComponent<IDamagable>();
        
        if (enemyScriptRef == null)
            return null;
        
        return attackType.ValidatePosition(position, enemyScriptRef.Correction, result.transform.position, flipped) ? enemyScriptRef : null;
    }
    public static List<IDamagable> AttackAll(Vector3 position, Attack attackType, LayerMask layerMask, bool flipped)
    {
        var results = Physics2D.OverlapCircleAll(position, attackType.Distance + attackType.Dispersion, layerMask);
        return (
            from enemyCollider in results 
            let enemyScriptRef = enemyCollider.gameObject.GetComponent<IDamagable>() 
            where enemyScriptRef != null 
            where attackType.ValidatePosition(position, enemyScriptRef.Correction, enemyCollider.transform.position, flipped) 
            select enemyScriptRef
            ).ToList();
    }
}
