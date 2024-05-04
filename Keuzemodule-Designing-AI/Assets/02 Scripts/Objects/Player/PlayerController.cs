using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode shootKey = KeyCode.Mouse0;

    private Animator animator;
    private PlayerMovement playerMovement;
    private PlayerShooting playerShooting;
    private PlayerHealth playerHealth;
    
    //beter animation int to string
    private static readonly int walking = Animator.StringToHash("isWalking");
    private static readonly int shoot = Animator.StringToHash("Shoot");
    private static readonly int isWalkingShoot = Animator.StringToHash("isWalkingShoot");

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerShooting = GetComponent<PlayerShooting>();
        playerHealth = GetComponent<PlayerHealth>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        HandleInput();
        UpdateAnimation();
    }

    private void HandleInput()
    {
        

        if (Input.GetKeyDown(jumpKey))
        {
            playerMovement.Jump();
        }

        if (Input.GetKeyUp(jumpKey))
        {
            playerMovement.HandleJumpCancel();
        }

        playerShooting.SetIsShooting(Input.GetKey(shootKey));
    }

    private void UpdateAnimation()
    {
        bool isWalking = Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f;
        bool isShooting = Input.GetKey(shootKey);

        if (isWalking && isShooting)
            animator.SetBool(isWalkingShoot, true);
        else
            animator.SetBool(isWalkingShoot, false);
    }
}