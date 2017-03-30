using UnityEngine;
using System.Collections;

public class RotateTree : MonoBehaviour
{

    protected Camera mycamera;
    // Use this for initialization
    void Start()
    {
        mycamera = Camera.main;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 temp = mycamera.transform.position;
        temp.y = 3;
        transform.LookAt(temp);
    }
}
