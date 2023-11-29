using UnityEngine;

public class Player : MonoBehaviour, IDamagable
{
    private CharacterController characterController;
    public float Health { get; set; } = 100f;  

    public float moveSpeed = 5f;
    public float jumpForce = 8f;
    [SerializeField] private float gravity = 20f;

    private Vector3 moveDirection;
    private bool isJumping;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        moveDirection = new Vector3(horizontal, 0f, vertical);
        moveDirection.Normalize();

        moveDirection = transform.TransformDirection(moveDirection);
        moveDirection *= moveSpeed;

        if (characterController.isGrounded)
        {
            isJumping = false;

            if (Input.GetButtonDown("Jump"))
            {
                isJumping = true;
                moveDirection.y = jumpForce;
            }
        }

        moveDirection.y -= gravity;
        characterController.Move(moveDirection * Time.deltaTime);
    }

    public void TakeDamage(float _damage)
    {
        Health -= _damage;

        if (Health <= 0)
        {
            Debug.Log("Player has been defeated!");
        }
    }
}