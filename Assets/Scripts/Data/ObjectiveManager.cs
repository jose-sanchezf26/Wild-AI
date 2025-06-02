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
                description = "Consigue un modelo con un error menor del 20% en cinco pruebas",
                objectiveType = ObjectiveType.DecreaseValue,
                scoreToAchieve = 20
            }
        }
        };

        var nivel5 = new LevelObjectives
        {
            level = 5,
            objectives = new List<Objective>
        {
            new Objective
            {
                objectiveName = "Desbalanced data",
                description = "Consigue una base de datos desbalanceada, con osos y zorros como clases mayoritarias y lobos y ciervos como minoritarias",
                objectiveType = ObjectiveType.CompleteTask,
                scoreToAchieve = 1
            },
            new Objective
            {
                objectiveName = "Create model desbalanced",
                description = "Crea y prueba un modelo sin usar ninguna técnica de balanceo",
                objectiveType = ObjectiveType.CompleteTask,
                scoreToAchieve = 1
            },
            new Objective
            {
                objectiveName = "Create model balanced",
                description = "Crea y prueba un modelo usando alguna técnica de balanceo",
                objectiveType = ObjectiveType.CompleteTask,
                scoreToAchieve = 1
            },
            new Objective
            {
                objectiveName = "Create model with f1-score",
                description = "Crea un modelo con un f1-score como mínimo de 0.7 en todas las clases",
                objectiveType = ObjectiveType.CompleteTask,
                scoreToAchieve = 1
            },
        }
        };
        allLevelObjectives.Add(nivel1);
        allLevelObjectives.Add(nivel5);
    }


    public bool ExistObjective(string name)
    {
        foreach (var obj in objectives)
        {
            if (obj.objectiveName == name)
            {
                return true;
            }
        }
        return false;
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
        // Codigo para cuando completas todos los objetivos
        EventLogger.Instance.LogEvent(new EventData("wai-complete_level", new LevelEvent(level)));
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
