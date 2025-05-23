using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class ModelSelector : MonoBehaviour
{
    public TMP_Dropdown modelDropdown;  // Asigna en el inspector
    public GameObject logisticPanel;
    public GameObject rfPanel;
    public GameObject decisionTreePanel;
    // Agrega los demás paneles aquí

    private Dictionary<string, GameObject> modelPanels;

    void Start()
    {
        // Asocia cada nombre del dropdown con su panel correspondiente
        modelPanels = new Dictionary<string, GameObject>()
        {
            { "Logistic Regression", logisticPanel },
            { "Random Forest", rfPanel },
            { "Decision Tree", decisionTreePanel },
        };

        modelDropdown.onValueChanged.AddListener(OnModelChanged);

        // Inicializar: desactivar todos los paneles
        // DeactivateAllPanels();
    }

    void OnModelChanged(int index)
    {
        DeactivateAllPanels();

        string selectedModel = modelDropdown.options[index].text;

        if (modelPanels.ContainsKey(selectedModel))
        {
            modelPanels[selectedModel].SetActive(true);
        }
    }

    void DeactivateAllPanels()
    {
        foreach (var panel in modelPanels.Values)
        {
            panel.SetActive(false);
        }
    }
}
