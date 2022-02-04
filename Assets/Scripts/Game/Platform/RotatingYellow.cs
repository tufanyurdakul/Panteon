using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingYellow : MonoBehaviour
{
    private Game game;
    private GameSettingsModel gameSettings;
    void Start()
    {
        gameSettings = GameObject.Find("GameScript").GetComponent<GameSettingsModel>();
        game = GameObject.Find("GameScript").GetComponent<Game>();
        StartCoroutine(Rotator());
    }
    IEnumerator Rotator()
    {
        while (!game.GameEnd)
        {
            yield return new WaitForSeconds(0.05f);
            transform.Rotate(0, 0, -gameSettings.MovePerTime);
        }
    }
}
