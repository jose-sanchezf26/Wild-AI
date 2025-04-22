using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class AnimalDataManager : MonoBehaviour
{
    // Lo hacemos Singleton
    public static AnimalDataManager Instance { get; private set; }

    // Nivel
    public int level;

    // Lista de datos de animales
    public List<AnimalData> animalDataList;

    // Campos para registrar un animal
    [SerializeField] private TMP_InputField widthText;
    [SerializeField] private TMP_InputField heightText;
    [SerializeField] private TMP_InputField weightText;
    [SerializeField] private TMP_Dropdown colorDropdown;
    [SerializeField] private TMP_InputField nameText;

    // Campos para visualizar el panel de datos
    [SerializeField] private GameObject dataEntry;
    [SerializeField] private GameObject dataEntryLevel1;
    [SerializeField] private GameObject dataPanel;

    // Campo para el objetivo del primer nivel
    [SerializeField] private Objective registerObjective;

    private void Awake()
    {
        // Aplicamos el patrón Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Mantener al cambiar de escena
        }
        else
        {
            Destroy(gameObject); // Evitar duplicados
        }
        animalDataList = new List<AnimalData>();
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
        animalDataList.Add(newAnimal);

        // Mostrar notificación
        NotificationManager.Instance.ShowNotification("Animal añadido!");
        
        if (registerObjective != null) registerObjective.UpgradeProgress(1); // Actualizar el progreso del objetivo

        // Cada vez que se añade un animal, se actualiza el panel de datos
        GameObject entry;
        if (level != 1)
            entry = Instantiate(dataEntry, dataPanel.transform);
        else
            entry = Instantiate(dataEntryLevel1, dataPanel.transform);


        entry.GetComponent<DataEntry>().Initialize(newAnimal);
        Debug.Log(animalDataList.Count);
    }

    public void RemoveAnimal(AnimalData animal)
    {
        animalDataList.Remove(animal);
    }
}
