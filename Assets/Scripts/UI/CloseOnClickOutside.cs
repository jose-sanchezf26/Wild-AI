using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CloseOnClickOutside : MonoBehaviour
{
    public GameObject panelToClose; // Asigna aquí el panel que quieres cerrar
    public bool pause = false; // Variable para pausar el juego
    private GraphicRaycaster raycaster;
    private EventSystem eventSystem;
    private RectTransform panelRect;

    void Start()
    {
        // Aseguramos que el panel es este mismo objeto
        panelToClose = gameObject;
        panelRect = GetComponent<RectTransform>();
        eventSystem = EventSystem.current;
        raycaster = panelToClose.GetComponentInParent<Canvas>().GetComponent<GraphicRaycaster>();
    }

    void Update()
    {
        // // Solo responder a clic izquierdo
        // if (Input.GetMouseButtonDown(0))
        // {
        //     // Si el clic fue sobre algún objeto UI (como el panel) no lo cerramos
        //     if (EventSystem.current.IsPointerOverGameObject())
        //     {
        //         return;  // El clic fue sobre un elemento UI, no hacer nada
        //     }

        //     // Si el clic es fuera del panel (fuera de su RectTransform)
        //     if (!RectTransformUtility.RectangleContainsScreenPoint(
        //         panelToClose.GetComponent<RectTransform>(), 
        //         Input.mousePosition, 
        //         Camera.main))
        //     {
        //         if (pause)
        //             Time.timeScale = 1f;
        //         panelToClose.SetActive(false); // Cerrar el panel
        //     }
        // }

        if (Input.GetMouseButtonDown(0))
        {
            // Preparamos para raycastear
            PointerEventData pointerData = new PointerEventData(eventSystem)
            {
                position = Input.mousePosition
            };

            List<RaycastResult> results = new List<RaycastResult>();
            raycaster.Raycast(pointerData, results);

            // Si el primer elemento bajo el puntero **no** es parte de este panel, cerramos
            bool clickedOnThisPanel = false;
            foreach (var rr in results)
            {
                // Si alguno de los objetos apuntados es este panel o un hijo, lo consideramos un click interior
                if (rr.gameObject == panelToClose || rr.gameObject.transform.IsChildOf(panelToClose.transform))
                {
                    clickedOnThisPanel = true;
                    break;
                }
            }

            if (!clickedOnThisPanel)
            {
                if (pause)
                    Time.timeScale = 1f;
                panelToClose.SetActive(false);
                EventLogger.Instance.LogEvent(new EventData("wai-close_window", new WindowEvent(ObjectiveManager.Instance.level, gameObject.name)));
            }
        }
    }
}
