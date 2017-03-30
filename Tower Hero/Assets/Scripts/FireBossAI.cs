using UnityEngine;
using System.Collections;

public class FireBossAI : EnemyAI
{
    //public override void GetNearestTarget()
    //{
    //    base.GetNearestTarget();
    //    attackRate = 7;
    //}

    public override void DealDamage()
    {
        if (target != null && currhp != 0)
        {
            audiosource.PlayOneShot(audio_attack);
            buildscript.TakeDamage((byte)(damage + (byte)(attacks * damage)), Color.red);
            attacks += 1;
        }
    }
}
