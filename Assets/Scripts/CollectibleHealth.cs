using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleHealth : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<PlayerController>(out var player))
        {
            if (player != null && player.Health < player.maxHealth)
            {
                player.ChangeHealth(1);
                Destroy(gameObject);
            }
        }
    }
}
