using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ScorePanel : MonoBehaviour
{
    public List<TextMeshProUGUI> tmpScores;
    public Button Send, SendWithNickName;
    public GameObject NickName;
    public TMP_InputField input;
    // Start is called before the first frame update
    void Start()
    {
        Send.onClick.AddListener(SendClick);
        SendWithNickName.onClick.AddListener(SendClick2);
        StartCoroutine(Get());
    }
    IEnumerator Get()
    {
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("field1=foo&field2=bar"));
        UnityWebRequest www = UnityWebRequest.Get("http://www.townclubriva.click/score.php");
        yield return www.SendWebRequest();
        string newJson = string.Empty;

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            char[] y = System.Text.Encoding.UTF8.GetString(www.downloadHandler.data).ToCharArray();
            foreach (var item in y)
            {

                newJson += item;
            }
        }
        newJson = "{ \"score\": " + newJson + "}";
        Score score = new Score();
        Root<Score> root = new Root<Score>();
        root = JsonUtility.FromJson<Root<Score>>(newJson);
        for (int i = 0; i < root.score.Length; i++)
        {
            tmpScores[i].SetText($"id : {root.score[i].user_id} - {root.score[i].user_name} - score: {root.score[i].user_score}");
        }
    }

    IEnumerator Upload(string nickname, string time, string work, int id)
    {
       
        WWWForm form = new WWWForm();
        if (id > 0)
            form.AddField("id", id.ToString());
        else
            form.AddField("nickname", nickname);
        form.AddField("score", time);
        form.AddField("myField", "myData");
        using (UnityWebRequest www = UnityWebRequest.Post("http://www.townclubriva.click/"+work, form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                string newJson = string.Empty;
                char[] y = System.Text.Encoding.UTF8.GetString(www.downloadHandler.data).ToCharArray();
                if (y.Length > 0)
                {
                    foreach (var item in y)
                    {

                        newJson += item;
                    }
                    newJson = newJson.Replace("[", "");
                    newJson = newJson.Replace("]", "");
                    Id yourid = new Id();
                    yourid = JsonUtility.FromJson<Id>(newJson);
                    PlayerPrefs.SetInt("id", int.Parse(yourid.user_id));
                }
            }
        }



    }
    private void SendClick()
    {
        int id = PlayerPrefs.GetInt("id");
        if (id == 0)
            NickName.SetActive(true);
        else
        {
            StartCoroutine(Upload(string.Empty, PlayerPrefs.GetInt("BestTime").ToString(), "updatescore.php", PlayerPrefs.GetInt("id")));
            StartCoroutine(Get());
        }

    }
    private void SendClick2()
    {
        if (input.text.Length > 4 && PlayerPrefs.GetInt("BestTime") > 0)
        {
            StartCoroutine(Upload(input.text, PlayerPrefs.GetInt("BestTime").ToString(), "insertscore.php", 0));
            StartCoroutine(Get());
        }
        NickName.SetActive(false);
    }
}

[System.Serializable]
public class Score
{
    public string user_id;
    public string user_name;
    public string user_score;
}
[System.Serializable]
public class Root<Score>
{
    public Score[] score;
}
[System.Serializable]
public class Id
{
    public string user_id;
}