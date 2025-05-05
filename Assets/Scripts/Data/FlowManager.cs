using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class FlowManager : MonoBehaviour
{
    public static FlowManager instance;
    public string groupID = "defaultGroup";
    public string loggedInUser = "defaultUser";
    public string session_id;
    public string game_id;
    public SelectedSet selectedSet;
    public bool sessionFinished = false;

    void Start()
    {
        game_id = "Wild AI";
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetUsername(string username)
    {
        loggedInUser = username;
    }
}



public class SelectedSet
{
    public string name = "DefaultSet";
}
