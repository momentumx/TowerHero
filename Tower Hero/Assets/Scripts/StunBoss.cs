using UnityEngine;
using System.Collections;

public class StunBoss : EnemyAI {

    float nextAttack2;
    public float attackRate2;
    public AudioClip audio_attack2;
    public GameObject Stun_charge;

    public void Stun()
    {
        audiosource.PlayOneShot(audio_attack2);
        Instantiate(Stun_charge).transform.position = new Vector3(transform.position.x, 5, transform.position.z);
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
