using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitObject : MonoBehaviour
{
    private Vector3 firstPosition;
    private PlayerModel playerModel;
    private BotModel botModel;
    private Rigidbody rb;
    private bool bot;
    private int firstSpeed;
    void Start()
    {
        firstPosition = gameObject.transform.position;
        rb = gameObject.GetComponent<Rigidbody>();
        playerModel = GetComponent<PlayerModel>();
        if (playerModel == null)
        {
            botModel = GetComponent<BotModel>();
            firstSpeed = botModel.Speed;
            bot = true;
        }
        else
            firstSpeed = playerModel.Speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
            gameObject.transform.position = firstPosition;
        if (other.CompareTag("Donut"))
            gameObject.transform.position = firstPosition;
        if (other.CompareTag("Rotator"))
            rb.AddForce(transform.forward * 50, ForceMode.Impulse);
        if (other.CompareTag("Ball"))
        {
            if (bot)
                botModel.Speed += 1;
            else
                playerModel.Speed += 1;
            Destroy(other.gameObject);
            StartCoroutine(SpeedIncreased());
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Water"))
            gameObject.transform.position = firstPosition;
        
        if (collision.gameObject.name == "Rotator")
            gameObject.transform.position = firstPosition;
        if (collision.gameObject.CompareTag("RotatePlatformPurple"))
            gameObject.transform.Translate(new Vector3(0.01f, 0, 0));
        if (collision.gameObject.CompareTag("RotatePlatformYellow"))
            gameObject.transform.Translate(new Vector3(-0.01f, 0, 0));
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("RotatePlatformPurple"))
            gameObject.transform.Translate(new Vector3(0.01f, 0,0));
        if (collision.gameObject.CompareTag("RotatePlatformYellow"))
            gameObject.transform.Translate(new Vector3(-0.01f, 0, 0));
    }
    IEnumerator SpeedIncreased()
    {
        yield return new WaitForSeconds(1.5f);
        if (bot)
            botModel.Speed = firstSpeed;
        else
            playerModel.Speed = firstSpeed;
    }
}
