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

    // Update is called once per frame
    public void Launch(Vector2 direction, float force)
    {
        rigidbody.AddForce(direction * force);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(gameObject); // Destroy the projectile on collision
    }
}
