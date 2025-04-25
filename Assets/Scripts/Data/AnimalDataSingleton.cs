using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class AnimalDataSingleton : MonoBehaviour
{
    // Lo hacemos Singleton
    public static AnimalDataSingleton Instance { get; private set; }

    // Lista de datos de animales
    public List<AnimalData> animalDataList;


    private void Awake()
    {
        // Aplicamos el patrón Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Mantener al cambiar de escena
            if (animalDataList == null) // Evita la reinicialización
                animalDataList = new List<AnimalData>();
        }
        else
        {
            Destroy(gameObject); // Evitar duplicados
        }
        animalDataList = new List<AnimalData>();
    }

    public void AddAnimal(AnimalData newAnimal)
    {
        animalDataList.Add(newAnimal);
    }

    public void RemoveAnimal(AnimalData animal)
    {
        animalDataList.Remove(animal);
    }
}
