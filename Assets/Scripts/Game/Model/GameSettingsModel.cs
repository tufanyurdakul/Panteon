using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettingsModel : MonoBehaviour
{
    public int MovePerTime { private set;  get; }
    public int SpeedRatio { private set; get; }
    public int TotalTime { private set; get; }
    public int BotNumber { private set; get; }

    void OnEnable()
    {
        GameSettings gameSettings = new GameSettings();
        gameSettings = JsonUtility.FromJson<GameSettings>(PlayerPrefs.GetString("GameSettings"));
        MovePerTime = gameSettings.MovePerTime;
        SpeedRatio = gameSettings.SpeedRatio;
        TotalTime = gameSettings.TotalTime;
        BotNumber = gameSettings.BotNumber;
    }
}
