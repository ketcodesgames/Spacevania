using UnityEngine;

public class DestroyOverTime : MonoBehaviour
{
    [SerializeField] private float lifetime = 2f;
    void Start()
    {
        Destroy(gameObject, lifetime);
    }
}
