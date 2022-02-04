using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRotator : MonoBehaviour
{
    private Game game;
    void Start()
    {
        game = GameObject.Find("GameScript").GetComponent<Game>();
        StartCoroutine(Rotate());
    }

    IEnumerator Rotate()
    {
        while (!game.GameEnd)
        {
            yield return new WaitForSeconds(0.1f);
            transform.Rotate(0, 10, 0);
        }
    }
}
