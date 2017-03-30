using UnityEngine;

public class ShamanBossAI : EnemyAI {

    float nextAttack2;
    public float attackRate2;
    public AudioClip audio_attack2;

    public void DealDamage2()
    {
        if (!poisoned)
        {
            audiosource.PlayOneShot(audio_attack2);
            foreach (Health enemy in Movement.buildings)
                if(enemy.tag == "Enemy")
                    ((EnemyAI)enemy).TakeDamage((short)-20, Color.green);
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

    public override void Die()
    {
        PlayerPrefs.SetInt(savedName, PlayerPrefs.GetInt(savedName) + 1);
        Destroy(gameObject);
    }
}
