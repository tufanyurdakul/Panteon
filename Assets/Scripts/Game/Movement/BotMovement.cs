using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
public class BotMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform redline, textTransform;
    private BotModel botModel;
    private Game game;
    private PlayerOrder playerOrder;
    private GameObject gameScript;
    private bool notSendFinish;
    private Animator anim;
    private Vector3 firstTextPosition, firstPosition;
    private List<AgentPattern> pattern;
    private int yourPattern, randomNumber,choosenPattern;
    private BotPatterns botPatterns;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        pattern = new List<AgentPattern>();
        gameScript = GameObject.Find("GameScript");//To Find GameScript
        playerOrder = gameScript.GetComponent<PlayerOrder>();
        botModel = GetComponent<BotModel>();
        agent = GetComponent<NavMeshAgent>();
        redline = GameObject.FindGameObjectWithTag("Redline").GetComponent<Transform>();
        agent.speed = botModel.Speed;
        game = gameScript.GetComponent<Game>();
        anim = GetComponent<Animator>();
        botPatterns = gameScript.GetComponent<BotPatterns>();
       
        //update:: firstPosition is for partially movement if hit obs. bot starts at path of partial 0. 
        firstPosition = gameObject.transform.position;
        rb = GetComponent<Rigidbody>();
        //get transforms for movement.
        for (int i = 1; i < 11; i++)
        {
            AgentPattern agentPattern = new AgentPattern();
            agentPattern.position = new List<Vector3>();
            for (int j = 1; j < 5; j++)
            {
                agentPattern.position.Add(GameObject.Find("Patterns/" + i + "/" + j).GetComponent<Transform>().position);
            }
            pattern.Add(agentPattern);
        }
        randomNumber = Random.Range(0, 4);
        choosenPattern = Random.Range(1, 8);
        #region updateTextPosition
        //for update:: get text and set parent is null for not effect from character movement.
        textTransform = GetComponentInChildren<TextMeshPro>().GetComponent<Transform>();
        firstTextPosition = textTransform.position;
        textTransform.SetParent(null);
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        //update:: For not rotating and do not effect to character position,id object is not parent of player and manually change position.
        textTransform.position = new Vector3(transform.position.x, firstTextPosition.y, gameObject.transform.position.z);
        if (!game.GameEnd)
        {
            //update:: For ai system...Partially Move
            //GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
            //float max = 1000;
            //foreach (var item in obstacles)
            //{
            //    if (Vector3.Distance(item.transform.position,transform.position) < max)
            //    {
            //        max = Vector3.Distance(item.transform.position, transform.position);
            //    }
            //}
            
            if (yourPattern < pattern.Count)
            {
                randomNumber = choosenPattern < 7 ? botPatterns.Patterns[choosenPattern][yourPattern] : randomNumber;
                if (transform.position.x - 1 <= pattern[yourPattern].position[randomNumber].x)
                {
                    yourPattern++;
                    randomNumber = Random.Range(0, 4);
                }
                else
                {
                    //if (max < 1 && yourPattern > 0)
                    //{
                    //    randomNumber = randomNumber < 3 && randomNumber >= 0 ? randomNumber + 1 : (randomNumber + 1) % 4;
                    //    agent.SetDestination(pattern[yourPattern - 1].position[randomNumber]);
                    //    agent.transform.LookAt(pattern[yourPattern - 1].position[randomNumber]);
                    //}
                    agent.SetDestination(pattern[yourPattern].position[randomNumber]);
                    agent.transform.LookAt(pattern[yourPattern].position[randomNumber]);
                }
            }
            if (rb.velocity.x > 10 || rb.velocity.y < -10)
            {
                rb.velocity = new Vector3(rb.velocity.x / 2, rb.velocity.y / 2, 0);
            }
            if (transform.position.x + 1 >= firstPosition.x)
            {
                yourPattern = 0;
                rb.velocity = new Vector3(0, 0, 0);
                agent.speed = botModel.Speed;
                agent.SetDestination(pattern[yourPattern].position[randomNumber]);
            }
        }
        else if (game.GameEnd)
        {
            agent.speed = 0;
            anim.SetBool("isWalk", false);
            rb.isKinematic = true;
        }
        if (transform.position.x <= redline.position.x && !notSendFinish)
        {
            rb.isKinematic = true;
            playerOrder.Finishers.Add(botModel.Id);
            notSendFinish = true;
            agent.speed = 0;
            anim.SetBool("isWalk", false);
            game.transform.position = redline.transform.position + new Vector3(-1, 0, 0);
        }
    }
}
public class AgentPattern
{
    public List<Vector3> position;
}
