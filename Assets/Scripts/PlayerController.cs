using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float jumpForce = 10f;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] BulletController shot;
    [SerializeField] Transform shotPoint;
    
    Rigidbody2D playerRigidBody;
    Animator playerAnimator;
    Vector2 moveInput;
    bool isGrounded;

    void Start()
    {
        playerRigidBody = GetComponent<Rigidbody2D>();
        playerAnimator = FindFirstObjectByType<Animator>();
    }

    void Update()
    {
        Run();
        IsGrounded();
        FlipSprite();
    }

    void Run()
    {
        Vector2 moveVector = new Vector2(moveInput.x * moveSpeed, playerRigidBody.linearVelocity.y);
        playerRigidBody.linearVelocity = moveVector;

        bool hasHorizontalSpeed = Mathf.Abs(playerRigidBody.linearVelocity.x) > Mathf.Epsilon;
        playerAnimator.SetBool("IsRunning", hasHorizontalSpeed);
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (value.isPressed && isGrounded)
        {
            playerRigidBody.linearVelocity = new Vector2(playerRigidBody.linearVelocity.x, jumpForce);
        }
    }

    void OnAttack()
    {
        Instantiate(shot, shotPoint.position, shotPoint.rotation).SetMoveDirection(transform.localScale.x, 0f);
        playerAnimator.SetTrigger("IsShooting");
    }

    void FlipSprite()
    {
        bool hasHorizontalSpeed = Mathf.Abs(playerRigidBody.linearVelocity.x) > Mathf.Epsilon;
        if (hasHorizontalSpeed)
        {
            float lineaVelocityXSign = Mathf.Sign(playerRigidBody.linearVelocity.x);
            transform.localScale = new Vector2(lineaVelocityXSign, 1f);
        }
    }

    void IsGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, .2f, groundLayer);
        playerAnimator.SetBool("IsJumping", !isGrounded);
    }
}
