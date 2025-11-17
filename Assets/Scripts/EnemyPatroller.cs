using UnityEngine;

public class EnemyPatroller : MonoBehaviour
{
    [SerializeField] Transform[] patrolPoints;
    [SerializeField] int currentPointIndex = 0;
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float waitTimeAtPoint = 1f;
    [SerializeField] float jumpForce = 10f;
    [SerializeField] Animator animator;

    private Rigidbody2D rb;
    private float waitCounter;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();   
        waitCounter = waitTimeAtPoint;

        foreach (Transform point in patrolPoints)
        {
            point.parent = null;
        }
    }

    void Update()
    {
        if (Mathf.Abs(transform.position.x - patrolPoints[currentPointIndex].position.x) > 0.2f)
        {
            if (transform.position.x < patrolPoints[currentPointIndex].position.x)
            {
                rb.linearVelocity = new Vector2(moveSpeed, rb.linearVelocity.y);
                transform.localScale = new Vector3(-1f, 1f, 1f);
            }
            else
            {
                rb.linearVelocity = new Vector2(-moveSpeed, rb.linearVelocity.y);
                transform.localScale = Vector3.one;
            }

            if (transform.position.y < patrolPoints[currentPointIndex].position.y - 0.5f && rb.linearVelocity.y < .1f)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            }
        }
        else
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            waitCounter -= Time.deltaTime;

            if (waitCounter <= 0f)
            {
                waitCounter = waitTimeAtPoint;

                currentPointIndex++;
                if (currentPointIndex >= patrolPoints.Length)
                {
                    currentPointIndex = 0;
                }
            }
        }

        animator.SetFloat("speed", Mathf.Abs(rb.linearVelocity.x));
    }
}
