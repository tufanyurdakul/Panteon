using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameFinishPanel : MonoBehaviour
{
    private GameObject gameScript;
    public List<TextMeshProUGUI> tmpOrders;
    void Start()
    {
        gameScript = GameObject.Find("GameScript");
        PlayerOrder playerOrder = gameScript.GetComponent<PlayerOrder>();
        //Show on panel to order of players
        for (int i = 0; i < tmpOrders.Count; i++)
        {
            //if Id is yours.
            string id = playerOrder.OrderedList[i].Key == 1 ? "Y" : "" + playerOrder.OrderedList[i].Key;
            tmpOrders[i].SetText((i + 1)+" - "+id);
        }
        StartCoroutine(ChangeScene());
    }
    IEnumerator ChangeScene()
    {
        yield return new WaitForSeconds(4);
        SceneManager.LoadScene("Menu");
    }
}
