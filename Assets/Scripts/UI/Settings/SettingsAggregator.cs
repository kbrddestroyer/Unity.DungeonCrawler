using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UIElements;

public class SettingsAggregator : MonoBehaviour
{
    [SerializeField] private UIDocument root;

    [SerializeField] private AudioMixer audioMixer;
    
    private Slider _master;
    private Slider _music;
    private Slider _sfx;
    
    private void OnChangedMaster(ref SettingsSerialized settings, float newValue)
    {
        settings.MasterAudio = newValue;
        audioMixer.SetFloat("master", newValue);
    }

    private void OnChangedMusic(ref SettingsSerialized settings, float newValue)
    {
        settings.MusicAudio = newValue;
        audioMixer.SetFloat("music", newValue);
    }

    private void OnChangedSfx(ref SettingsSerialized settings, float newValue)
    {
        settings.SfxAudio = newValue;
        audioMixer.SetFloat("sfx", newValue);
    }
    
    private void Awake()
    {
        var settings = SettingsSerialized.Instance;
        
        _master = root.rootVisualElement.Q<Slider>("master");
        _music = root.rootVisualElement.Q<Slider>("music");
        _sfx = root.rootVisualElement.Q<Slider>("sounds");

        _master.RegisterValueChangedCallback(evt => OnChangedMaster(ref settings, evt.newValue));
        _music.RegisterValueChangedCallback(evt => OnChangedMusic(ref settings, evt.newValue));
        _sfx.RegisterValueChangedCallback(evt => OnChangedSfx(ref settings, evt.newValue));
    }

    private void OnEnable()
    {
        var instance = SettingsSerialized.Instance;
        _master.value = instance.MasterAudio;
        _music.value = instance.MusicAudio;
        _sfx.value = instance.SfxAudio;
        
        audioMixer.SetFloat("master", instance.MasterAudio);
        audioMixer.SetFloat("music", instance.MusicAudio);
        audioMixer.SetFloat("sfx", instance.SfxAudio);
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
