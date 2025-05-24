using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ModelParameter : MonoBehaviour
{
    public TextMeshProUGUI parameterText;
    public TMP_InputField inputField;
    public TMP_Dropdown dropdown;
    public string parameterName;
    public string parameterValue;

    void Start()
    {
        parameterName = parameterText.text;
    }

    public void GetValues()
    {
        if (inputField != null)
        {
            parameterValue = inputField.text;
        }
        else if (dropdown != null)
        {
            parameterValue = dropdown.options[dropdown.value].text;
        }
    }
}

[System.Serializable]
public class ParameterData
{
    public string parameterName;
    public string parameterValue;
}
