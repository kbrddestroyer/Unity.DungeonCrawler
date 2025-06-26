#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelGenerator))]
public class LevelGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var generator = (LevelGenerator) target;
 
        if (GUILayout.Button("Generate Level"))
        {
            generator.GenerateNew();
        }
    }
}
#endif