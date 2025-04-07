using UnityEngine;

public class HelpButton : MonoBehaviour
{
    [SerializeField] private GameObject helpPanel; 

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
    }
}