using System.Collections.Generic;
using UnityEngine;

public class EventLogger : MonoBehaviour
{
    // Lista para almacenar los eventos
    private List<EventData> eventLog = new List<EventData>();

    // Singleton para que sea accesible desde cualquier parte del juego
    public static EventLogger Instance;

    // Número de eventos que se envían al servidor
    private const int MAX_EVENTS = 10;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // Mantener el logger entre escenas si es necesario
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Método para añadir un evento
    // public void LogEvent(string eventDescription, string type)
    // {
    //     // string timestamp = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    //     string timestamp = System.DateTime.Now.ToString("HH:mm:ss");
    //     eventLog.Add($"{timestamp}: {eventDescription}");
    //     Debug.Log(timestamp + " " + eventDescription);
    //     I_BE2_ProgrammingEnv i_BE2_ProgrammingEnv = FindFirstObjectByType<BE2_ProgrammingEnv>();

    //     // Identifica el tipo de evento
    //     if (type == "modifySBR")
    //     {
    //         Debug.Log(CreateSBRString(i_BE2_ProgrammingEnv));
    //     }

    //     // Si ya hay 10 eventos, los envía al servidor
    //     if (eventLog.Count == MAX_EVENTS)
    //     {
    //         SendEvents("game_events");
    //     }
    // }

    public void LogEvent(EventData eventData)
    {
        // I_BE2_ProgrammingEnv i_BE2_ProgrammingEnv = FindFirstObjectByType<BE2_ProgrammingEnv>();

        // // Identifica el tipo de evento
        // if (eventData.data is DropBlockEvent dropBlockEvent)
        // {
        //     string sbr = CreateSBRString(i_BE2_ProgrammingEnv);
        //     dropBlockEvent.sbr = sbr;
        //     eventLog.Add(eventData);
        //     SendEvent(eventData);
        // }
        // else if (eventData.data is ModifySBREvent modifySBREvent)
        // {
        //     string sbr = CreateSBRString(i_BE2_ProgrammingEnv);
        //     modifySBREvent.sbr = sbr;
        //     eventLog.Add(eventData);
        //     SendEvent(eventData);
        // }
        // else if (eventData.data is DuplicateBlockEvent duplicateBlockEvent)
        // {
        //     string sbr = CreateSBRString(i_BE2_ProgrammingEnv);
        //     duplicateBlockEvent.sbr = sbr;
        //     eventLog.Add(eventData);
        //     SendEvent(eventData);
        // }
        // else if (eventData.data is DeleteBlockEvent deleteBlockEvent)
        // {
        //     string sbr = CreateSBRString(i_BE2_ProgrammingEnv);
        //     deleteBlockEvent.sbr = sbr;
        //     eventLog.Add(eventData);
        //     SendEvent(eventData);
        // }
        // else
        // {
            eventLog.Add(eventData);
            SendEvent(eventData);
        // }
    }

    // Guardar los eventos en un archivo al final de la sesión
    public void SaveLog()
    {
        // string filePath = Path.Combine(Application.persistentDataPath, "event_log.txt");
        // File.WriteAllLines(filePath, eventLog.ToArray());
        // Debug.Log($"Log guardado en: {filePath}");
    }

    // Método para enviar los eventos al servidor
    public void SendEvents(string type)
    {
        string jsonData = JsonUtility.ToJson(new EventsData(eventLog));
        DbReporter.SendEvent(type, jsonData);
        eventLog.Clear();
    }

    public void SendEvent(EventData eventData)
    {
        string jsonData = JsonUtility.ToJson(eventData);
        DbReporter.SendEvent(eventData);
        eventLog.Clear();
    }

    // Guardar automáticamente cuando el juego se cierra
    private void OnApplicationQuit()
    {
        SaveLog();
    }

    // Clase para estructurar los eventos en JSON
    [System.Serializable]
    private class EventsData
    {
        public List<EventData> events;
        public EventsData(List<EventData> events)
        {
            this.events = events;
        }
    }
}

