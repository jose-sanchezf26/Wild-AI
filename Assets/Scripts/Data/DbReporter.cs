using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using NativeWebSocket;
using System.Threading;
using Unity.VisualScripting;

public class DbReporter : MonoBehaviour
{
    #region Clases y Estructuras
    [System.Serializable]
    public class StartGameData
    {
        public string game_id = "shapes_playtest_2";
        public string group = FlowManager.instance.groupID;
        public string version_num = version;
        public OSEnvConfig env_configs = new OSEnvConfig();

        [System.Serializable]
        public class OSEnvConfig
        {
            public string OS = SystemInfo.operatingSystem ?? "unknown";
            public bool unityEditor = Application.isEditor;
        }
    }

    [System.Serializable]
    public class StartLevelData
    {
        public string user = FlowManager.instance.loggedInUser;
        public string group = FlowManager.instance.groupID;
        public string task_id;
        public string set_id;
        public bool fullscreen = Screen.fullScreen;
        public Vector2Int resolution = new Vector2Int(Screen.width, Screen.height);
        public string conditions;
    }

    public class DataObj
    {
        public string type;
        public string user;
        public string data;
        public Action<string> callback;

        public DataObj(string newType, string newData, Action<string> newCallback = null)
        {
            type = newType;
            data = newData;
            callback = newCallback;
            if (FlowManager.instance != null) { user = FlowManager.instance.loggedInUser; } else { user = "unknown"; }
        }

        public string ToJson() => JsonUtility.ToJson(this);
    }

    public class ResponseMessage
    {
        // public int status;
        // public string message;

        // public override string ToString() => $"Response {status}: '{message}'";
        public string type;
        public string username;

    }
    #endregion

    #region Variables
    public static DbReporter instance;
    public const string version = "0.6.2";

    private Queue<DataObj> toPost = new Queue<DataObj>();
    private Queue<EventData> toPostEventData = new Queue<EventData>();
    private bool needRestart = false;
    private CancellationTokenSource cancelTokenSource;
    private string serverUri = "ws://localhost:8000/ws/game-events";
    #endregion

    #region Unity Callbacks
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

    private async void Start()
    {
        Debug.Log("✅ Start() de DbReporter se ha llamado.");
        await ConnectWebSocket();
    }
    #endregion

    #region Métodos Principales
    public static void SendStartGameEvent(Action<string> callback)
    {
        var data = new StartGameData();
        SendEvent("start_game", JsonUtility.ToJson(data), callback);
    }

    // TODO: COMENTADO PORQUE NO EXISTE PUZZLEDATA
    // public static void SendStartLevelEvent(PuzzleData whichLevel)
    // {
    //     var data = new StartLevelData
    //     {
    //         task_id = whichLevel?.puzzleName ?? "Sandbox",
    //         set_id = FlowManager.instance.selectedSet?.name ?? "None",
    //         conditions = whichLevel?.GetConditionsString() ?? "{}"
    //     };
    //     SendEvent("start_level", JsonUtility.ToJson(data));
    // }

    public static void SendEvent(string type, string data, Action<string> callback = null)
    {
        instance.toPost.Enqueue(new DataObj(type, data, callback));
    }

    public static void SendEvent(EventData eventData)
    {
        // instance.toPostEventData.Enqueue(eventData);
        SendEventWebSocket(eventData);
    }
    #endregion

    #region WebSocket
    static WebSocket webSocket;
    private async Task ConnectWebSocket()
    {
        Debug.Log("🟢 Intentando conectar WebSocket...");
        webSocket = new WebSocket("ws://localhost:8000/ws/game-events");
        // webSocket = new WebSocket("ws://localhost:8000");
        webSocket.OnOpen += () =>
    {
        Debug.Log("Connection open!");
        EventData logInEvent = new EventData("sr-log_in", new PlayerEvent());
        EventLogger.Instance.LogEvent(logInEvent);
    };

        webSocket.OnError += (e) =>
        {
            Debug.Log("Error! " + e);
        };

        webSocket.OnClose += (e) =>
        {
            Debug.Log("Connection closed!");
        };

        webSocket.OnMessage += (bytes) =>
        {
            Debug.Log("OnMessage!");
            Debug.Log(bytes);

            // getting the message as a string
            // var message = System.Text.Encoding.UTF8.GetString(bytes);
            // Debug.Log("OnMessage! " + message);
        };

        // waiting for messages
        await webSocket.Connect();
    }

    void Update()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        webSocket.DispatchMessageQueue();
#endif
    }


    static async void SendEventWebSocket(EventData eventData)
    {
        if (webSocket.State == WebSocketState.Open)
        {
            string jsonMessage = eventData.ToJson();
            Debug.Log(jsonMessage);
            var bytes = Encoding.UTF8.GetBytes(jsonMessage);
            await webSocket.SendText(jsonMessage);
        }
    }

    private async void OnApplicationQuit()
    {
        await webSocket.Close();
    }
    #endregion
}