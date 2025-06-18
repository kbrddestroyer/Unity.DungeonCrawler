using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class SettingsAggregator : MonoBehaviour
{
    [SerializeField] private UIDocument root;

    private Slider _master;
    private Slider _music;
    private Slider _sfx;
    
    
    private void Awake()
    {
        var settings = SettingsSerialized.Instance;
        
        _master = root.rootVisualElement.Q<Slider>("master");
        _music = root.rootVisualElement.Q<Slider>("music");
        _sfx = root.rootVisualElement.Q<Slider>("sounds");

        _master.RegisterValueChangedCallback(evt => settings.MasterAudio = evt.newValue);
        _music.RegisterValueChangedCallback(evt => settings.MusicAudio = evt.newValue);
        _sfx.RegisterValueChangedCallback(evt => settings.SfxAudio = evt.newValue);
    }

    private void OnEnable()
    {
        var instance = SettingsSerialized.Instance;
        _master.value = instance.MasterAudio;
        _music.value = instance.MusicAudio;
        _sfx.value = instance.SfxAudio;
    }

    private void OnDestroy()
    {
        Serializer.WriteData(SettingsSerialized.Instance);
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (!root)
            Debug.LogWarning($"Root component of {gameObject.name} is not set");
    }
#endif
}
