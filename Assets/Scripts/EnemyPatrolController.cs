using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolController : MonoBehaviour
{
    private Animator animator;

    public float speed = 2.0f;
    public bool isVertical; // If true, patrols vertically; otherwise, horizontally
    public float changeTime = 3.0f;

    [Header("Random Axis Change Settings")]
    public bool enableRandomAxisChange = false;
    [Range(0f, 1f)]
    public float axisChangeChance = 0.3f; // 30% chance to change axis
    public float minAxisChangeTime = 2.0f;
    public float maxAxisChangeTime = 8.0f;

    [Header("Collision Detection")]
    public float collisionCheckDistance = 0.5f; // How far ahead to check for collisions

    private float timer;
    private float axisChangeTimer;
    private int direction = 1; // 1 for forward, -1 for backward

    private new Rigidbody2D rigidbody;
    private Vector2 moveDirection;

    bool isBroken = true; // Flag to indicate if the enemy is broken

    private AudioSource audioSource;

    public ParticleSystem smokeEffect; // Particle effect for when the enemy is broken

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();

        timer = changeTime;

        // Initialize random axis change timer if enabled
        if (enableRandomAxisChange)
        {
            axisChangeTimer = Random.Range(minAxisChangeTime, maxAxisChangeTime);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Handle direction change timer
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            direction = -direction;
            timer = changeTime;
        }

        // Handle random axis change if enabled
        if (enableRandomAxisChange)
        {
            axisChangeTimer -= Time.deltaTime;
            if (axisChangeTimer <= 0)
            {
                // Check if we should change axis based on chance
                if (Random.Range(0f, 1f) <= axisChangeChance)
                {
                    isVertical = !isVertical;
                    Debug.Log($"Enemy changed axis to: {(isVertical ? "Vertical" : "Horizontal")}");
                }

                // Reset the axis change timer
                axisChangeTimer = Random.Range(minAxisChangeTime, maxAxisChangeTime);
            }
        }
    }

    void FixedUpdate()
    {
        if (!isBroken) return; // If the enemy is not broken, do not move

        // Check for collision ahead before moving
        Vector2 currentPosition = rigidbody.position;
        Vector2 moveDirection = Vector2.zero;

        if (isVertical)
        {
            moveDirection = Vector2.up * direction;
            animator.SetFloat("MoveX", 0);
            animator.SetFloat("MoveY", direction);
        }
        else
        {
            moveDirection = Vector2.right * direction;
            animator.SetFloat("MoveX", direction);
            animator.SetFloat("MoveY", 0);
        }

        // Use a slightly offset start position to avoid self-collision
        Vector2 rayStart = currentPosition + moveDirection * 0.1f;

        // Raycast to check for collision (exclude self)
        RaycastHit2D hit = Physics2D.Raycast(
            rayStart,
            moveDirection,
            collisionCheckDistance,
            LayerMask.GetMask("Environment", "Enemy", "Fixable")
        );

        // Make sure we didn't hit ourselves
        if (hit.collider != null && hit.collider.gameObject != gameObject)
        {
            // Hit something! Change direction immediately
            direction = -direction;
            Debug.Log($"Enemy hit {hit.collider.name}, changing direction!");

            // Optional: Also change axis if random axis change is enabled
            if (enableRandomAxisChange && Random.Range(0f, 1f) <= 0.5f) // 50% chance on collision
            {
                isVertical = !isVertical;
                Debug.Log("Enemy also changed axis due to collision!");
            }
        }
        else
        {
            // Safe to move
            Vector2 position = currentPosition + speed * Time.fixedDeltaTime * moveDirection;
            rigidbody.MovePosition(position);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<PlayerController>(out var player))
        {
            player.ChangeHealth(-1);
        }
    }

    public void FixEnemy()
    {
        isBroken = false;
        rigidbody.simulated = false;
        animator.SetTrigger("Fixed");
        audioSource.Stop();
        smokeEffect.Stop();
    }
}
