using UnityEngine;

public class Infiltrator : EnemyAI
{

    // Use this for initialization
    void Awake()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        m_Animator = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Mana").transform;
        if (target == null || !target.gameObject.activeSelf)
            target = GameObject.FindGameObjectWithTag("Spawnrubble").transform;
        //transform.position = target.position;
        transform.position = new Vector3(target.position.x - 3, 0, target.position.z);
        if (GameObject.FindGameObjectWithTag("Gate"))
            target = GameObject.FindGameObjectWithTag("Gate").transform;
        buildscript = target.GetComponent<BuildingScript>();
        occupiedbit = 1;
        GetStuff();

    }

    public override void GetNearestTarget()
    {
        is2d = false;
        agent.updateRotation = true;
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if (tags.Length > 0)
        {
            Instantiate(GameObject.Find("Wagon"), transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

    public override void DealDamage()
    {
        is2d = true;

        if (currhp != 0)
        {
            audiosource.PlayOneShot(audio_attack);
            buildscript.TakeDamage(damage, Color.red);
        }
    }

}
