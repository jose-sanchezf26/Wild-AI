using UnityEngine;

public class StepsNavegator : MonoBehaviour
{
    public GameObject[] steps;
    private int currentStepIndex = 0;

    public void NextStep()
    {
        if (currentStepIndex < steps.Length - 1)
        {
            steps[currentStepIndex].SetActive(false);
            currentStepIndex++;
            steps[currentStepIndex].SetActive(true);
        }
    }

    public void PreviousStep()
    {
        if (currentStepIndex > 0)
        {
            steps[currentStepIndex].SetActive(false);
            currentStepIndex--;
            steps[currentStepIndex].SetActive(true);
        }
    }
}
