using UnityEngine;
using UnityEngine.EventSystems;

public class Ruler : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private RectTransform rectTransform;
    [SerializeField] private Canvas canvas;
    private CanvasGroup canvasGroup;
    [SerializeField] private Vector2 initialPosition;
    public bool onHorizontalMode = true;
    public GameObject numberImage;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    void Update()
    {
        if (gameObject.activeSelf)
        {
            if (Input.GetMouseButtonDown(1))
            {
                onHorizontalMode = !onHorizontalMode;
                rectTransform.localEulerAngles = new Vector3(0, 0, onHorizontalMode ? 0 : 270);
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("RulerUI.OnPointerDown");
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        numberImage.SetActive(false);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("RulerUI.OnBeginDrag");
        canvasGroup.blocksRaycasts = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("RulerUI.OnEndDrag");
        canvasGroup.blocksRaycasts = true;
    }

    public void RestartPosition()
    {
        GetComponent<RectTransform>().anchoredPosition = initialPosition;
    }
}
