using UnityEngine;

public class BossProjectile : MonoBehaviour
{

    [SerializeField]
    GameObject explosion;// explosion2;
    [SerializeField]
    AudioClip explode;
    Vector3 castlepos;
    AudioSource audsource;
    FlashScreen flashscript;
    // Use this for initialization
    void Start()
    {
        GameObject player = GameObject.Find("cursor");
        flashscript = player.GetComponent<FlashScreen>();
        GameObject castle = GameObject.FindGameObjectWithTag("Gate");
        castlepos = castle.transform.position;
        audsource = GameObject.Find("cursor").GetComponent<AudioSource>();
        //audsource.PlayOneShot(fire);
        GetComponent<Rigidbody>().velocity = new Vector3(castlepos.x - transform.position.x, -transform.position.y, castlepos.z - transform.position.z).normalized * 100;
    }

    void FixedUpdate()
    {
        //float temp_distance = (GetDistance(castlepos.x, castlepos.z));
        if (transform.position.y <= 0)
        {
            audsource.PlayOneShot(explode);
            flashscript.Bang();
            transform.position = new Vector3(castlepos.x, transform.position.y, castlepos.z);
            if (explosion != null)
                Instantiate(explosion).transform.position = transform.position;
           // if (explosion2 != null)
           //     Instantiate(explosion2).transform.position = transform.position;
            Destroy(gameObject);
        }
    }

    protected float GetDistance(float _x, float _z)
    {
        return (Mathf.Abs(_x - transform.position.x) + Mathf.Abs(_z - transform.position.z));
    }
}
