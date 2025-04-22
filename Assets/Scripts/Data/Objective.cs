using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public enum ObjectiveType
{
    CompleteTask,
    AchieveScore
}


public class Objective : MonoBehaviour
{
    public string objectiveName;
    public string description;
    public ObjectiveType objectiveType;
    public bool isCompleted;
    public int scoreToAchieve;
    public int scoreAchieved;
    private GameObject uiObject;
    private GameObject tick;

    public void Initialize(GameObject uiPrefab, GameObject uiParent)
    {
        uiObject = Instantiate(uiPrefab, uiParent.transform);
        UpdateText();
        tick = uiObject.transform.Find("Image/Tick").gameObject;
        tick.SetActive(false);
    }

    public void UpgradeProgress(int score)
    {
        // Se suma el score y se actualiza la cadena
        scoreAchieved += score;
        UpdateText();

        if (scoreAchieved >= scoreToAchieve)
        {
            isCompleted = true;
            tick.SetActive(true);
        }
    }

    private void UpdateText()
    {
        if (objectiveType == ObjectiveType.CompleteTask)
        {
            uiObject.GetComponentInChildren<TextMeshProUGUI>().text = $"{description}";
        }
        else if (objectiveType == ObjectiveType.AchieveScore)
        {
            uiObject.GetComponentInChildren<TextMeshProUGUI>().text = $"{description} ({scoreAchieved}/{scoreToAchieve})";
        }
    }

}
