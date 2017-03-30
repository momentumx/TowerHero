using UnityEngine;

public class ProjectileScript : MonoBehaviour
{

    [SerializeField]
    protected GameObject explosion, explosion2, terrain;
    [SerializeField]
    protected AudioClip explode;
    [HideInInspector]
    public Vector3 cursorpos;
    protected AudioSource audsource;
    [HideInInspector]
    public Rigidbody rigid;
    [HideInInspector]
    public bool goingDown;
    [HideInInspector]
    public float magnitude;
    public float damage, scale;
    float scaleMag;
    public float shake;
    // Use this for initialization
    virtual public void Start()
    {
        damage *= magnitude;
        scaleMag = magnitude * .1f;
        if (scaleMag < 1)
            scaleMag = 1;
        scale *= scaleMag;

        transform.localScale = new Vector3(scale, scale, scale);
        foreach (ParticleSystem particles in GetComponentsInChildren<ParticleSystem>())
            particles.startSize = scale;

        rigid = GetComponent<Rigidbody>();
        GameObject cursor = GameObject.Find("cursor");
        cursorpos = cursor.transform.position;
        audsource = cursor.GetComponent<AudioSource>();
        //audsource.PlayOneShot(fire);
        rigid.velocity = new Vector3(cursorpos.x - transform.position.x, -transform.position.y, cursorpos.z - transform.position.z).normalized * 300;
        if (GameObject.Find("AirBoss(Clone)") != null)
        {
            AirBoss.spells.Add(gameObject);
        }
    }

    public virtual void FixedUpdate()
    {
        if (transform.position.y <= 0)
        {
            audsource.PlayOneShot(explode);
            Camera.main.GetComponent<CameraShake>().PlayShake(shake*magnitude*.2f);
            transform.position = new Vector3(cursorpos.x, 0, cursorpos.z);

            if (terrain != null)
            {
                terrain = Instantiate(terrain);
                terrain.transform.position = transform.position;
                terrain.transform.localScale = new Vector3(scale, scale, 1);
            }

            GameObject explo = Instantiate(explosion);
            explo.transform.position = transform.position;
            explo.transform.localScale *= scaleMag;
            foreach (ParticleSystem particles in explo.GetComponentsInChildren<ParticleSystem>())
                particles.startSize *= scaleMag;

            if (explosion2 != null)
            {
                GameObject explo2 = Instantiate(explosion2);
                explo2.transform.position = transform.position;
                explo2.transform.localScale *= scaleMag;
                foreach (ParticleSystem particles in explo2.GetComponentsInChildren<ParticleSystem>())
                    particles.startSize *= scaleMag;
                if (explo2.GetComponent<Collisiondeathdetection>())
                    explo2.GetComponent<Collisiondeathdetection>().magnitude = magnitude;
            }
            if (GameObject.Find("AirBoss(Clone)") != null)
            {
                if (AirBoss.spells.Contains(gameObject))
                {
                    AirBoss.spells.Remove(gameObject);
                }
            }
            Destroy(gameObject);

        }
    }
}
