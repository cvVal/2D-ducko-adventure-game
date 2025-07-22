using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootProjectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    public float maxDistanceFromPlayer = 10.0f; // Maximum distance before destruction
    
    private new Rigidbody2D rigidbody;
    private Transform playerTransform;

    private PlayerController player;
    private AudioSource audioSource;
    
    // Start is called before the first frame update
    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        
        // Find the player in the scene
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogWarning("Player not found! Make sure player has 'Player' tag.");
        }
    }

    void Update()
    {
        // Check if player reference exists before calculating distance
        if (playerTransform != null)
        {
            float distanceFromPlayer = Vector3.Distance(transform.position, playerTransform.position);
            
            if (distanceFromPlayer > maxDistanceFromPlayer)
            {
                Destroy(gameObject); // Destroy the projectile if it's too far from player
            }
        }
        else
        {
            // Fallback: destroy after a certain time if no player reference
            if (transform.position.magnitude > 100.0f)
            {
                Destroy(gameObject);
            }
        }
    }

    // Update is called once per frame
    public void Launch(Vector2 direction, float force)
    {
        player.PlaySound(audioSource.clip); // Play the projectile sound
        rigidbody.AddForce(direction * force);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        EnemyPatrolController enemy = other.GetComponent<EnemyPatrolController>();
        if (enemy != null && enemy.CompareTag("Fixable"))
        {
            enemy.FixEnemy(); // Call the method to fix the enemy
        }
        else if (enemy != null && enemy.CompareTag("Enemy"))
        {
            Destroy(enemy.gameObject);
        }
        Destroy(gameObject); // Destroy the projectile on collision
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject); // Destroy the projectile on collision with any object      
    }
}
