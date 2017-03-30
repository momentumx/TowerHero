using UnityEngine;

public class dropscript : MonoBehaviour {
    
    Material circle;
    Color mycolor;
	// Use this for initialization
	void Start () {
        circle = transform.GetChild(0).GetComponent<MeshRenderer>().material;
        mycolor = circle.color;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        transform.position = new Vector3(transform.position.x + .01f, transform.position.y - .02f, transform.position.z);
        mycolor.a -= .03f;

        circle.color = mycolor;
        if (mycolor.a <= 0)
            Destroy(gameObject);
	}
}
