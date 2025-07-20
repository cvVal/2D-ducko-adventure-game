using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolController : MonoBehaviour
{
    public float speed = 2.0f;
    public bool isVertical; // If true, patrols vertically; otherwise, horizontally
    public float changeTime = 3.0f;
    private float timer;
    private int direction = 1; // 1 for forward, -1 for backward

    private new Rigidbody2D rigidbody;
    private Vector2 moveDirection;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        timer = changeTime;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            direction = -direction;
            timer = changeTime;
        }
    }

    void FixedUpdate()
    {
        Vector2 position = (Vector2)rigidbody.position + speed * Time.fixedDeltaTime * moveDirection;
        if (isVertical)
        {
            position.y += speed * Time.deltaTime * direction;
        }
        else
        {
            position.x += speed * Time.deltaTime * direction;
        }
        rigidbody.MovePosition(position);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<PlayerController>(out var player))
        {
            player.ChangeHealth(-1);
        }
    }
}
