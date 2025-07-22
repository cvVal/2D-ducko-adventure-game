using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleHealth : MonoBehaviour
{

    public AudioClip collectedClip; // Sound to play when health is collected

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<PlayerController>(out var player))
        {
            if (player != null && player.Health < player.maxHealth)
            {
                player.PlaySound(collectedClip); // Play the collection sound
                player.ChangeHealth(1);
                Destroy(gameObject);
            }
        }
    }
}
