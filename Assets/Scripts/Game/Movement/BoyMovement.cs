using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BoyMovement : MonoBehaviour, IDragHandler, IPointerClickHandler, IPointerDownHandler
{
    public Camera cam;
    private PlayerModel playerModel;
    private GameObject gameScript, player, paintObject, copyPaint;
    private Game game;
    private GameSettingsModel gameSettings;
    private PlayerOrder playerOder;
    private Scale scale;
    private RaycastHit hit;
    private Ray ray;
    private Vector3 Offset, newLocation;
    private bool move, paintDown,notSendFinish;
    private Animator anim;
    private Transform EndTransform, paintTransform, swipeTransform,redline;
    private void Start()
    {
        paintDown = true;
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
        paintObject = Resources.Load("Prefab/Brush/SwipeTransfrom") as GameObject;//For New Trail Position with Copy Object
        Transform[] positions = EndTransform.GetComponentsInChildren<Transform>(); //When Finish Find Transforms For Drawing
        scale = gameScript.GetComponent<Scale>();
        Debug.Log(scale.ScalePercentage);
        foreach (var item in positions)
        {
            //Change Camera Position For Drawing
            if (item.name == "CameraPosition")
                paintTransform = item;
            //Trail Object Transform For Drawing
            else if (item.name == "PaintObject")
                swipeTransform = item;
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        //If game is not over or finish you can move
        if (!game.Painting && !game.GameEnd)
            Move();
        //if game is finishing and set to camera for drawing
        else if (game.Painting && cam.transform.position != paintTransform.position)
            SetCameraPositionForPaint();
        //if game is not over and camera is setted than you can drawing
        else if (game.Painting && cam.transform.position == paintTransform.position && game.Painting && !game.GameEnd)
        {
            ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 1000))
            {
                Vector3 locate = hit.point;
                //If you mouse down is exit, than create new trail position.
                if (paintDown)
                {
                    copyPaint = Instantiate(paintObject);
                    copyPaint.transform.position = locate;
                    copyPaint.transform.SetParent(swipeTransform);
                    paintDown = false;
                }
                else
                {
                    float yLower = (cam.transform.position.y - (cam.transform.position.y / 2) - 0.2f);
                    float yUp = (cam.transform.position.y + (cam.transform.position.y / 2) + 0.2f);
                    float zLower = (cam.transform.position.z - (1.2f * scale.ScalePercentage)) ;
                    float zUp = (cam.transform.position.z + (1.2f * scale.ScalePercentage)) ;
                    //if mouse is out of camera position,do not move paint object
                    if (locate.y >= yLower && locate.y <= yUp && locate.z >= zLower && locate.z <= zUp)
                        copyPaint.transform.position = locate;//if drag is continous than change same object transfrom to draw.
                }
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!game.Painting && !game.GameEnd)
            Move();
        else if (game.Painting && cam.transform.position != paintTransform.position)
            SetCameraPositionForPaint();
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
    private void SetCameraPositionForPaint()
    {
        // set camera for drawing
        cam.transform.position = paintTransform.position;
        cam.transform.rotation = paintTransform.rotation;
    }
    void Update()
    {
        // if touch or move command is given by user than move without game is over or finish
        if (move && !game.GameEnd && !game.Painting)
        {
            player.transform.position = Vector3.MoveTowards(player.transform.position, newLocation, playerModel.Speed * Time.deltaTime);

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
        else if (game.Painting && cam.transform.position != paintTransform.position)
            SetCameraPositionForPaint();
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
                playerOder.Finishers.Add(1);
            }
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        //if painting and you don't drag set new position for trail.
        if (game.Painting)
            paintDown = true;
    }
}
