using System.Collections;
using TMPro;
using UnityEngine;

public class Popup : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField, Range(0f, 10f)] private float lifetime;
    
    private IEnumerator ScheduleDeath()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }

    private void Start()
    {
        StartCoroutine(ScheduleDeath());
    }
    
    public void SetText(string displayText) => this.text.text = displayText;
}

