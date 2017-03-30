using UnityEngine;
using System.Collections;

public class Raider : EnemyAI {
    public override void GetNearestTarget()
    {
        target = null;
        attacking = false;
        m_Animator.SetBool("IsMoving", false);
        occupiedbit = 1;

        if (tags.Length == 0)
        {
            if (GameObject.Find("Wagon").activeSelf && GameObject.Find("Wagon").GetComponent<Health>().currhp > 0)
                target = GameObject.Find("Wagon").transform;
            else
            {
                if (GameObject.FindGameObjectWithTag("Gate"))
                    target = GameObject.FindGameObjectWithTag("Gate").transform;
            }
            if (target == null)
                return;
            buildscript = target.GetComponent<Health>();
            GetStuff();
        }
        else
            base.GetNearestTarget();
    }
}
