using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
[RequireComponent(typeof(SphereCollider))]
public class EnemyAI : Health
{
    //set in unity
    public AudioClip audio_attack, audio_dead;
    public GameObject dmg;
    Collider doubleDamg;
    public string[] tags;
    public int damage, atkrange, dot,dotp;
    public float attackRate;

    protected short timerF, timerP, timerC;
    protected Health buildscript;
    protected Transform target;
    protected float nextAttack, offx, offz;
    protected bool attacking, burning, frostbitten, poisoned, is2d = true;
    [HideInInspector]
    public Animator m_Animator;
    protected AudioSource audiosource;
    [HideInInspector]
    public UnityEngine.AI.NavMeshAgent agent;
    protected short attacks;
    public string savedName;
    Movement cursor;
	Color resetColor;

    public override void Start()
    {
        base.Start();
        cursor = GameObject.Find("cursor").GetComponent<Movement>();
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.updateRotation = false;
        // get the components on the object we need ( should not be null due to require component so no need to check )
        audiosource = cursor.GetComponent<AudioSource>();
        m_Animator = GetComponentInChildren<Animator>();
        GetNearestTarget();
		resetColor = GetComponent<SpriteRenderer>().color;
    }

    virtual public void Attack() { }

    virtual public void DealDamage()//called from animator
    {
        if (target != null && currhp != 0)
        {
            if (audio_attack == null)
                audiosource.PlayOneShot(cursor.audio_attacks[(int)Random.Range(0, cursor.audio_attacks.Length)]);
            else
                audiosource.PlayOneShot(audio_attack);
            buildscript.TakeDamage(damage, Color.red);
            if (GetDistance(target.position.x + offx, target.position.z + offz) > atkrange)
            {
                attacking = false;
                buildscript.occupied = false;
                buildscript.occupiedbits &= ~occupiedbit;
                agent.Resume();
                m_Animator.SetBool("IsMoving", true);
            }
        }
    }

    public override void FixedUpdate()
    {
        if (is2d)
        {
            Vector3 lookat = camerapos.position;
            lookat.y = 0;
            transform.LookAt(lookat);
            float angle = Vector3.Dot((agent.destination - transform.position), camerapos.right);
            if (Mathf.Abs(angle) > .1f)
            {
                scale.x = Mathf.Abs(scale.x) * Mathf.Sign(angle);
                transform.localScale = scale;
            }
        }
        if (currhp == 0)
            return;
        base.FixedUpdate();
        if (frostbitten || burning)
        {
            ++timerC;
            ++timerF;
            if (timerF == 80)
            {
                if (burning)
                    TakeDamage(dot, Color.red);
                if (frostbitten)
                    TakeDamage(dot, Color.blue);
                timerF = 0;
            }
            if(timerC == 400)
            {
                frostbitten = false;
                burning = false;
                GetComponent<SpriteRenderer>().color = resetColor;
                agent.speed *= 2;
                attackRate /= 2;
                timerC = 0;
            }
        }

        if (poisoned)
        {
            ++timerP;
            if (timerP % 60 == 0)
            {
                TakeDamage((short)(armor + dotp), Color.green);
                if (timerP == 600)
                {
                    poisoned = false;
                    timerP = 0;
                    Destroy(transform.GetChild(1).gameObject);
                    if (!frostbitten && !burning)
                        GetComponent<SpriteRenderer>().color = resetColor;

                }

            }
        }

        if ((buildscript != null && buildscript.currhp == 0) || target == null)
        {
            GetNearestTarget();
            return;
        }

        if (attacking)
        {
            if (Time.time > nextAttack)
            {
                Attack();
                nextAttack = Time.time + attackRate;
                m_Animator.SetTrigger("Attack");
            }
        }
        else
        {

            if (buildscript != null && ((buildscript.occupiedbits & occupiedbit) != 0 || buildscript.occupied))
            {
                GetNearestTarget();
                return;
            }
            if (GetDistance(agent.destination.x, agent.destination.z) < atkrange + 1)//redudant check so we aren't wasting processor on setdestination
            {
                if (buildscript != null)
                    buildscript.occupiedbits |= occupiedbit;
                agent.Stop();
                attacking = true;
                m_Animator.SetBool("IsMoving", false);
                attacks = 0;
            }
            if(agent.destination.x != target.position.x + offx)
                agent.SetDestination(new Vector3(target.position.x + offx, 0, target.position.z + offz));
        }
    }


    protected float GetDistance(float _x, float _z)
    {

        return (Mathf.Abs(_x - transform.position.x) + Mathf.Abs(_z - transform.position.z));
    }

    void OnTriggerExit(Collider coll)
    {
        if (currhp != 0)
        {
            if (coll.tag == "Frost" && frostbitten)
            {
                frostbitten = false;
                if (!poisoned && !burning)
                    GetComponent<SpriteRenderer>().color = resetColor;
                agent.speed *= 2;
                attackRate /= 2;
                timerF = 0;
                timerC = 0;
            }
            if (coll.tag == "Napalm")
            {
                burning = false;
                if (!poisoned && !frostbitten)
                    GetComponent<SpriteRenderer>().color = resetColor;
                timerF = 0;
                timerC = 0;
            }
        }
    }

    //take damage
    void OnTriggerEnter(Collider coll)
    {
        if (currhp != 0)
        {
            if (coll.tag == "Frost" && !frostbitten)
            {
                dot = Mathf.CeilToInt(coll.GetComponent<Collisiondeathdetection>().magnitude * .17f);
                frostbitten = true;
                GetComponent<SpriteRenderer>().color = Color.blue;
                agent.speed *= .5f;
                agent.velocity.Normalize();
                attackRate *= 2;
                return;
            }

            if (coll.tag == "Poison")
            {
                dotp = Mathf.CeilToInt(coll.GetComponent<ProjectileScript>().magnitude*2);
                if (dotp > 10)
                    dotp = 10;
                if (!poisoned)
                {
                    GameObject poison = Instantiate(Resources.Load<GameObject>("ImPoisened"));
                    poison.transform.position = transform.position;
                    poison.transform.SetParent(transform);
                }
                poisoned = true;
                GetComponent<SpriteRenderer>().color = Color.green;
                return;
            }

            if (coll.tag == "Napalm")
            {
                dot = (int)Mathf.Ceil(coll.GetComponent<Collisiondeathdetection>().magnitude * .5f);
                burning = true;
                GetComponent<SpriteRenderer>().color = Color.red;
                return;
            }
            if (coll.tag == "Shield")
            {
                transform.position = coll.transform.position+(transform.position - coll.transform.position).normalized * coll.bounds.extents.x;
                return;
            }
            if (coll.tag == "Convert" && HP < 100 && savedName != "Drummer")
            {
                GetComponent<SpriteRenderer>().color = Color.magenta;
                resetColor = GetComponent<SpriteRenderer>().color;
                Movement.enemiesKilled += 1;
                Movement.buildings.Remove(this);
                GameControllerScript.converted.Add(this);
                tags = new string[1] { "Enemy" };
                GetNearestTarget();
                return;
            }
            if (coll.tag == "Blackhole" && HP < 100)
            {
                foreach (ParticleSystem parts in coll.GetComponentsInChildren<ParticleSystem>())
                    parts.Play();
                Movement.buildings.Remove(this);
                Movement.enemiesKilled += 1;
                Destroy(coll.gameObject,.5f);
                coll.tag = "Untagged";
                PlayerPrefs.SetInt(savedName, PlayerPrefs.GetInt(savedName) + 1);
                Destroy(gameObject);
                return;
            }
            if (coll.tag == "Spell" && doubleDamg != coll)
            {
                TakeDamage(Mathf.CeilToInt(coll.GetComponent<ProjectileScript>().damage), Color.red, coll.GetComponent<ProjectileScript>().magnitude);
                doubleDamg = coll;
            }
        }
    }

    public override void TakeDamage(int _dmg, Color _color, float _magnitude = 1)
    {
        if (currhp != 0)
        {
            Movement.shotsHit += 1.0f;
            int hp = currhp;
            base.TakeDamage(_dmg, _color);
            _dmg = hp - currhp;
            GameObject _text = (GameObject)Instantiate(dmg, Camera.main.WorldToScreenPoint(new Vector3(transform.position.x - 1, transform.localScale.y * GetComponent<SphereCollider>().radius * 4, transform.position.z)), Quaternion.identity);
            _text.GetComponent<UnityEngine.UI.Text>().text = (_dmg < 1 ? '+' + (-_dmg).ToString() : _dmg.ToString());
            _text.GetComponent<UnityEngine.UI.Text>().color = _color;
            if (armor > 0 && _dmg > 0 && !poisoned)
            {
                _text.transform.GetChild(0).gameObject.SetActive(true);
                audiosource.PlayOneShot(cursor.armor);
            }
            if (currhp < 1)
            {
                Movement.enemiesKilled += 1;
                is2d = true;
                if (buildscript != null)
                {
                    buildscript.occupiedbits &= ~occupiedbit;
                    buildscript.occupied = false;
                }
                target = null;
                    Movement.buildings.Remove(this);
                currhp = 0;
                float angle = Random.Range(0, 6.28f);
                Vector3 direction = new Vector3(transform.position.x + Mathf.Cos(angle), 0, transform.position.z + Mathf.Sin(angle));
                agent.velocity = (direction - transform.position).normalized *5* _magnitude;
                agent.acceleration = 5;
                if(agent.enabled)
                    agent.Stop();
                healthbar.gameObject.SetActive(false);
                if (audio_dead == null)
                    audiosource.PlayOneShot(cursor.audio_deaths[(int)Random.Range(0, cursor.audio_deaths.Length)]);
                else
                    audiosource.PlayOneShot(audio_dead);
                m_Animator.SetTrigger("IsDead");
            }
        }
    }


    // and finally the actual process for finding the nearest object:
    virtual public void GetNearestTarget()
    {
        target = null;
        attacking = false;
        m_Animator.SetBool("IsMoving", false);
        float distance = Mathf.Infinity;
        occupiedbit = 1;

        // loop through the  buildings, remembering nearest one found
        foreach (Health building in Movement.buildings)
        {
            bool unwanted = true;
            sbyte i = -1; while (++i != tags.Length)
                if (building.tag == tags[i]) { unwanted = false; break; }

            if (unwanted)
                continue;
            if (!building.occupied && building.currhp != 0)
            {
                float thisdistance = GetDistance(building.transform.position.x, building.transform.position.z);
                if (thisdistance < distance)
                {
                    target = building.transform;
                    distance = thisdistance;
                    buildscript = building;
                }
            }
        }
        if (target != null)
            GetStuff();
    }

    virtual public void GetStuff()
    {

        while ((buildscript.occupiedbits & occupiedbit) != 0)
            occupiedbit <<= 1;

        byte spawnspot = (byte)Mathf.Log(occupiedbit, 2);//the number of times the bit got bitshifted
        int avaiablespaces = buildscript.radius * 3f < 32 ? (int)(buildscript.radius * 3f) : 32;

        if (occupiedbit != 0 && spawnspot <= avaiablespaces)
        {
            m_Animator.SetBool("IsMoving", true);
            offx = buildscript.radius * Mathf.Cos(spawnspot * (2 * Mathf.PI / avaiablespaces));
            offz = buildscript.radius * Mathf.Sin(spawnspot * (2 * Mathf.PI / avaiablespaces));
            if (agent.enabled)
            {
                agent.Resume();
                agent.SetDestination(new Vector3(target.position.x + offx, 0, target.position.z + offz));
            }
        }
        else
        {
            buildscript.occupied = true;
            GetNearestTarget();
        }


    }


    virtual public void Die()//called form animator
    {
        PlayerPrefs.SetInt(savedName, PlayerPrefs.GetInt(savedName) + 1);
        //if(gameObject.)
        SpriteRenderer deadSprite = new GameObject("deadcopy", typeof(SpriteRenderer)).GetComponent<SpriteRenderer>();//create a gameobject with a sprite renderer
        SpriteRenderer mysprite = GetComponent<SpriteRenderer>();
        deadSprite.sprite = mysprite.sprite;
        deadSprite.transform.position = new Vector3(transform.position.x,.1f,transform.position.z);
        deadSprite.transform.localScale = transform.localScale;
        Vector3 lookat = camerapos.position;
        lookat.y = 1000;
        transform.LookAt(lookat);
        deadSprite.transform.rotation = transform.rotation;
        deadSprite.flipX = mysprite.flipX;
        deadSprite.color = mysprite.color;
        deadSprite.material = mysprite.material;
        deadSprite.sortingOrder = -2;
        Destroy(gameObject);
    }
}