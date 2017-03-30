using UnityEngine;

public class FallingMeteorScript : MonoBehaviour {

    public GameObject explosion, terrain;
    AudioSource audiosor;
    public AudioClip explo;
    public float magnitude;

    // Use this for initialization
    void Start () {
	    audiosor = GameObject.Find("cursor").GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        transform.position = new Vector3(transform.position.x, transform.position.y - 2.5f, transform.position.z);
        if (transform.position.y < 0)
        {
            foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
                enemy.GetComponent<EnemyAI>().TakeDamage(10, Color.red);
            Camera.main.GetComponent<CameraShake>().PlayShake(magnitude);
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
            Instantiate(explosion).transform.position = transform.position;
            audiosor.PlayOneShot(explo);
            terrain = Instantiate(terrain);
            terrain.transform.position = transform.position;
            terrain.transform.localScale = new Vector3(5, 5, 1);
            Destroy(gameObject);
        }
	}
}
