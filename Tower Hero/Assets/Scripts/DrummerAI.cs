using UnityEngine;

public class DrummerAI : EnemyAI
{
    public float buff;
    int right;
    float angle;
    public override void DealDamage()
    {
        attacking = false;
        GetNearestTarget();
        if (!poisoned)
        {
            audiosource.PlayOneShot(audio_attack);
            foreach (Health enemy in Movement.buildings)
            {
                if (enemy.tag == "Enemy" && enemy!=this)
                {
                    ((EnemyAI)enemy).agent.speed += buff;
                    enemy.GetComponent<SpriteRenderer>().color = new Color(.8f, .1f, .1f);
                    ((EnemyAI)enemy).attackRate -= 1;
                }
            }
        }
        m_Animator.SetBool("IsMoving", true);
        agent.Resume();
    }


    void Awake()
    {
        right = Random.Range(0, 2);
        if (right == 0)
            right = -1;
        target = GameObject.Find("GameController").transform;
        angle = Mathf.Atan2((transform.position.z - target.position.z), (transform.position.x - target.position.x));
    }

    public override void GetNearestTarget()
    {

        angle += right * .1f;

        offx = 75 * Mathf.Cos(angle);
        offz = 75 * Mathf.Sin(angle);
        agent.SetDestination(new Vector3(target.position.x + offx, 0, target.position.z + offz));
        m_Animator.SetBool("IsMoving", true);

    }
}
