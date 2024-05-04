using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float acceleration = 10f;

    [Header("Jump Settings")]
    [SerializeField] private float maxJumpForce = 20f;
    [SerializeField] private float initialJumpForce = 10f;
    [SerializeField] private float jumpCancelMultiplier = 0.5f;
    [SerializeField] private float jumpAcceleration;

    private Rigidbody2D rb;
    private bool canJump = true;
    private bool isGrounded = false;
    private bool isOnPlatform = false;
    private bool isWallGliding = false;
    private bool isWallGlidingRight = false;
    private bool isWallGlidingLeft = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        Move(horizontalInput);
    }
    private void Move(float horizontalInput)
    {
        if (!isGrounded)
        {
            if (isWallGlidingLeft && horizontalInput < 0)
            {
                horizontalInput = 4;
            }

            if (isWallGlidingRight && horizontalInput > 0)
            {
                horizontalInput = -4;
            }
        }

        Vector2 targetVelocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
        rb.velocity = Vector2.Lerp(rb.velocity, targetVelocity, Time.deltaTime * acceleration);
    }




    public void Jump()
    {
        if (canJump && (isGrounded || (isOnPlatform && rb.velocity.y <= 0)))
        {
            float jumpingForce = Mathf.Min(initialJumpForce + Time.deltaTime * jumpAcceleration, maxJumpForce);
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(Vector2.up * jumpingForce, ForceMode2D.Impulse);
            canJump = false;
        }
    }

    public void HandleJumpCancel()
    {
        if (!isGrounded && rb.velocity.y > 0 && !canJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * jumpCancelMultiplier);
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
