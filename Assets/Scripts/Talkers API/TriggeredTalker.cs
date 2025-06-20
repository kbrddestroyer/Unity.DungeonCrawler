using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class TriggeredTalker : Talker
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Player>())
            TriggerTextOutput();
    }
}
