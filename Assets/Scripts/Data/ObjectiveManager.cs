using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class ObjectiveManager : MonoBehaviour
{
    public static ObjectiveManager Instance { get; private set; }

    public static List<LevelObjectives> allLevelObjectives;

    public static List<Objective> objectives;
    public int level;


    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        InitializeAllLevelObjectives();
    }

    public void LoadObjectivesForLevel(int levelN)
    {
        this.level = levelN;
        var level = allLevelObjectives.Find(l => l.level == levelN);
        objectives = level.objectives;
        Debug.Log("Cargando objetivos del nivel " + objectives.Count);
    }

    public static void InitializeAllLevelObjectives()
    {
        allLevelObjectives = new List<LevelObjectives>();

        // Objetivos del Nivel 1
        var nivel1 = new LevelObjectives
        {
            level = 1,
            objectives = new List<Objective>
        {
            new Objective
            {
                objectiveName = "Register animals",
                description = "Registra animales en la base de datos",
                objectiveType = ObjectiveType.AchieveScore,
                scoreToAchieve = 5
            },
            new Objective
            {
                objectiveName = "Create model",
                description = "Crea y prueba un primer modelo",
                objectiveType = ObjectiveType.CompleteTask,
                scoreToAchieve = 1
            },
            new Objective
            {
                objectiveName = "Reach Error",
                description = "Consigue un modelo con un error menor del 30% en cinco pruebas",
                objectiveType = ObjectiveType.DecreaseValue,
                scoreToAchieve = 20
            }
        }
        };

        allLevelObjectives.Add(nivel1);
    }


    public void AddScoreToObjective(string name, float amount)
    {
        foreach (var obj in objectives)
        {
            if (obj.objectiveName == name)
            {
                obj.UpgradeProgress(amount);
                CheckCompletedObjectives();
            }
        }
    }

    private void CheckCompletedObjectives()
    {
        foreach (var obj in objectives)
        {
            if (!obj.isCompleted)
                return;
        }
        WindowsManager windowsManager = FindFirstObjectByType<WindowsManager>();
        if (windowsManager != null)
            windowsManager.OpenObjectivesCompletedPanel();
    }

}

public class LevelObjectives
{
    public int level;
    public List<Objective> objectives;
}
