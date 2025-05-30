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
    private int animalCount = 0;


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
        newAnimal.id = animalCount;
        animalCount++;
        animalDataList.Add(newAnimal);
        // Objetivo del nivel 5
        if (ObjectiveManager.Instance.level == 5)
        {
            CheckDesbalanceObjective();
        }
    }

    public void RemoveAnimal(AnimalData animal)
    {
        animalDataList.Remove(animal);
        EventLogger.Instance.LogEvent(new EventData("wai-delete_animal", new AnimalEvent(ObjectiveManager.Instance.level, animal)));
        // Objetivo del nivel 5
        if (ObjectiveManager.Instance.level == 5)
        {
            CheckDesbalanceObjective();
        }
    }

    public void CheckDesbalanceObjective()
    {
        if (CheckDesbalance())
        {
            ObjectiveManager.Instance.AddScoreToObjective("Desbalanced data", 1);
        } else
        {
            ObjectiveManager.Instance.AddScoreToObjective("Desbalanced data", -1);
        }
    }

    public bool CheckDesbalance()
    {
        int osos = 0, zorros = 0, lobos = 0, ciervos = 0;

        // Contamos cuántos hay de cada especie
        foreach (var animal in animalDataList)
        {
            switch (animal.name.ToLower())
            {
                case "oso":
                    osos++;
                    break;
                case "zorro":
                    zorros++;
                    break;
                case "lobo":
                    lobos++;
                    break;
                case "ciervo":
                    ciervos++;
                    break;
            }
        }

        bool criterio = (osos >= 2 * lobos) && (osos >= 2 * ciervos) && (zorros >= 2 * lobos) && (zorros >= 2 * ciervos) && (ciervos > 0) && (lobos > 0) && (osos > 0) && (zorros > 0);
        return criterio;
    }
}
