using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootProjectile : MonoBehaviour
{
    private new Rigidbody2D rigidbody;
    // Start is called before the first frame update
    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (transform.position.magnitude > 50.0f)
        {
            Destroy(gameObject); // Destroy the projectile if it goes too far
        }  
    }

    // Update is called once per frame
    public void Launch(Vector2 direction, float force)
    {
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
