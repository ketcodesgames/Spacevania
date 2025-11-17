using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] float bulletSpeed = 10f;
    [SerializeField] GameObject impactEffect;

    Rigidbody2D bulletRigidBody;
    Vector2 moveDirection = new Vector2(1, 0);
    int damage = 1;

    void Start()
    {
        bulletRigidBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        bulletRigidBody.linearVelocity = moveDirection * bulletSpeed;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyHealthController enemyHealth = other.GetComponent<EnemyHealthController>();
            if (enemyHealth != null)
            {
                enemyHealth.DamageEnemy(damage);
            }
        }

        Instantiate(impactEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    public void SetMoveDirection(float x, float y)
    {
        moveDirection = new Vector2(x, y);
    }
}
