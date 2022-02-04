using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitObject : MonoBehaviour
{
    private Vector3 firstPosition;
    private Rigidbody rb;
    void Start()
    {
        firstPosition = gameObject.transform.position;
        rb = gameObject.GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
            gameObject.transform.position = firstPosition;
        if (other.CompareTag("Donut"))
            gameObject.transform.position = firstPosition;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Water"))
            gameObject.transform.position = firstPosition;
        if (collision.gameObject.CompareTag("Rotator"))
            rb.AddForce(transform.forward * 10, ForceMode.Impulse);
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
}
