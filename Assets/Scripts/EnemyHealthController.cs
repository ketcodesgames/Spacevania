using UnityEngine;

public class EnemyHealthController : MonoBehaviour
{
    [SerializeField] int totalHealth = 3;
    [SerializeField] GameObject deadEffect;

    public void DamageEnemy(int damageAmount)
    {
        totalHealth -= damageAmount;
        if (totalHealth <= 0)
        {
            Die();
        }
    }
    
    private void Die()
    {
        if (deadEffect != null)
        {
            Instantiate(deadEffect, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}