using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public InputAction MoveAction;
    public float speed = 3.0f;
    private new Rigidbody2D rigidbody;
    private Vector2 move;

    // Start is called before the first frame update
    void Start()
    {
        MoveAction.Enable();
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        move = MoveAction.ReadValue<Vector2>();
        Debug.Log("Move: " + move);
    }

    void FixedUpdate()
    {
        Vector2 position = (Vector2)rigidbody.position + speed * Time.fixedDeltaTime * move;
        rigidbody.MovePosition(position);
    }
}
