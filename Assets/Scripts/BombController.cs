using UnityEngine;

public class BombController : MonoBehaviour
{
    [SerializeField] private float explosionDelay = .5f;
    [SerializeField] private GameObject explosionEffect;

    void Update()
    {
        explosionDelay -= Time.deltaTime;

        if (explosionDelay <= 0)
        {
            if (explosionEffect != null)
            {
                Instantiate(explosionEffect, transform.position, Quaternion.identity);
            }

            Destroy(gameObject, explosionDelay);
        }
    }
}
