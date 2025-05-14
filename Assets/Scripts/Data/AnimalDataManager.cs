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
    [SerializeField] private TMP_Dropdown nameText;

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
            name = nameText != null ? nameText.options[nameText.value].text : ""
        };
        AnimalDataSingleton.Instance.AddAnimal(newAnimal);
        EventLogger.Instance.LogEvent(new EventData("wai-register_animal", new AnimalEvent(ObjectiveManager.Instance.level, newAnimal)));

        // Mostrar notificación
        NotificationManager.Instance.ShowNotification("Animal añadido!");

        // Actualizar el progreso del objetivo del primer nivel
        ObjectiveManager.Instance.AddScoreToObjective("Register animals", 1);

        // Añade el animal al panel
        AddAnimalToPanel(newAnimal);

        // Autogenera animales dependidendo del nivel
        if (autoGenerate)
        {
            AutoGenerateAnimals(newAnimal.name);
        }
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

    // Autogenerador
    public bool autoGenerate = false;
    public int autoGenerateCount = 9;
    public float nullPercentage = 0.2f; // Porcentaje de nulos
    public List<CreateAnimalData> animalsInfo = new List<CreateAnimalData>();

    public float noisePercentage = 0.3f;

    public void AutoGenerateAnimals(string name)
    {
        string animalName = "";
        Vector2 heightRange = new Vector2(0.8f, 1.2f);
        Vector2 widthRange = new Vector2(0.8f, 1.2f);
        float densityFactor = 0;
        ColorData[] possibleColors = new ColorData[0];
        // Se averigua el tipo de animal
        foreach (var animalInfo in animalsInfo)
        {
            if (animalInfo.animalName == name)
            {
                animalName = animalInfo.animalName;
                heightRange = animalInfo.heightRange;
                widthRange = animalInfo.widthRange;
                densityFactor = animalInfo.densityFactor;
                possibleColors = animalInfo.possibleColors;
                // break;
            }
        }

        if (animalName != "")
        {
            for (int i = 0; i < autoGenerateCount; i++)
            {
                AnimalData newAnimal = new AnimalData(animalName, heightRange, widthRange, densityFactor, possibleColors, 0.3f); // Generar datos de animal
                AnimalDataSingleton.Instance.AddAnimal(newAnimal);
                AddAnimalToPanel(newAnimal); // Añadir al panel
            }
        }
    }
}

public class CreateAnimalData
{
    public string animalName;
    public Vector2 heightRange;
    public Vector2 widthRange;
    public float densityFactor;
    public ColorData[] possibleColors;
    public CreateAnimalData(string animalName, Vector2 heightRange, Vector2 widthRange, float densityFactor, ColorData[] possibleColors)
    {
        this.animalName = animalName;
        this.heightRange = heightRange;
        this.widthRange = widthRange;
        this.densityFactor = densityFactor;
        this.possibleColors = possibleColors;
    }
}
