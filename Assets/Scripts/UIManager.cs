using UnityEngine;
using TMPro;
using System.Collections;

public class UIManager : MonoBehaviour
{
    [Header("Unlock Message Settings")]
    [SerializeField] CanvasGroup unlockMessageGroup;
    [SerializeField] TMP_Text messageText;
    [SerializeField] float fadeDuration = 0.5f;
    [SerializeField] float visibleDuration = 2f;

    private Coroutine currentRoutine;

    public void ShowUnlockMessage(string abilityName)
    {
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(ShowMessageRoutine(abilityName));
    }

    private IEnumerator ShowMessageRoutine(string abilityName)
    {
        messageText.text = $"Unlocked: {abilityName}";

        // Fade In
        yield return StartCoroutine(FadeCanvasGroup(unlockMessageGroup, 0f, 1f, fadeDuration));

        // Stay visible
        yield return new WaitForSeconds(visibleDuration);

        // Fade Out
        yield return StartCoroutine(FadeCanvasGroup(unlockMessageGroup, 1f, 0f, fadeDuration));
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup group, float from, float to, float duration)
    {
        float elapsed = 0f;
        group.alpha = from;
        group.gameObject.SetActive(true);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            group.alpha = Mathf.Lerp(from, to, elapsed / duration);
            yield return null;
        }

        group.alpha = to;

        if (Mathf.Approximately(to, 0f))
            group.gameObject.SetActive(false);
    }
}
