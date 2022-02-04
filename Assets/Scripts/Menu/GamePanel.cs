using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GamePanel : MonoBehaviour
{
    private GameSettings CurrentSettings;
    private PlayerStats CurrentPlayer;
    public List<Scrollbar> Scrolls;
    public List<TextMeshProUGUI> tmpValues;
    public Button btnOkey;
    enum Values
    {
        playerSpeed = 3,
        obsSpeed = 14,
        time = 30,
        rotate = 4
    }
    void Start()
    {
        CurrentSettings = new GameSettings();
        CurrentPlayer = new PlayerStats();
        CurrentSettings = JsonUtility.FromJson<GameSettings>(PlayerPrefs.GetString("GameSettings"));
        CurrentPlayer = JsonUtility.FromJson<PlayerStats>(PlayerPrefs.GetString("Player"));
        Scrolls[0].value = (float)CurrentPlayer.Speed / (1 + (float)Values.playerSpeed);
        Scrolls[1].value = (float)CurrentSettings.SpeedRatio / (1 + (float)Values.obsSpeed);
        Scrolls[2].value = ((float)CurrentSettings.TotalTime - 30 )/  ((float)Values.time );
        Scrolls[3].value = ((float)CurrentSettings.MovePerTime ) / (1 + (float)Values.rotate);
        btnOkey.onClick.AddListener(ClickOkey);
    }
    private void ClickOkey()
    {
        PlayerStats playerStats = new PlayerStats()
        {
            Id = 1,
            Speed = (int)(1 + (Scrolls[0].value * (int)Values.playerSpeed))
        };
        GameSettings gameSettings = new GameSettings()
        {
            BotNumber = CurrentSettings.BotNumber,
            TotalTime = (int)(30 + (Scrolls[2].value * (int)Values.time)),
            MovePerTime = (int)(1 + (Scrolls[3].value * (int)Values.rotate)),
            SpeedRatio = (int)(1 + (Scrolls[1].value * (int)Values.obsSpeed))
        };
        PlayerPrefs.SetString("GameSettings", JsonUtility.ToJson(gameSettings));
        PlayerPrefs.SetString("Player", JsonUtility.ToJson(playerStats));
    }
    // Update is called once per frame
    void Update()
    {
        tmpValues[0].SetText("(" + (int)(1 + (Scrolls[0].value * (int)Values.playerSpeed)) + ")");
        tmpValues[1].SetText("(" + (int)(1 + (Scrolls[1].value * (int)Values.obsSpeed)) + ")");
        tmpValues[2].SetText("(" + (int)(30 + (Scrolls[2].value * ((int)Values.time))) + ")");
        tmpValues[3].SetText("(" + (int)(1 + (Scrolls[3].value * (int)Values.rotate)) + ")");
    }
}
