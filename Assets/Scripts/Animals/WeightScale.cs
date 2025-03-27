using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class WeightScale : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private RectTransform rectTransform;
    [SerializeField] private Canvas canvas;
    private CanvasGroup canvasGroup;
    [SerializeField] private Vector2 initialPosition;
    public GameObject numberImage;
    public TextMeshProUGUI text;
    public float duration = 1f;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        text.text = "0.00";
        numberImage.SetActive(false);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
    }

    public void RestartPosition()
    {
        GetComponent<RectTransform>().anchoredPosition = initialPosition;
    }

    private Coroutine CountingCoroutine;

    public void UpdateText(float newValue)
    {
        if (CountingCoroutine != null)
        {
            StopCoroutine(CountingCoroutine);
        }
        StartCoroutine(CountText(newValue));
    }

    private IEnumerator CountText(float newValue)
    {
        float previousValue = float.Parse(text.text);
        float elapsedTime = 0f;
        float startValue = previousValue;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            previousValue = Mathf.Lerp(startValue, newValue, elapsedTime / duration);
            text.text = previousValue.ToString("F2");
            yield return null; // Espera hasta el próximo frame
        }

        text.text = newValue.ToString("F2"); // Asegurar que el valor final sea exacto
    }
}
