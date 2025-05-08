using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class AnimalDataManager : MonoBehaviour
{
    // Nivel
    public int level;

    // Campos para registrar un animal
    [SerializeField] private TMP_InputField widthText;
    [SerializeField] private TMP_InputField heightText;
    [SerializeField] private TMP_InputField weightText;
    [SerializeField] private TMP_Dropdown colorDropdown;
    [SerializeField] private TMP_InputField nameText;

    // Campos para visualizar el panel de datos
    [SerializeField] private GameObject dataEntry;
    [SerializeField] private GameObject dataPanel;

    // Campo para el objetivo del primer nivel
    [SerializeField] private Objective registerObjective;

    void Start()
    {
        LoadAnimalList();
    }

    public void AddAnimal()
    {
        AnimalData newAnimal = new AnimalData
        {

            width = float.Parse(widthText.text),
            height = float.Parse(heightText.text),
            weight = float.Parse(weightText.text),
            color = colorDropdown != null ? colorDropdown.options[colorDropdown.value].text : "",
            name = nameText != null ? nameText.text : ""
        };
        AnimalDataSingleton.Instance.AddAnimal(newAnimal);
        EventLogger.Instance.LogEvent(new EventData("wai-register_animal", new AnimalEvent(ObjectiveManager.Instance.level, newAnimal)));
                

        // Mostrar notificación
        NotificationManager.Instance.ShowNotification("Animal añadido!");

        // Actualizar el progreso del objetivo del primer nivel
        ObjectiveManager.Instance.AddScoreToObjective("Register animals", 1);

        // Añade el animal al panel
        AddAnimalToPanel(newAnimal);
    }

    public void AddAnimalToPanel(AnimalData animal)
    {
        GameObject entry;
        entry = Instantiate(dataEntry, dataPanel.transform);

        entry.GetComponent<DataEntry>().Initialize(animal);
        Debug.Log(AnimalDataSingleton.Instance.animalDataList.Count);
    }

    public void LoadAnimalList()
    {
        foreach (AnimalData animal in AnimalDataSingleton.Instance.animalDataList)
        {
            AddAnimalToPanel(animal);
        }
    }

    public void RemoveAnimal(AnimalData animal)
    {
        AnimalDataSingleton.Instance.RemoveAnimal(animal);
        EventLogger.Instance.LogEvent(new EventData("wai-delete_animal", new AnimalEvent(ObjectiveManager.Instance.level, animal)));
    }
}
