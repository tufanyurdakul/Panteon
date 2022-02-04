using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class BotMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform finish,redline;
    private BotModel botModel;
    private Game game;
    private PlayerOrder playerOrder;
    private GameObject gameScript;
    private bool notSendFinish;
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        gameScript = GameObject.Find("GameScript");//To Fing GameScript
        playerOrder = gameScript.GetComponent<PlayerOrder>();
        botModel = GetComponent<BotModel>();
        agent = GetComponent<NavMeshAgent>();
        finish = GameObject.FindGameObjectWithTag("GreenLine").GetComponent<Transform>();
        redline = GameObject.FindGameObjectWithTag("Redline").GetComponent<Transform>();
        agent.speed = botModel.Speed;
        game = gameScript.GetComponent<Game>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!game.GameEnd)
        {
            agent.SetDestination(finish.position);
        }
        else if (game.GameEnd)
        {
            agent.speed = 0;
            anim.SetBool("isWalk", false);
        }
        if (transform.position.x <= redline.position.x && !notSendFinish)
        {
            playerOrder.Finishers.Add(botModel.Id);
            notSendFinish = true;
        }
    }
}
