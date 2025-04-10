using TMPro;
using UnityEngine;
using System.Collections;

public class NotificationManager : MonoBehaviour
{
    public static NotificationManager Instance;

    public GameObject notificationPanel; // ← referencia al panel completo
    private TextMeshProUGUI notificationText;
    private CanvasGroup canvasGroup;
    public float fadeDuration = 0.5f;
    public float displayTime = 2f;

    private Coroutine currentNotification;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        canvasGroup = notificationPanel.GetComponent<CanvasGroup>();
        notificationText = notificationPanel.GetComponentInChildren<TextMeshProUGUI>();
        canvasGroup.alpha = 0f;
        notificationPanel.SetActive(false);
    }

    public void ShowNotification(string message)
    {
        Debug.Log("Notification: " + message);
        if (currentNotification != null)
            StopCoroutine(currentNotification);

        currentNotification = StartCoroutine(FadeNotification(message));
    }

    private IEnumerator FadeNotification(string message)
    {
        notificationText.text = message;
        notificationPanel.SetActive(true);

        yield return StartCoroutine(FadeCanvasGroup(0f, 1f));
        yield return new WaitForSecondsRealtime(displayTime);
        yield return StartCoroutine(FadeCanvasGroup(1f, 0f));

        notificationPanel.SetActive(false);
    }

    private IEnumerator FadeCanvasGroup(float start, float end)
    {
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            canvasGroup.alpha = Mathf.Lerp(start, end, elapsed / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = end;
    }
}
