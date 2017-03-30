using UnityEngine;

public class DmgTextScript : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        transform.SetParent(GameObject.Find("Canvas").transform);
        Destroy(gameObject, 1);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = new Vector2(transform.position.x, transform.position.y + 1);
    }
}
