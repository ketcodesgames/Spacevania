using UnityEngine;

[CreateAssetMenu(fileName = "AbilitySO", menuName = "Scriptable Objects/AbilitySO")]
public class AbilitySO : ScriptableObject
{
    [SerializeField] string abilityName;
    [SerializeField] float abilityMagnitude;
    [SerializeField] bool isEnabled;

    public string AbilityName => abilityName;
    public float AbilityMagnitude => abilityMagnitude;
    public bool IsEnabled => isEnabled;

    public void SetEnabled(bool value)
    {
        isEnabled = value;
    }
}
