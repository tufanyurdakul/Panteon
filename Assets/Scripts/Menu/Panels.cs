using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Net;
using System.IO;

public class Panels : MonoBehaviour
{
    public Button btnStart,btnReset,btnBotPanel,btnGamePanel,btnScorePanel;
    public GameObject botPanel, gamePanel,scorePanel;
    public TextMeshProUGUI tmpTimer;
    void Start()
    {
        float ScalePercentage = ((float)Screen.width * 16) / ((float)Screen.height * 9);
        transform.localScale = new Vector3(transform.localScale.x * ScalePercentage, transform.localScale.y, transform.localScale.z);
        btnStart.onClick.AddListener(ClickStart);
        btnReset.onClick.AddListener(ClickReset);
        btnBotPanel.onClick.AddListener(ClickBotPanel);
        btnGamePanel.onClick.AddListener(ClickGamePanel);
        btnScorePanel.onClick.AddListener(ClickScorePanel);
        int bestTime = PlayerPrefs.GetInt("BestTime");
        string writerTime = bestTime > 0 ? bestTime.ToString() : "60";
        tmpTimer.SetText("Best Time: "+writerTime);
       
    }
    private void ClickBotPanel()
    {
        if (!botPanel.activeSelf)
        {
            botPanel.SetActive(true);
            scorePanel.SetActive(false);
            gamePanel.SetActive(false);
        }
    }
    private void ClickGamePanel()
    {
        if (!gamePanel.activeSelf)
        {
            botPanel.SetActive(false);
            scorePanel.SetActive(false);
            gamePanel.SetActive(true);
        }
    }
    private void ClickScorePanel()
    {
        botPanel.SetActive(false);
        scorePanel.SetActive(true);
        gamePanel.SetActive(false);
    }
    private void ClickStart()
    {
        SceneManager.LoadScene("SampleScene");
    }
    private void ClickReset()
    {
        PlayerPrefs.SetInt("isFirst", 0);
        SceneManager.LoadScene("Menu");
    }
}
