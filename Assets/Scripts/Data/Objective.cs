using TMPro;
using UnityEngine;
using UnityEngine.UI;


public enum ObjectiveType
{
    CompleteTask,
    AchieveScore,
    DecreaseValue
}


public class Objective : MonoBehaviour
{
    public string objectiveName;
    public string description;
    public ObjectiveType objectiveType;
    public bool isCompleted;
    public float scoreToAchieve;
    public float scoreAchieved;
    private GameObject uiObject;
    private GameObject tick;

    public void Initialize(GameObject uiPrefab, GameObject uiParent)
    {
        uiObject = Instantiate(uiPrefab, uiParent.transform);
        UpdateText();
        tick = uiObject.transform.Find("Image/Tick").gameObject;
        tick.SetActive(isCompleted);
    }

    public void UpgradeProgress(float score)
    {
        // Se suma el score y se actualiza la cadena
        if (objectiveType == ObjectiveType.AchieveScore)
        {
            scoreAchieved += score;
            if (scoreAchieved >= scoreToAchieve)
            {
                isCompleted = true;
                EventLogger.Instance.LogEvent(new EventData("wai-complete_objective", new CompleteObjectiveEvent(ObjectiveManager.Instance.level, objectiveName)));
                tick.SetActive(true);
            }
            else
            {
                isCompleted = false;
                tick.SetActive(false);
            }
            UpdateText();
        }
        if (objectiveType == ObjectiveType.DecreaseValue)
        {
            scoreAchieved = score;
            if (scoreAchieved <= scoreToAchieve)
            {
                isCompleted = true;
                EventLogger.Instance.LogEvent(new EventData("wai-complete_objective", new CompleteObjectiveEvent(ObjectiveManager.Instance.level, objectiveName)));
                tick.SetActive(true);
            }
            else
            {
                isCompleted = false;
                tick.SetActive(false);
            }
            UpdateText();
        }
        if (objectiveType == ObjectiveType.CompleteTask)
        {
            if (score > 0)
            {
                isCompleted = true;
                tick.SetActive(true);
                EventLogger.Instance.LogEvent(new EventData("wai-complete_objective", new CompleteObjectiveEvent(ObjectiveManager.Instance.level, objectiveName)));
                UpdateText();
            }
            else
            {
                isCompleted = false;
                tick.SetActive(false);
            }
        }
    }

    public void CompleteObjective()
    {
        // Marca el objetivo como completado
        isCompleted = true;
        tick.SetActive(true);
        EventLogger.Instance.LogEvent(new EventData("wai-complete_objective", new CompleteObjectiveEvent(ObjectiveManager.Instance.level, objectiveName)));
        UpdateText();
    }

    private void UpdateText()
    {
        if (objectiveType == ObjectiveType.CompleteTask)
        {
            uiObject.GetComponentInChildren<TextMeshProUGUI>().text = $"{description}";
        }
        if (objectiveType == ObjectiveType.AchieveScore)
        {
            uiObject.GetComponentInChildren<TextMeshProUGUI>().text = $"{description} ({(int)scoreAchieved}/{(int)scoreToAchieve})";
        }
        if (objectiveType == ObjectiveType.DecreaseValue)
        {
            uiObject.GetComponentInChildren<TextMeshProUGUI>().text = $"{description} ({scoreAchieved:F2}/{scoreToAchieve:F2})";
        }
    }

}
