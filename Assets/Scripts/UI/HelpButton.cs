using UnityEngine;

public class HelpButton : MonoBehaviour
{
    [SerializeField] private GameObject helpPanel; 
    public bool pause = false;

    void Start()
    {
        if(pause)
            Time.timeScale = 0f; 
    }

    public void ToggleHelpPanel()
    {
        if (helpPanel != null)
        {
            helpPanel.SetActive(true); 
        }
        else
        {
            Debug.LogWarning("Help panel is not assigned in the inspector.");
        }
        if (helpPanel.activeSelf && pause)
            Time.timeScale = 0f; 
    }
}