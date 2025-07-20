using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Player movement
    public InputAction MoveAction;
    public float speed = 3.0f;
    private new Rigidbody2D rigidbody;
    private Vector2 move;

    // Health system
    public int maxHealth = 5;
    private int currentHealth;
    public int Health { get { return currentHealth; } }

    // Temporary invincibility
    public float timeInvincible = 2.0f;
    bool isInvincible;
    float damageCooldown;

    // Start is called before the first frame update
    void Start()
    {
        MoveAction.Enable();
        rigidbody = GetComponent<Rigidbody2D>();

        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        move = MoveAction.ReadValue<Vector2>();
        if (isInvincible)
        {
            damageCooldown -= Time.deltaTime;
            if (damageCooldown <= 0)
            {
                isInvincible = false; // Reset invincibility after cooldown
                Debug.Log("Player is no longer invincible.");
            }
        }
    }

    void FixedUpdate()
    {
        Vector2 position = (Vector2)rigidbody.position + speed * Time.fixedDeltaTime * move;
        rigidbody.MovePosition(position);
    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
           if (isInvincible)
            {
                return; // Ignore damage if invincible
            }
            isInvincible = true;
            damageCooldown = timeInvincible;

        }
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        UIManager.Instance.SetHealthValue(currentHealth / (float)maxHealth);
    }
}
