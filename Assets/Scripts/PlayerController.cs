using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Animation
    private Animator animator;
    private Vector2 moveDirection = new(1, 0);

    // NPC interaction
    public InputAction TalkAction;

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

    // Projectile shooting
    public GameObject projectilePrefab;
    public float projectileSpeed = 300f;

    // Audio
    private AudioSource audioSource;
    public AudioClip damageClip; // Sound to play when player takes damage

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        MoveAction.Enable();
        TalkAction.Enable();
        rigidbody = GetComponent<Rigidbody2D>();

        audioSource = GetComponent<AudioSource>();

        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        move = MoveAction.ReadValue<Vector2>();

        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            moveDirection.Set(move.x, move.y);
            moveDirection.Normalize(); // Ensure consistent speed in all directions
        }
        animator.SetFloat("Look X", moveDirection.x);
        animator.SetFloat("Look Y", moveDirection.y);
        animator.SetFloat("Speed", move.magnitude);

        if (isInvincible)
        {
            damageCooldown -= Time.deltaTime;
            if (damageCooldown <= 0)
            {
                isInvincible = false; // Reset invincibility after cooldown
                Debug.Log("Player is no longer invincible.");
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Launch(); // Launch a projectile when Space is pressed
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            FindFriend(); // Check for nearby NPCs when X is pressed
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
            // Only play sound when actually taking damage
            PlaySound(damageClip);
            isInvincible = true;
            damageCooldown = timeInvincible;
            animator.SetTrigger("Hit");
        }
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        UIManager.Instance.SetHealthValue(currentHealth / (float)maxHealth);
    }

    void Launch()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody.position + Vector2.up * 0.5f, Quaternion.identity);
        ShootProjectile projectile = projectileObject.GetComponent<ShootProjectile>();
        projectile.Launch(moveDirection, projectileSpeed);
        animator.SetTrigger("Launch");
    }

    void FindFriend()
    {
        RaycastHit2D hit = Physics2D.Raycast(
            rigidbody.position + Vector2.up * 0.2f,
            moveDirection,
            1.5f,
            LayerMask.GetMask("NPC")
        );

        if (hit.collider != null)
        {
            if (hit.collider.TryGetComponent<NPC>(out var npc))
            {
                Debug.Log("Found NPC: " + hit.collider.name);
                UIManager.Instance.DisplayNPCDialogue(npc);
            }
            else
            {
                Debug.LogWarning("Hit object doesn't have NPC component!");
            }
        }
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
