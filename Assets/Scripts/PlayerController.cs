using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float jumpForce = 10f;
    [SerializeField] float dashSpeed = 25f;
    [SerializeField] float dashDuration = 0.2f;
    [SerializeField] float afterImageLifetime = 0.1f;
    [SerializeField] float afterImageSpawnInterval = 0.05f;
    [SerializeField] float dashCooldown = 1f;
    [SerializeField] float switchSpriteDuration = 0.5f;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] BulletController shot;
    [SerializeField] Transform shotPoint;
    [SerializeField] SpriteRenderer playerSpriteRenderer;
    [SerializeField] SpriteRenderer ballSpriteRenderer;
    [SerializeField] SpriteRenderer afterImageSpriteRenderer;
    [SerializeField] Color afterImageColor;
    [SerializeField] GameObject standingSprite;
    [SerializeField] GameObject ballSprite;
    [SerializeField] Animator playerAnimator;
    [SerializeField] Animator ballAnimator;

    Rigidbody2D playerRigidBody;
    Vector2 moveInput;
    bool isGrounded;
    bool canDoubleJump;
    float dashCounter = 0f;
    float afterImageCounter = 0f;
    float dashRechargeCounter = 0f;
    float switchSpriteCounter = 0f;

    void Start()
    {
        playerRigidBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Dash();
        DashCooldown();
        Run();
        IsGrounded();
        SwitchSprites();
        FlipSprite();
    }

    void Dash()
    {
        if (dashCounter > 0)
        {
            dashCounter -= Time.deltaTime;
            playerRigidBody.linearVelocity = new Vector2(transform.localScale.x * dashSpeed, playerRigidBody.linearVelocity.y);
            afterImageCounter -= Time.deltaTime;

            if (afterImageCounter <= 0f)
            {
                ShowAfterImage();
            }

            dashRechargeCounter = dashCooldown;
        }
    }

    void DashCooldown()
    {
        if (dashRechargeCounter > 0f)
        {
            dashRechargeCounter -= Time.deltaTime;
        }
    }

    void Run()
    {
        if (dashCounter > 0)
        {
            return;
        }

        Vector2 moveVector = new Vector2(moveInput.x * moveSpeed, playerRigidBody.linearVelocity.y);
        playerRigidBody.linearVelocity = moveVector;

        bool hasHorizontalSpeed = Mathf.Abs(playerRigidBody.linearVelocity.x) > Mathf.Epsilon;

        if (standingSprite.activeSelf)
        {
            playerAnimator.SetBool("IsRunning", hasHorizontalSpeed);
        }
        else
        {
            ballAnimator.SetBool("IsRolling", hasHorizontalSpeed);   
        }
    }

    void SwitchSprites()
    {
        if (moveInput.y < 0)
        {
            switchSpriteCounter -= Time.deltaTime;
            if (switchSpriteCounter < 0)
            {
                standingSprite.SetActive(false);
                ballSprite.SetActive(true);
            }
        }
        else if (moveInput.y > 0)
        {
            switchSpriteCounter -= Time.deltaTime;
            if (switchSpriteCounter < 0)
            {
                standingSprite.SetActive(true);
                ballSprite.SetActive(false);
            }
        }
        else
        {
            switchSpriteCounter = switchSpriteDuration;
        }
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (value.isPressed &&
            (isGrounded || canDoubleJump))
        {
            if (isGrounded)
            {
                canDoubleJump = true;
            }
            else
            {
                canDoubleJump = false;
                playerAnimator.SetTrigger("DoubleJump");
            }

            playerRigidBody.linearVelocity = new Vector2(playerRigidBody.linearVelocity.x, jumpForce);
        }
    }

    void OnAttack()
    {
        if (standingSprite.activeSelf)
        {
            Instantiate(shot, shotPoint.position, shotPoint.rotation).SetMoveDirection(transform.localScale.x, 0f);
            playerAnimator.SetTrigger("IsShooting");
        }
    }

    void OnDash(InputValue value)
    {
        if (value.isPressed && dashCounter <= 0f && dashRechargeCounter <= 0f)
        {
            dashCounter = dashDuration;
            ShowAfterImage();
        }
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

    void ShowAfterImage()
    {
        bool isPlayerStanding = standingSprite.activeSelf;
        Vector3 position = isPlayerStanding ? transform.position : ballSprite.transform.position;
        Sprite sprite = isPlayerStanding ? playerSpriteRenderer.sprite : ballSpriteRenderer.sprite;

        SpriteRenderer afterImage = Instantiate(afterImageSpriteRenderer, position, transform.rotation);
        afterImage.sprite = sprite;
        afterImage.transform.localScale = transform.localScale;
        afterImage.color = afterImageColor;

        Destroy(afterImage.gameObject, afterImageLifetime);

        afterImageCounter = afterImageSpawnInterval;
    }
}
