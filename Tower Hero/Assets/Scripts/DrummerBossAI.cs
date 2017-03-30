using UnityEngine;

public class DrummerBossAI : EnemyAI {

    float nextAttack2;
    public float attackRate2;
    public AudioClip audio_attack2;
    public float buff;

    public override void Start()
    {
        base.Start();
        nextAttack2 = attackRate2 + Time.time;
    }

    public void DealDamage2()
    {
        if (!poisoned)
        {
            audiosource.PlayOneShot(audio_attack2);
            foreach (Health enemy in Movement.buildings)
            {
                if (enemy.tag == "Enemy")
                {
                    ((EnemyAI)enemy).agent.speed += buff;
                    enemy.GetComponent<SpriteRenderer>().color = new Color(.8f, .1f, .1f);
                    if (((EnemyAI)enemy).attackRate > 1)
                        ((EnemyAI)enemy).attackRate -= 1;
                }
            }
        }

    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if (Time.time > nextAttack2)
        {
            nextAttack2 = Time.time + attackRate2;
            m_Animator.SetTrigger("Attack2");
        }
    }
}
