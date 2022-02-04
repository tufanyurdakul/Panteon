using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingPurple : MonoBehaviour
{
    // Start is called before the first frame update
    private Game game;
    private GameSettingsModel gameSettings;
    void Start()
    {
        game = GameObject.Find("GameScript").GetComponent<Game>();
        gameSettings = GameObject.Find("GameScript").GetComponent<GameSettingsModel>();
        StartCoroutine(Rotator());
    }
    IEnumerator Rotator()
    {
        while(!game.GameEnd)
        {
            yield return new WaitForSeconds(0.05f);
            transform.Rotate(0, 0, gameSettings.MovePerTime);
        }
    }
}
