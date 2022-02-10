using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BoyMovement : MonoBehaviour, IDragHandler, IPointerClickHandler
{
    public Camera cam;
    public Image percentageBg, percentage;
    public List<Texture2D> brush;
    public TextMeshProUGUI tmpPercantage;
    public Button ChangeBrush, ChangeCamera;
    private PlayerModel playerModel;
    private GameObject gameScript, player;
    private Game game;
    private GameSettingsModel gameSettings;
    private PlayerOrder playerOder;
    private Scale scale;
    private RaycastHit hit;
    private Ray ray;
    private Vector3 Offset, newLocation;
    private bool move, cameraSetted, choosenCamera;
    private Animator anim;
    private Transform EndTransform, paintTransform, paintTransform2, swipeTransform, redline, textTransform;
    private Vector3 firstTextPosition;
    private Texture2D txt;
    private int choosenBrush;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");//Find Player To Move Or Other
        playerModel = player.GetComponent<PlayerModel>();//Get Values Like Speed
        gameScript = GameObject.Find("GameScript");//To Fing GameScript
        playerOder = gameScript.GetComponent<PlayerOrder>();
        game = gameScript.GetComponent<Game>();//For General Rules Of Game Like Finish Time,Drawing Time
        gameSettings = gameScript.GetComponent<GameSettingsModel>();//For Game Settings
        Offset = cam.transform.position - player.transform.position;//Get First Camera Position(Distance Between Player And Camera)
        anim = player.GetComponent<Animator>();//Change Of Player Animation(Walking Or Idle)
        EndTransform = GameObject.FindGameObjectWithTag("Finish").GetComponent<Transform>();//Finish Line's Transform
        redline = GameObject.FindGameObjectWithTag("Redline").GetComponent<Transform>();//Finish Line's Transform
        StartCoroutine(EndControl());//Drawing Control
        Transform[] positions = EndTransform.GetComponentsInChildren<Transform>(); //When Finish Find Transforms For Drawing
        scale = gameScript.GetComponent<Scale>();
        #region updateTextPosition
        //for update:: get text and set parent is null for not effect from character movement.
        textTransform = player.gameObject.GetComponentInChildren<TextMeshPro>().GetComponent<Transform>();
        firstTextPosition = textTransform.position;
        textTransform.SetParent(null);
        #endregion
        //brush = Resources.Load("Prefab/Brush/Brush") as GameObject;
        foreach (var item in positions)
        {
            //Change Camera Position For Drawing
            if (item.name == "CameraPosition2")
                paintTransform = item;
            if (item.name == "CameraPosition")
                paintTransform2 = item;
            //Trail Object Transform For Drawing
            else if (item.name == "PaintObject")
                swipeTransform = item;
        }
        MeshRenderer mesh = swipeTransform.gameObject.GetComponent<MeshRenderer>();
        txt = new Texture2D(1024, 768, TextureFormat.ARGB32, false);
        mesh.material.mainTexture = txt;
        ChangeBrush.onClick.AddListener(BrushChange);
        ChangeCamera.onClick.AddListener(CameraChange);
    }
    public void OnDrag(PointerEventData eventData)
    {
        //If game is not over or finish you can move
        if (!game.Painting && !game.GameEnd)
            Move();
        //if game is finishing and set to camera for drawing
        else if (game.Painting && !cameraSetted)
            SetCameraPositionForPaint(paintTransform);
        //if game is not over and camera is setted than you can drawing
        else if (game.Painting && cameraSetted && game.Painting && !game.GameEnd)
        {
            ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 1000))
            {
                #region Brush
                Color32[] textPixels = txt.GetPixels32();
                Color32[] brushPixels = brush[choosenBrush].GetPixels32();
                Vector2Int brushes = new Vector2Int(brush[choosenBrush].width / 2, brush[choosenBrush].height / 2);
                float X = hit.textureCoord.x * (float)txt.width;
                float Y = hit.textureCoord.y * (float)txt.height;
                for (int i = 0; i < brush[choosenBrush].width; i++)
                {
                    int xPos = i - brushes.x + (int)X;
                    if (xPos < 0 || xPos >= txt.width)
                        continue;
                    for (int j = 0; j < brush[choosenBrush].height; j++)
                    {
                        int yPos = j - brushes.y + (int)Y;
                        if (yPos < 0 || yPos >= txt.height)
                            continue;
                        int tPos = xPos + (txt.width * yPos);

                        if (brushPixels[i + (j * brush[choosenBrush].width)].g < textPixels[tPos].g)
                        {
                            textPixels[tPos] = brushPixels[i + (j * brush[choosenBrush].width)];
                        }
                    }
                }
                txt.SetPixels32(textPixels);
                txt.Apply();
                #endregion
                #region Percantage
                textPixels = txt.GetPixels32();
                int count = 0;
                foreach (var item in textPixels)
                {
                    if (item.r > 225)
                    {
                        count++;
                    }
                }
                percentage.fillAmount = (float)count / (float)txt.GetPixels32().Length;
                tmpPercantage.SetText($"{(Mathf.Ceil(percentage.fillAmount * 10000) / 10000) * 100}%");
                #endregion
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!game.Painting && !game.GameEnd)
            Move();
        else if (game.Painting && !cameraSetted)
            SetCameraPositionForPaint(paintTransform);
    }

    private void Move()
    {
        //Move Function
        ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 1000))
        {
            newLocation = hit.point;
            player.transform.LookAt(new Vector3(hit.point.x, player.transform.position.y, hit.point.z));
            move = true;
        }
        anim.SetBool("isWalk", move);
    }
    private void SetCameraPositionForPaint(Transform cameraTransform)
    {
        cameraSetted = true;
        // set camera for drawing
        cam.transform.position = cameraTransform.position;
        cam.transform.rotation = cameraTransform.rotation;
        if (!percentageBg.gameObject.activeSelf)
            percentageBg.gameObject.SetActive(true);
        if (!ChangeCamera.gameObject.activeSelf)
            ChangeCamera.gameObject.SetActive(true);
        if (!ChangeBrush.gameObject.activeSelf)
            ChangeBrush.gameObject.SetActive(true);
    }
    void Update()
    {
        //update:: For not rotating and do not effect to character position,id object is not parent of player and manually change position.
        textTransform.position = new Vector3(player.gameObject.transform.position.x, firstTextPosition.y, player.gameObject.transform.position.z);
        // if touch or move command is given by user than move without game is over or finish
        if (move && !game.GameEnd && !game.Painting)
        {
            player.gameObject.transform.position = Vector3.MoveTowards(player.transform.position, newLocation, playerModel.Speed * Time.deltaTime);

            if (player.transform.position == newLocation)
                move = false;
            anim.SetBool("isWalk", move);
        }
        
        //Camera Follows Player with offset
        if (!game.GameEnd && !game.Painting)
        {
            Vector3 targetPosition = player.transform.position + Offset;
            cam.transform.position = new Vector3(targetPosition.x, cam.transform.position.y, targetPosition.z);
        }
        //If game or paint , player don't stay in walk animation
        else if (game.GameEnd || game.Painting && anim.GetBool("isWalk"))
            anim.SetBool("isWalk", false);
        else if (game.Painting && !cameraSetted)
            SetCameraPositionForPaint(paintTransform);
    }
    IEnumerator EndControl()
    {
        //if finish the game player should drawing
        while (!game.Painting)
        {
            yield return new WaitForSeconds(0.1f);
            if (player.gameObject.transform.position.x < redline.position.x)
            {
                game.Painting = true;
                int yourTime = PlayerPrefs.GetInt("BestTime");
                if (yourTime > 0 && yourTime > game.Timer)
                    PlayerPrefs.SetInt("BestTime", (game.Timer));
                else if (yourTime == 0)
                    PlayerPrefs.SetInt("BestTime", (game.Timer));
                playerOder.Finishers.Add(1);
            }
        }
    }
    private void BrushChange()
    {
        if (choosenBrush == 0)
            choosenBrush = 1;
        else
            choosenBrush = 0;
    }
    private void CameraChange()
    {
        if (!choosenCamera)
            SetCameraPositionForPaint(paintTransform2);
        else
            SetCameraPositionForPaint(paintTransform);
        choosenCamera = !choosenCamera;
    }
}
