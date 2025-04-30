using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AnimalSpawnInfo
{
    public GameObject prefab;
    public int quantity;
    public float noisePercentage = 0.2f; // Porcentaje de ruido para la variación
}

public class AnimalSpawner : MonoBehaviour
{
    [SerializeField] private List<AnimalSpawnInfo> animalsToSpawn;
    [SerializeField] private Transform spawnArea;  
    [SerializeField] private Vector2 spawnRange = new Vector2(10f, 10f);  

    void Start()
    {
        SpawnAnimals();
    }

    void SpawnAnimals()
    {
        foreach (var animal in animalsToSpawn)
        {
            for (int i = 0; i < animal.quantity; i++)
            {
                Vector3 spawnPosition = GetRandomPosition();
                GameObject animalObj = Instantiate(animal.prefab, spawnPosition, Quaternion.identity);
                animalObj.GetComponent<Animal>().CalculateWeight(animal.noisePercentage); // Calcula el peso del animal
            }
        }
    }

    Vector3 GetRandomPosition()
    {
        float x = Random.Range(-spawnRange.x, spawnRange.x);
        float y = Random.Range(-spawnRange.y, spawnRange.y);
        return spawnArea.position + new Vector3(x, y, 0);
    }

    // 🔹 Dibuja el área de spawn en la pestaña Scene
    void OnDrawGizmos()
    {
        if (spawnArea == null) return;

        Gizmos.color = Color.green;  // Color del área de spawn
        Vector3 center = spawnArea.position;
        Vector3 size = new Vector3(spawnRange.x * 2, spawnRange.y * 2, 0); 

        Gizmos.DrawWireCube(center, size);  // Dibuja el área como un rectángulo
    }
}
