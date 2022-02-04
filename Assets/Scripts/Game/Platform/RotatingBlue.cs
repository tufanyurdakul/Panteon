using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingBlue : MonoBehaviour
{
    private Game game;
    private GameSettingsModel gameSettings;
    enum RotateWay
    {
        left = 0,
        right = 1
    }
    void Start()
    {
        game = GameObject.Find("GameScript").GetComponent<Game>();
        gameSettings = GameObject.Find("GameScript").GetComponent<GameSettingsModel>();
        int random = Random.Range((int)RotateWay.left, (int)RotateWay.right + 1);
        StartCoroutine(Rotator(random));
    }
    IEnumerator Rotator(int rotate)
    {
        int rotator = rotate == (int)RotateWay.left ? -gameSettings.MovePerTime : gameSettings.MovePerTime;
        while (!game.GameEnd)
        {
            yield return new WaitForSeconds(0.05f);
            transform.Rotate(0, 0, rotator);
        }
    }
}
