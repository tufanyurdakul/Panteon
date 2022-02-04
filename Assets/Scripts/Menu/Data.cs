using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data : MonoBehaviour
{
    
    private void Awake()
    {
        //Data For Menu
        if (PlayerPrefs.GetInt("isFirst") == 0)
        {
            PlayerPrefs.SetString("Player", "{\"Speed\":2,\"Id\":1}");
            PlayerPrefs.SetString("Bot", "{\"Speed\":2,\"Id\":2}");
            PlayerPrefs.SetString("GameSettings", "{\"MovePerTime\":2,\"SpeedRatio\":10,\"TotalTime\":45,\"BotNumber\":10}");
            PlayerPrefs.SetInt("isFirst", 1);
        }
    }
}
