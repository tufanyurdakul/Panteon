using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BotPanel : MonoBehaviour
{
    enum Values
    {
        botNumber = 19,
        botSpeed = 3
    }
    public List<Scrollbar> Scrolls;
    public List<TextMeshProUGUI> tmpValues;
    public Button btnOkey;
    private GameSettings CurrentSettings;
    private BotStats CurrentBot;
    void Start()
    {
        CurrentSettings = new GameSettings();
        CurrentSettings = JsonUtility.FromJson<GameSettings>(PlayerPrefs.GetString("GameSettings"));
        CurrentBot = new BotStats();
        CurrentBot = JsonUtility.FromJson<BotStats>(PlayerPrefs.GetString("Bot"));
        Scrolls[0].value = ((float)CurrentSettings.BotNumber / ((float)Values.botNumber + 1));
        Scrolls[1].value = ((float)CurrentBot.Speed / ((float)Values.botSpeed + 1));
        btnOkey.onClick.AddListener(ClickOkey);
    }
    private void ClickOkey()
    {
        BotStats botModel = new BotStats()
        {
            Id = 2,
            Speed = (int)(1 + (Scrolls[1].value * (int)Values.botSpeed))
        };
        GameSettings gameSettings = new GameSettings()
        {
            BotNumber = (int)(1 + (Scrolls[0].value * (int)Values.botNumber)),
            TotalTime = CurrentSettings.TotalTime,
            MovePerTime = CurrentSettings.MovePerTime,
            SpeedRatio = CurrentSettings.SpeedRatio
        };
        PlayerPrefs.SetString("GameSettings", JsonUtility.ToJson(gameSettings));
        PlayerPrefs.SetString("Bot", JsonUtility.ToJson(botModel));
    }
    // Update is called once per frame
    void Update()
    {
        tmpValues[0].SetText("(" + (int)(1 + (Scrolls[0].value * (int)Values.botNumber))+")");
        tmpValues[1].SetText("(" + (int)(1 + (Scrolls[1].value * (int)Values.botSpeed))+")");
    }
}
