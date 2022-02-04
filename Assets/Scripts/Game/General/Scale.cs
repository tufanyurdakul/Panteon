using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scale : MonoBehaviour
{
    public GameObject MovementImage;
    public GameObject PaintObject;
    public GameObject EndPanel;
    public float ScalePercentage { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        ScalePercentage = ((float)Screen.width * 16) / ((float)Screen.height * 9);
        PaintObject.transform.localScale = new Vector3(PaintObject.transform.localScale.x * ScalePercentage, PaintObject.transform.localScale.y, PaintObject.transform.localScale.z);
        MovementImage.transform.localScale = new Vector3(MovementImage.transform.localScale.x * ScalePercentage, MovementImage.transform.localScale.y, MovementImage.transform.localScale.z);
        EndPanel.transform.localScale = new Vector3(EndPanel.transform.localScale.x * ScalePercentage, EndPanel.transform.localScale.y, EndPanel.transform.localScale.z);

    }
}
