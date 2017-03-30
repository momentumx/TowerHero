using UnityEngine;

public class WagonScript : Health
{
    GameObject bucket, target;
    UnityEngine.AI.NavMeshAgent agent;
    float startx, startz;
    [HideInInspector]
    public float timer;
    [SerializeField]
    float respawnTime;
    enum State
    {
        Finding,
        Returning,
        Respawning,
        Waiting
    }
    State mystate = State.Finding;

	public int manaGain = 50;


    // Use this for initialization
    void Awake()
    {
        //respawnTime = 1 / respawnTime;
        startx = transform.position.x;
        startz = transform.position.z;
        bucket = transform.GetChild(1).gameObject;
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        GetNearestTarget();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if (htimer != 0)
        {
            ++htimer;
            if (htimer == 5)
            {
                htimer = 0;
                if(currhp > 0)
                    healthbar.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(0, 1, 0, .5f);
            }
        }
        if (target == null || !target.activeSelf)
            GetNearestTarget();

        healthbar.LookAt(Camera.main.transform);

        if (mystate == State.Respawning)
        {
            ++timer;
            healthbar.GetChild(0).localScale = new Vector2(timer / (respawnTime) * .9f, healthbar.GetChild(0).localScale.y);
            if (timer >= respawnTime)
            {
                healthbar.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(0, 1, 0, .5f);
                healthbar.gameObject.SetActive(false);
                agent.Resume();
                GetNearestTarget();
                timer = 0;
                tag = "Wagon";
                mystate = State.Finding;
                currhp = HP;
            }

        }
    }
    public override void TakeDamage(int _dmg, Color _color, float _magnitude = 1)
    {
        if (currhp != 0)
        {
            healthbar.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, .5f);
            htimer = 1;
            base.TakeDamage(_dmg, _color);
            if (currhp <= 0)
            {
                bucket.SetActive(false);
                mystate = State.Respawning;
                agent.Stop();
                healthbar.GetChild(0).GetComponent<SpriteRenderer>().color = Color.gray;
                transform.position = new Vector3(startx, transform.position.y, startz);
                currhp = 0;
                tag = "Untagged";
            }
        }
    }

    void OnTriggerEnter(Collider _other)
    {
        if (_other.gameObject == target)
        {
            switch (mystate)
            {
                case State.Finding:
                    target = GameObject.FindGameObjectWithTag("Gate");
                    if (target == null)
                        return;
                    agent.SetDestination(new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z));
                    mystate = State.Returning;
                    bucket.SetActive(true);
                    break;
                case State.Returning:
                    GetNearestTarget();
                    mystate = State.Finding;
                    bucket.SetActive(false);
                    Movement.mana += manaGain;
                    break;
            }
        }
    }

    void GetNearestTarget()
    {
        target = GameObject.FindGameObjectWithTag("Mana");
        if (target == null || !target.activeSelf)
            target = GameObject.FindGameObjectWithTag("Gate");
        if(target != null && target.activeSelf)
        agent.SetDestination(new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z));
    }
}