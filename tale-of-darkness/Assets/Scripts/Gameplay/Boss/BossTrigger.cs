using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    public event EventHandler OnPlayerEnterTrigger;

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            OnPlayerEnterTrigger?.Invoke(this, EventArgs.Empty);
        }
    }
}
