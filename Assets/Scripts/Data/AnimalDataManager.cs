using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class AnimalDataManager : MonoBehaviour
{
    // Lo hacemos Singleton
    public static AnimalDataManager Instance { get; private set; }

    private List<AnimalData> animalDataList;

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

    // Campos para registrar un animal
    [SerializeField] private TMP_InputField widthText;
    [SerializeField] private TMP_InputField heightText;
    [SerializeField] private TMP_InputField weightText;
    [SerializeField] private TMP_Dropdown colorDropdown;
    [SerializeField] private TMP_InputField nameText;
    public void AddAnimal()
    {
        AnimalData newAnimal = new AnimalData
        {
            width = float.Parse(widthText.text),
            height = float.Parse(heightText.text),
            weight = float.Parse(weightText.text),
            color = colorDropdown.options[colorDropdown.value].text,
            name = nameText.text
        };
        animalDataList.Add(newAnimal);
        // Cada vez que se añade un animal, se actualiza el panel de datos
        GameObject entry = Instantiate(dataEntry, dataPanel.transform);
        entry.GetComponent<DataEntry>().Initialize(newAnimal);
        Debug.Log(animalDataList.Count);
    }

    public void RemoveAnimal(AnimalData animal)
    {
        animalDataList.Remove(animal);
    }

    // Campos para visualizar el panel de datos
    [SerializeField] private GameObject dataEntry;
    [SerializeField] private GameObject dataPanel;

    public void ShowDataPanel()
    {
        foreach (Transform child in dataPanel.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (var animal in animalDataList)
        {
            GameObject entry = Instantiate(dataEntry, dataPanel.transform);
            entry.GetComponent<DataEntry>().Initialize(animal);
        }
    }
}
