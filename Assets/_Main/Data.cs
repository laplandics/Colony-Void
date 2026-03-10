using System;
using UnityEngine;

[Serializable]
public class Data
{
    public Preferences preferences = new();
    public GameState gameState = new();
}

[Serializable]
public class GameState
{
    public CamConfig[] cameras;
    public UnitConfig[] units;
}

[Serializable]
public class CamConfig : IEntityConfig
{
    public string entityKey;
    public string entityType;
    public string[] components;

    public string EntityKey { get => entityKey; set => entityKey = value; }
    public string EntityType => entityType;
}

[Serializable]
public class UnitConfig : IEntityConfig
{
    public string entityKey;
    public string entityType;
    public string unitType;
    public string[] systems;
    public string[] modules;
    
    public string EntityKey { get => entityKey; set => entityKey = value; }
    public string EntityType => entityType;
}

[Serializable]
public class Preferences
{
    public VideoPreferences video = new();
    public GamePreferences game = new();
    public CameraPreferences camera = new();
}

[Serializable]
public class GamePreferences
{
    public string[] managers = { nameof(CameraManager), };
    public char splitChar = '/';
    public float gameCycleInterval = 10f;
}

[Serializable]
public class VideoPreferences
{
    public bool vSync = false;
    public int frameRate = 144;
}

[Serializable]
public class CameraPreferences
{
    public Color32 backgroundColor = new( 30, 30, 30, 255 );
    public float farClip = 100f;
    
    [Space]
    public float minZoom = 4f;
    public float maxZoom = 20f;
    public float zoomSpeed = 0.2f;
    public float zoomStep = 1.5f;

    [Space]
    public float baseSpeed = 10f;
    public float maxSpeed = 18f;
    public float minSpeed = 5f;
    public float moveMult = 1f;
    public float dragMult = 1.8f;
    
    [Space]
    public float minVerticalAngle = 0f;
    public float maxVerticalAngle = 88f;
    public float horizontalSensitivity = 1f;
    public float verticalSensitivity = 1.5f;
}