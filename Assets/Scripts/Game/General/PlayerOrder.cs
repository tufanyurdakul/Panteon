using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class PlayerOrder : MonoBehaviour
{
    // Start is called before the first frame update
    public List<TextMeshProUGUI> tmpOrders;
    public List<TextMeshProUGUI> tmpIds;
    public List<int> Finishers { get; set; }
    public Transform Finish;
    private Dictionary<int, float> AllPlayers;
    public List<KeyValuePair<int, float>> OrderedList { get; private set; }
    private GameObject[] bots;
    private Game game;
    void Start()
    {
        Finishers = new List<int>();
        game = GetComponent<Game>();
        bots = GameObject.FindGameObjectsWithTag("Bot");
        AllPlayers = new Dictionary<int, float>();
        StartCoroutine(GetAllPlayer());
    }
    IEnumerator GetAllPlayer()
    {
        //Bot is insantiated so bots cannot be null.If bots are exist you can order by x value
        yield return new WaitForSeconds(0.1f);
        bots = GameObject.FindGameObjectsWithTag("Bot");
        if (bots.Length > 0)
        {
            GetAllPlayers();
            StartCoroutine(SetOrder());
        }
        else
            StartCoroutine(GetAllPlayer());
    }
    private void GetAllPlayers()
    {
        AllPlayers = new Dictionary<int, float>();
        foreach (var item in bots)
        {
            BotModel botStat = item.gameObject.GetComponent<BotModel>();
            //if do not finish
            if (item.transform.position.x > Finish.position.x)
                AllPlayers.Add(botStat.Id, item.transform.position.x);
            else
            {
                //if finish x changes according to time
                for(int i = 0; i< Finishers.Count; i++)
                {
                    if (Finishers[i] == botStat.Id)
                        AllPlayers.Add(botStat.Id, - (100 - (i * 2)));
                }
            }
        }
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        // if player don't finish platform
        if (player.transform.position.x > Finish.position.x)
            AllPlayers.Add(1, player.transform.position.x);
        // if finish x of player changes according to time
        else
        {
            for (int i = 0; i < Finishers.Count; i++)
            {
                if (Finishers[i] == 1)
                    AllPlayers.Add(1, - (100 - (i * 2)));
            }
        }
    }
    private void WhiteText()
    {
        foreach (var item in tmpIds)
        {
            item.color = Color.white;
        }
    }
    IEnumerator SetOrder()
    {
        #region SortAndShowOrderWithText
        while (!game.GameEnd)
        {
            yield return new WaitForSeconds(0.1f);
            WhiteText();//Set All Text To White
            GetAllPlayers();
            OrderedList = AllPlayers.ToList();//Dictionary To List
            OrderedList.Sort((pair1, pair2) => pair1.Value.CompareTo(pair2.Value));//Sort By Value
            //show on text- center of five text is you.If you are last or last - 1 you cannot center.Also you are 1 or 2
            #region ShowText
            for (int i = OrderedList.Count - 1; i >= 0; i--)
            {
                if (OrderedList[i].Key == 1)
                {
                    if (i == OrderedList.Count - 1)
                    {
                        tmpIds[tmpIds.Count - 1].SetText("Y");
                        tmpIds[tmpIds.Count - 1].color = Color.red;
                        tmpOrders[tmpIds.Count - 1].SetText("" + (i + 1));
                        for (int h = 0; h < 4; h++)
                        {
                            tmpIds[3 - h].SetText("" + OrderedList[i - h - 1].Key);
                            tmpOrders[3 - h].SetText("" + (i - h));
                        }
                    }
                    else if (i == OrderedList.Count - 2)
                    {
                        tmpIds[tmpIds.Count - 2].SetText("Y");
                        tmpIds[tmpIds.Count - 2].color = Color.red;
                        tmpOrders[tmpIds.Count - 2].SetText("" + (i + 1));
                        for (int h = 0; h < 3; h++)
                        {
                            tmpIds[2 - h].SetText("" + OrderedList[i - h - 1].Key);
                            tmpOrders[2 - h].SetText("" + (i - h - 2));
                        }
                        tmpIds[4].SetText("" + OrderedList[i + 1].Key);
                        tmpOrders[4].SetText("" + (i + 2));
                    }
                    else if (i > 1 && i < OrderedList.Count - 2)
                    {
                        tmpIds[tmpIds.Count - 3].SetText("Y");
                        tmpIds[tmpIds.Count - 3].color = Color.red;
                        tmpOrders[tmpIds.Count - 3].SetText("" + (i + 1));
                        for (int h = 0; h < 2; h++)
                        {
                            tmpIds[1 - h].SetText("" + OrderedList[i - h - 1].Key);
                            tmpOrders[1 - h].SetText("" + (i - h));
                            tmpIds[h + 3].SetText("" + OrderedList[i + h + 1].Key);
                            tmpOrders[h + 3].SetText("" + (i + h + 2));
                        }
                    }
                    else if (i == 1)
                    {
                        tmpIds[i].SetText("Y");
                        tmpIds[i].color = Color.red;
                        tmpOrders[i].SetText("" + (i + 1));
                        for (int h = 0; h < 3; h++)
                        {
                            tmpIds[2 + h].SetText("" + OrderedList[i + h + 1].Key);
                            tmpOrders[2 + h].SetText("" + (i + h + 2));
                        }
                        tmpIds[0].SetText("" + OrderedList[0].Key);
                        tmpOrders[0].SetText("" + (i));
                    }
                    else if (i == 0)
                    {
                        tmpIds[0].SetText("Y");
                        tmpIds[0].color = Color.red;
                        tmpOrders[0].SetText("" + (i + 1));
                        for (int h = 0; h < 4; h++)
                        {
                            tmpIds[1 + h].SetText("" + OrderedList[h + 1].Key);
                            tmpOrders[1 + h].SetText("" + (h + 2));
                        }
                    }
                }
            }
            #endregion
        }
        #endregion
    }
}
