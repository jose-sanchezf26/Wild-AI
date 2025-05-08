using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;


// Clases que definen los tipos de evento y sus campos
public class EventData
{
    // Usuario
    public string user;
    // ID de la sesión
    public string sessionId;
    // ID de la partida
    public string gameId;
    // Tipo de evento
    public string eventType;
    //Fecha del evento
    public string time;
    // Variable para los datos del evento
    public PlayerEvent data;

    public EventData(string eventType, PlayerEvent data)
    {
        user = FlowManager.instance.loggedInUser;
        gameId = FlowManager.instance.game_id;
        sessionId = FlowManager.instance.session_id;
        this.data = data;
        this.eventType = eventType;
        time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
    }
    public string ToJson() => JsonConvert.SerializeObject(this, new JsonSerializerSettings
    {
        TypeNameHandling = TypeNameHandling.None, // Serializa correctamente la herencia
        Formatting = Formatting.Indented
    });
}


// Clase padre para el resto de eventos
[JsonObject(MemberSerialization.OptIn)]
public class PlayerEvent
{
    public PlayerEvent() { }
}

[JsonObject(MemberSerialization.OptIn)]
public class LevelEvent : PlayerEvent
{
    [JsonProperty]
    public int level;
    public LevelEvent(int level) : base()
    {
        this.level = level;
    }
}

[JsonObject(MemberSerialization.OptIn)]
public class CompleteObjectiveEvent : LevelEvent
{
    [JsonProperty]
    public string objective;
    public CompleteObjectiveEvent(int level, string objective) : base(level)
    {
        this.objective = objective;
    }
}

[JsonObject(MemberSerialization.OptIn)]
public class WindowEvent : LevelEvent
{
    [JsonProperty]
    public string windowName;

    public WindowEvent(int level, string windowName) : base(level)
    {
        this.windowName = windowName;
    }
}

[JsonObject(MemberSerialization.OptIn)]
public class AnimalEvent : LevelEvent
{
    [JsonProperty]
    public int id;
    [JsonProperty]
    public string type;
    [JsonProperty]
    public float height;
    [JsonProperty]
    public float width;
    [JsonProperty]
    public float weight;
    [JsonProperty]
    public string color;

    public AnimalEvent(int level, AnimalData animal) : base(level)
    {
        id = animal.id;
        type = animal.name;
        height = animal.height;
        width = animal.width;
        weight = animal.weight;
        color = animal.color;
    }
}
