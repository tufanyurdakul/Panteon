using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveHalfDonut : MonoBehaviour
{
    private Transform[] children;
    private Game game;
    void Start()
    {
        game = GameObject.Find("GameScript").GetComponent<Game>();
        children = gameObject.GetComponentsInChildren<Transform>();
        StartCoroutine(MoveX(children[1].position, children[2].position));
    }

    IEnumerator MoveX(Vector3 moveObject, Vector3 staticObject)
    {
        int plus = 1;
        while (!game.GameEnd)
        {
            float myRandom = Random.Range(0.5f, 2);
            yield return new WaitForSeconds(myRandom);
            if (moveObject.x <= staticObject.x)
                plus *= -1;
            if (moveObject.x >= staticObject.x)
                plus *= -1;
            children[1].Translate(new Vector3((float)plus / 5, 0, 0));
        }
    }
}
