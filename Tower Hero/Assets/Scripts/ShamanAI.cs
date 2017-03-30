using UnityEngine;

public class ShamanAI : EnemyAI {

    int right;
    float angle;


    void Awake()
    {
        right = Random.Range(0, 2);
        if (right == 0)
            right = -1;
        target = GameObject.Find("GameController").transform;
        angle = Mathf.Atan2((transform.position.z-target.position.z), (transform.position.x - target.position.x));
    }

    public override void DealDamage()
    {
        m_Animator.SetBool("IsMoving", true);
        agent.Resume();
        attacking = false;
        GetNearestTarget();
        if (!poisoned)
        {
            GetComponent<SpriteRenderer>().color = Color.white;
            audiosource.PlayOneShot(audio_attack);
            foreach (Health enemy in Movement.buildings)
                if(enemy.tag == "Enemy")
                    ((EnemyAI)enemy).TakeDamage((short)-damage, Color.green);
        }

    }

    public override void GetNearestTarget()
    {

        angle += right * .1f;
        
        offx = 75 * Mathf.Cos(angle);
        offz = 75 * Mathf.Sin(angle);
        agent.SetDestination(new Vector3(target.position.x + offx, 0, target.position.z + offz));
        m_Animator.SetBool("IsMoving", true);

    }

    public override void Die()
    {
        PlayerPrefs.SetInt(savedName, PlayerPrefs.GetInt(savedName) + 1);
        Destroy(gameObject);
    }
}
