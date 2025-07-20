using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardZone : MonoBehaviour
{
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.TryGetComponent<PlayerController>(out var player))
        {
            if (player != null)
            {
                player.ChangeHealth(-1);
                Debug.Log("Player entered a hazard zone. Current Health: " + player.Health);
            }
        }
    }
}
