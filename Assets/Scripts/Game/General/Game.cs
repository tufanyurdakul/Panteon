using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Game : MonoBehaviour
{
    public int Timer { get; private set; }
    public bool GameEnd { get; private set; }
    public bool Painting { get; set; }
    public TextMeshProUGUI tmpTime;
    public Transform botSpawn;
    public GameObject EndPanel;
    private GameSettingsModel gameSettings;
    private GameObject Enemy;
    void Start()
    {
        Enemy = Resources.Load("Prefab/Characters/Girl") as GameObject;
        gameSettings = GetComponent<GameSettingsModel>();
        InsantiateEnemies();
        StartCoroutine(SetTime());
    }
    private IEnumerator SetTime()
    {
        while (!GameEnd)
        {
            // if time is more than gameSettings.TotalTime than game is finish
            yield return new WaitForSeconds(1);
            Timer++;
            tmpTime.SetText($"{gameSettings.TotalTime - Timer}");
            if (gameSettings.TotalTime - Timer <= 0)
            {
                GameEnd = true;
                EndPanel.SetActive(true);
            }
        }
    }
    //ins enemies
    private void InsantiateEnemies()
    {
        for (int i = 0; i < gameSettings.BotNumber / 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                GameObject bot = Instantiate(Enemy, botSpawn.position + new Vector3(-(float)i/3, 0, (float)j/3), botSpawn.rotation);
                BotModel botModel = bot.GetComponent<BotModel>();
                botModel.Id = botModel.Id + (i * 5) + j;
                TextMeshPro tmpBot = bot.GetComponentInChildren<TextMeshPro>();
                tmpBot.SetText(""+botModel.Id);
            }
        }
        
    }
}
