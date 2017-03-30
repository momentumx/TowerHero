using UnityEngine;
using System.Collections;

public class villagerScript : MonoBehaviour
{
    public Vector3 runPos;
    Transform camerapos;
    public Sprite[] minis;
    public Sprite[] boxes;
    bool run;

    void Start()
    {
        runPos = (runPos - transform.position).normalized*.2f;
        Destroy(gameObject, 3);
        camerapos = Camera.main.transform;
        GetComponent<SpriteRenderer>().sprite = minis[Random.Range(0, minis.Length)];
        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = boxes[Random.Range(0, boxes.Length)];
    }

    void FixedUpdate()
    {
        Vector3 lookat = camerapos.position;
        lookat.y = 0;
        transform.LookAt(lookat);
        if (run)
            transform.position += runPos;
        
    }

    public void SetRun()
    {
        run = true;
    }
}
