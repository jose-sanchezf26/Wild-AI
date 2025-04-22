using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class ObjectiveManager : MonoBehaviour
{
    public static ObjectiveManager Instance { get; private set; }

    public List<LevelObjectives> allLevelObjectives;

    [SerializeField] public List<Objective> objectives;
    public int level;

    void Start()
    {
        InitializeAllLevelObjectives();
        LoadObjectivesForLevel(1);
        FindFirstObjectByType<ObjectiveUIRenderer>().RenderObjectives();

    }

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void LoadObjectivesForLevel(int levelN)
    {
        this.level = levelN;
        var level = allLevelObjectives.Find(l => l.level == levelN);
        objectives = level.objectives;
    }

    public void InitializeAllLevelObjectives()
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
            }
        }
        };

        allLevelObjectives.Add(nivel1);
    }

    public void AddScoreToObjective(string name, int amount)
    {
        foreach (var obj in objectives)
        {
            if (obj.objectiveName == name)
            {
                obj.UpgradeProgress(amount);
            }
        }
    }

}

public class LevelObjectives
{
    public int level;
    public List<Objective> objectives;
}
