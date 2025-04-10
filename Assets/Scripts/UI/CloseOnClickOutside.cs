using UnityEngine;
using UnityEngine.EventSystems;

public class CloseOnClickOutside : MonoBehaviour
{
    public GameObject panelToClose; // Asigna aquí el panel que quieres cerrar
    public bool pause = false; // Variable para pausar el juego

    void Start()
    {
        // Aseguramos que el panel es este mismo objeto
        panelToClose = gameObject;
    }

    void Update()
    {
        // Solo responder a clic izquierdo
        if (Input.GetMouseButtonDown(0))
        {
            // Si el clic fue sobre algún objeto UI (como el panel) no lo cerramos
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;  // El clic fue sobre un elemento UI, no hacer nada
            }

            // Si el clic es fuera del panel (fuera de su RectTransform)
            if (!RectTransformUtility.RectangleContainsScreenPoint(
                panelToClose.GetComponent<RectTransform>(), 
                Input.mousePosition, 
                Camera.main))
            {
                if (pause)
                    Time.timeScale = 1f;
                panelToClose.SetActive(false); // Cerrar el panel
            }
        }
    }
}
