using UnityEngine;
using UnityEngine.UI;

public class ObjectiveUIRenderer : MonoBehaviour
{
    public GameObject uiPrefab;
    public GameObject uiParent;

    void Start()
    {
        // RenderObjectives();
    }

    public void RenderObjectives()
    {
        foreach (Objective objective in ObjectiveManager.objectives)
        {
            objective.Initialize(uiPrefab, uiParent);
            Debug.Log("Objective renderizado: " + objective.objectiveName);
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
    }

}
