using UnityEngine;

public class AbilityUnlock : MonoBehaviour
{
    [SerializeField] AbilitySO abilityToUnlock;
    [SerializeField] GameObject unlockEffect;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (LayerMask.LayerToName(collider.gameObject.layer) == "Player")
        {
            if (abilityToUnlock != null)
            {
                abilityToUnlock.SetEnabled(true);
                
                Instantiate(unlockEffect, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }
}
