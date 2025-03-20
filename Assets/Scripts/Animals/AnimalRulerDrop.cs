using UnityEngine;
using UnityEngine.EventSystems;

public class AnimalRulerDrop : MonoBehaviour, IDropHandler
{
    public RectTransform uiImageRectTransform;
    public AnimalData animal;
    private Camera mainCamera;
    void Start()
    {
        animal = new AnimalData();
        mainCamera = Camera.main;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            // eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
            // Asignamos la posición del animal a la imagen arrastrada
            Vector3 worldPos = transform.position; // La posición del animal en el mundo
            uiImageRectTransform.position = worldPos;

            // Ajustar el tamaño de la imagen en el Canvas según el tamaño del animal y el zoom de la cámara
            RectTransform rectTransform = GetComponent<RectTransform>();
            uiImageRectTransform.sizeDelta = rectTransform.sizeDelta; // Ajustar el tamaño de la imagen
        }
    }
}
