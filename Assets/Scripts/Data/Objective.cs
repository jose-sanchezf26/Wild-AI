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
    public GameObject uiPrefab;
    public GameObject uiParent;
    private GameObject uiObject;
    private GameObject tick;
    void Start()
    {
        isCompleted = false;
        scoreAchieved = 0;
        uiObject = Instantiate(uiPrefab, uiParent.transform); 
        uiObject.GetComponentInChildren<TextMeshProUGUI>().text = $"{description} ({scoreAchieved}/{scoreToAchieve})"; 
        tick = uiObject.transform.Find("Image/Tick").gameObject;
        tick.SetActive(false);
    }

    public void UpgradeProgress(int score)
    {
        // Se suma el score y se actualiza la cadena
        scoreAchieved += score;
        uiObject.GetComponentInChildren<TextMeshProUGUI>().text = $"{description} ({scoreAchieved}/{scoreToAchieve})"; 

        if (scoreAchieved >= scoreToAchieve)
        {
            isCompleted = true;
            tick.SetActive(true);
        }
    }

}
