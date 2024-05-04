using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float maxJumpForce = 20f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float jumpCancelMultiplier = 0.5f;
    [SerializeField] private float jumpAcceleration;

    private Rigidbody2D rb;
    private bool canJump = true;
    private bool isGrounded = false;
    private bool isOnPlatform = false;
   
    private bool isWallGlidingRight = false;
    private bool isWallGlidingLeft = false;
    private bool isWallGliding = false;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        Move(horizontalInput);
    }

    public void Move(float _horizontalInput)
    {
        float horizontalInput = _horizontalInput;
        if (isWallGliding)
        {
            if (isWallGlidingLeft || isWallGlidingRight)
            {
                horizontalInput = 0;
            }
        }

        Vector2 targetVelocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
        rb.velocity = Vector2.Lerp(rb.velocity, targetVelocity, Time.deltaTime * acceleration);

        if (isWallGliding)
        {
            rb.velocity = new Vector2(0f, rb.velocity.y);
        }
    }


    public void Jump()
    {
        if (canJump && (isGrounded || (isOnPlatform && rb.velocity.y <= 0)))
        {
            float jumpingForce = Mathf.Min(this.jumpForce + Time.deltaTime * jumpAcceleration, maxJumpForce);
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(Vector2.up * jumpingForce, ForceMode2D.Impulse);
            canJump = false;
        }
    }

    public void HandleJumpCancel()
    {
        if (!isGrounded && rb.velocity.y > 0 && !canJump)
        {
            Vector2 velocity = rb.velocity;
            velocity = new Vector2(velocity.x, velocity.y * jumpCancelMultiplier);
            rb.velocity = velocity;
        }
    }

    private void SetGrounded(bool grounded)
    {
        isGrounded = grounded;
        canJump = grounded;
    }
    
    private bool IsWall()
    {
        return isWallGlidingRight || isWallGlidingLeft;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent(out Wall wall) && !isGrounded && rb.velocity.y <= 0)
        {
            Debug.Log(rb.velocity);

            float horizontalInput = Input.GetAxis("Horizontal");
            if (Mathf.Abs(horizontalInput) > 0 && !isGrounded)
            {
                isWallGlidingRight = horizontalInput > 0;
                isWallGlidingLeft = horizontalInput < 0;
                isWallGliding = (isWallGlidingRight && isWallGlidingRight != IsWallRight()) ||
                                (isWallGlidingLeft && isWallGlidingLeft != IsWallLeft());
                SetGrounded(false);
            }
            else
            {
                isWallGliding = false;
            }
        }
    }


    private bool IsWallRight()
    {
        return isWallGlidingRight;
    }

    private bool IsWallLeft()
    {
        return isWallGlidingLeft;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent(out Wall wall))
        {
            isWallGlidingLeft = false;
            isWallGlidingRight = false;
            isWallGliding = IsWall();
        }
        
        if (collision.collider.TryGetComponent(out Platform platform))
        {
            isOnPlatform = false;
            SetGrounded(false);
        }

        if (collision.collider.TryGetComponent(out Ground ground))
        {
            SetGrounded(false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent(out Platform platform))
        {
            isOnPlatform = true;
            SetGrounded(true);
        }

        if (collision.collider.TryGetComponent(out Ground ground))
        {
            SetGrounded(true);
        }
    }
}