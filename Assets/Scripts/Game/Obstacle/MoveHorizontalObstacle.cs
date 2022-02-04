using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveHorizontalObstacle : MonoBehaviour
{
    public List<Transform> obsTransform;
    private Game game;
    private GameSettingsModel gameSettings;
    enum Move
    {
        vertical = 0,
        horizantal = 1
    }
    void Start()
    {
        game = GameObject.Find("GameScript").GetComponent<Game>();
        gameSettings = GameObject.Find("GameScript").GetComponent<GameSettingsModel>();
        int random = Random.Range(0,2);
        if (random == (int)Move.vertical)
            StartCoroutine(MoveVertical());
        if (random == (int)Move.horizantal)
            StartCoroutine(MoveHorizantal());

    }
    IEnumerator MoveVertical()
    {
        int plus = 1;
        while (!game.GameEnd)
        {
            yield return new WaitForSeconds(0.02f);
            if (gameObject.transform.position.z <= obsTransform[0].position.z)
                plus *= -1;
            if (gameObject.transform.position.z >= obsTransform[1].position.z)
                plus *= -1;
            transform.Translate(new Vector3(0, 0, (float)plus / (20 - (float)gameSettings.SpeedRatio) ));
        }
    }
    IEnumerator MoveHorizantal()
    {
        int plus = 1;
        while (!game.GameEnd)
        {
            yield return new WaitForSeconds(0.02f);
            if (gameObject.transform.position.y <= obsTransform[0].position.y)
                plus *= -1;
            if (gameObject.transform.position.y >= obsTransform[1].position.y)
                plus *= -1;
            transform.Translate(new Vector3(0, (float)plus / (20 - (float)gameSettings.SpeedRatio)), 0);
        }
    }
}
