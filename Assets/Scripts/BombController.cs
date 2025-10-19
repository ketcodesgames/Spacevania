using UnityEngine;

public class BombController : MonoBehaviour
{
    [SerializeField] private float explosionDelay = .5f;
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private float bombBlastRadius = 1.5f;
    [SerializeField] private LayerMask destructibleLayer;

    void Update()
    {
        explosionDelay -= Time.deltaTime;

        if (explosionDelay <= 0)
        {
            if (explosionEffect != null)
            {
                Instantiate(explosionEffect, transform.position, Quaternion.identity);
            }

            Collider2D[] hitObjects = Physics2D.OverlapCircleAll(transform.position, bombBlastRadius, destructibleLayer);
            DestroyHitObjects(hitObjects);

            Destroy(gameObject, explosionDelay);
        }
    }
    
    void  DestroyHitObjects(Collider2D[] hitObjects)
    {
        if (hitObjects.Length > 0)
        {
            foreach (Collider2D hitObject in hitObjects)
            {
                if (hitObject != null)
                {
                    Destroy(hitObject.gameObject);
                }
            }
        }
    }
}
