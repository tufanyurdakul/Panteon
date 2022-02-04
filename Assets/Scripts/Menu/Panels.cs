using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Panels : MonoBehaviour
{
    public Button btnStart,btnReset,btnBotPanel,btnGamePanel;
    public GameObject botPanel, gamePanel;
    void Start()
    {
        float ScalePercentage = ((float)Screen.width * 16) / ((float)Screen.height * 9);
        transform.localScale = new Vector3(transform.localScale.x * ScalePercentage, transform.localScale.y, transform.localScale.z);
        btnStart.onClick.AddListener(ClickStart);
        btnReset.onClick.AddListener(ClickReset);
        btnBotPanel.onClick.AddListener(ClickBotPanel);
        btnGamePanel.onClick.AddListener(ClickGamePanel);

    }
    private void ClickBotPanel()
    {
        if (!botPanel.activeSelf)
        {
            botPanel.SetActive(true);
            gamePanel.SetActive(false);
        }
    }
    private void ClickGamePanel()
    {
        if (!gamePanel.activeSelf)
        {
            botPanel.SetActive(false);
            gamePanel.SetActive(true);
        }
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
