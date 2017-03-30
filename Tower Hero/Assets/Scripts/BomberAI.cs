using UnityEngine;

public class BomberAI : EnemyAI
{
    public override void DealDamage()
    {
        base.DealDamage();
        m_Animator.SetTrigger("IsDead");
        if (tags[0] == "Enemy")
            GameControllerScript.converted.Remove(this);
        else
            Movement.buildings.Remove(this);
    }
}

