using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : MonoBehaviour
{
    public int Id { get; private set; }
    public int Speed { get; set; }
    private void Awake()
    {
        PlayerStats playerStats = new PlayerStats();
        playerStats = JsonUtility.FromJson<PlayerStats>(PlayerPrefs.GetString("Player"));
        this.Id = playerStats.Id;
        this.Speed = playerStats.Speed;
    }
}
