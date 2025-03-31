using System.Data;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class AnimalRulerDrop : MonoBehaviour, IDropHandler
{
    public RectTransform uiImageRectTransform;
    public GameObject numberImage;
    public float offsetY;
    public float offsetNumberY;
    public float offsetX;
    public float offsetNumberX;
    public AnimalData animal;
    private Camera mainCamera;
    public TMP_InputField inputFieldWidth;
    public TMP_InputField inputFieldHeight;
    void Start()
    {
        animal = new AnimalData();
        mainCamera = Camera.main;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.GetComponent<Ruler>() != null)
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

                // Ajustamos la posición del texto que indica la medida del animal
                numberImage.SetActive(true);
                RectTransform numberImageRectTransform = numberImage.GetComponent<RectTransform>();
                float numberHeight = numberImageRectTransform.sizeDelta.y * numberImageRectTransform.lossyScale.y;
                float numberYPosition = newYPosition - (uiImageRectTransform.sizeDelta.y / 2) - (numberHeight / 2) - offsetNumberY;
                numberImageRectTransform.position = new UnityEngine.Vector2(worldPos.x, numberYPosition);
                numberImage.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = animal.width.ToString("F2") + "m";
                inputFieldWidth.text = animal.width.ToString("F2");
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

                // Ajustamos la posición del texto que indica la medida del animal
                numberImage.SetActive(true);
                RectTransform numberImageRectTransform = numberImage.GetComponent<RectTransform>();
                // Posición de la regla
                // Obtener la ALTURA de la regla (ya que está rotada, esta altura es el "largo" real)
                float rulerHeight = uiImageRectTransform.sizeDelta.y * uiImageRectTransform.lossyScale.y;

                // Obtener el ancho de la imagen del número
                float numberWidth = numberImageRectTransform.sizeDelta.x * numberImageRectTransform.lossyScale.x;


                // Calcular la nueva posición en X para la imagen del número
                float numberXPosition = newXPosition - (rulerHeight / 2) - (numberWidth / 2) - offsetNumberX;

                // Aplicar la nueva posición
                numberImageRectTransform.position = new UnityEngine.Vector2(numberXPosition, worldPos.y);
                numberImage.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = animal.height.ToString("F2") + "m";
                inputFieldHeight.text = animal.height.ToString("F2");
            }
        }
    }
}
