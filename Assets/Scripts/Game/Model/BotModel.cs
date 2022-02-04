using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BotModel : MonoBehaviour
{
    public int Id { get;  set; }
    public int Speed { get; set; }
    private void Awake()
    {
        BotStats botStats = new BotStats();
        botStats = JsonUtility.FromJson<BotStats>(PlayerPrefs.GetString("Bot"));
        this.Id = botStats.Id;
        this.Speed = botStats.Speed;
    }
}
