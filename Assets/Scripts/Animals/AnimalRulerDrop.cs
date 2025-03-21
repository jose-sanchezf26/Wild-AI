using System.Data;
using System.Numerics;
using UnityEngine;
using UnityEngine.EventSystems;

public class AnimalRulerDrop : MonoBehaviour, IDropHandler
{
    public RectTransform uiImageRectTransform;
    public float offsetY = 0.5f;
    public float offsetX = 0.5f;
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
            Ruler ruler = eventData.pointerDrag.GetComponent<Ruler>();
            // Posición horizontal
            if (ruler.onHorizontalMode)
            {
                // Asignamos la posición del animal a la imagen arrastrada
                UnityEngine.Vector2 worldPos = transform.position; // La posición del animal en el mundo
                                                                   // Obtener la altura de la imagen del animal
                float animalHeight = GetComponent<RectTransform>().sizeDelta.y * transform.lossyScale.y;
                float newYPosition = worldPos.y - (animalHeight / 2) - (uiImageRectTransform.sizeDelta.y / 2) - offsetY;
                uiImageRectTransform.position = new UnityEngine.Vector2(worldPos.x, newYPosition);

                // Aplicar tamaño y posición en la UI
                uiImageRectTransform.sizeDelta = new UnityEngine.Vector2(GetComponent<RectTransform>().sizeDelta.x, uiImageRectTransform.sizeDelta.y); // Ajuste de tamaño
            }
            else // Posición vertical
            {
                // Asignamos la posición del animal a la imagen arrastrada
                UnityEngine.Vector2 worldPos = transform.position; // Posición del animal en el mundo

                // Obtener el ancho de la imagen del animal
                float animalWidth = GetComponent<RectTransform>().sizeDelta.x * transform.lossyScale.x;

                // Calcular la nueva posición en X (alinear a la **izquierda** del animal con un pequeño offset)
                float newXPosition = worldPos.x - (animalWidth / 2) - (uiImageRectTransform.sizeDelta.y / 2) - offsetX;

                // Aplicamos la nueva posición
                uiImageRectTransform.position = new UnityEngine.Vector2(newXPosition, worldPos.y);

                // Ajustar tamaño de la regla a la **altura** del animal
                uiImageRectTransform.sizeDelta = new UnityEngine.Vector2(GetComponent<RectTransform>().sizeDelta.y, uiImageRectTransform.sizeDelta.y);
            }
        }
    }
}
