using System;
using UnityEngine;

public class person : MonoBehaviour
{
    private float moveSpeed = 5f;
    private float jampForce = 2f;

    private LayerMask groundLayer;
    private float groundCheckRadius = 0.1f;
    private Transform groundCheckPoint;

    private Rigidbody2D rb;
    private float moveInput;
    private bool isGrounded;
    private bool facingRight = true;
    private Transform detector;

    private Animator animator;

    //лазанье по лестнице
    private bool isOnLadder = false;
    private float climbSpeed = 3f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        detector = transform.GetChild(0);
        groundLayer = LayerMask.GetMask("Ground");
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        moveInput = Input.GetAxis("Horizontal");

        if (Input.GetAxis("Jump") > 0 && IsGround())
        {
            animator.SetBool("isJumping", true);
            rb.AddForce(transform.up * jampForce, ForceMode2D.Impulse);
        }
        else
        {
            animator.SetBool("isJumping", false);
        }

        // Подъём по лестнице
        if (isOnLadder)
        {
            float verticalInput = Input.GetAxisRaw("Vertical");
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, verticalInput * climbSpeed);
            animator.SetBool("climb", verticalInput != 0);
        }
        Flip();
    }

    void FixedUpdate()
    {
        //Движение персонажа
        animator.SetFloat("speed", Mathf.Abs(moveInput));
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
    }

    bool IsGround()
    {
        return Physics2D.OverlapCircle(detector.position, groundCheckRadius, groundLayer);
    }
    private void Flip()
    {
        if (moveInput == 0) return;

        if (moveInput < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    // включение и отключение гравитации

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isOnLadder = true;
            rb.gravityScale = 0f;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isOnLadder = false;
            rb.gravityScale = 1f;
        }
    }

}
