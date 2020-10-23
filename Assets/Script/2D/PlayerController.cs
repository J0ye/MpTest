using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collision))]
public class PlayerController : NetworkBehaviour
{
    [Range(0.1f, 100f)]
    public float speed = 10f;
    [Range(0.1f, 100f)]
    public float jumpForce = 10f;

    private Rigidbody2D rb;
    private Collision col;
    private bool doubleJump = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collision>();

        if(!isLocalPlayer)
        {
            GetComponent<SpriteRenderer>().color = Color.red;
            gameObject.tag = "Enemy";
        }
    }

    private void Update()
    {
        if (!isLocalPlayer) return;

        if (Input.GetKeyDown(KeyCode.Space)) Jump();

        if (col.onGround) Grounded();
    }

    // need to use FixedUpdate for rigidbody
    void FixedUpdate()
    {
        // only let the local player control the racket.
        // don't control other player's rackets
        if (!isLocalPlayer) return;

        float h = Input.GetAxis("Horizontal");

        rb.velocity = new Vector2(h * speed, rb.velocity.y);
    }

    private void Jump()
    {
        if (!col.onGround && !doubleJump) return;
        if (!col.onGround && doubleJump) doubleJump = false;

        //Debug.Log(gameObject + " jumped");
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    private void Grounded()
    {
        doubleJump = true;
    }
}
