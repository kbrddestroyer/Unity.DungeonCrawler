using System;
using System.Globalization;
using TMPro;
using UnityEngine;

public class PlayerGUIAggregator : MonoBehaviour
{
    public static PlayerGUIAggregator Instance { get; private set; }

    [SerializeField] private TMP_Text attack;
    [SerializeField] private TMP_Text health;

    public void SetAttackValue(float value) => attack.text = Math.Round(value).ToString(CultureInfo.CurrentCulture);
    public void SetHealthValue(float value) => health.text = Math.Round(value).ToString(CultureInfo.CurrentCulture);
    
    private void OnEnable() => Instance = this;
    private void OnDisable() => Instance = null;
}
