using UnityEngine;
using UnityEngine.EventSystems;

public class CloseOnClickOutside : MonoBehaviour
{
    public GameObject panelToClose; // Asigna aquí el panel que quieres cerrar

    void Start()
    {
        panelToClose = gameObject;
    }

    void Update()
    {
        // Solo responde si se hace clic
        if (Input.GetMouseButtonDown(0))
        {
            // Si no estás haciendo clic en un UI element
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                panelToClose.SetActive(false);
                return;
            }

            // Si el clic es fuera del panel
            if (!RectTransformUtility.RectangleContainsScreenPoint(
                panelToClose.GetComponent<RectTransform>(),
                Input.mousePosition,
                Camera.main))
            {
                panelToClose.SetActive(false);
            }
        }
    }
}

