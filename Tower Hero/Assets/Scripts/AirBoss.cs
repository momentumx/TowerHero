using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AirBoss : EnemyAI {

    static public List<GameObject> spells;
    List<GameObject> spelltoremove;
    GameObject spelltodeflect;
    //bool deflecting = false;

    void Awake()
    {
        spelltoremove = new List<GameObject>();
        spells = new List<GameObject>();
    }
	public void Deflect()
    {
        if (spelltodeflect != null)
        {
            ProjectileScript deflectthis = spelltodeflect.GetComponent<ProjectileScript>();
            deflectthis.goingDown = false;
            float angle = Random.Range(Mathf.PI*.5f, Mathf.PI*1.5f);
            deflectthis.cursorpos = new Vector3(
                transform.position.x + 40 * Mathf.Cos(angle),
                0,
                transform.position.z + 40 * Mathf.Sin(angle)
                );
            deflectthis.rigid.velocity = new Vector3(deflectthis.cursorpos.x - deflectthis.transform.position.x, -deflectthis.transform.position.y, deflectthis.cursorpos.z - deflectthis.transform.position.z).normalized * 300;

        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if (spells != null)
        {
            foreach (GameObject spell in spells)
            {
                float spell_distance = Vector3.Distance(transform.position, spell.transform.position);
                if (spell_distance <= 75)
                {
                    if (Random.value > 0.5f && spell != spelltodeflect)
                    {
                        spelltodeflect = spell;
                        m_Animator.SetTrigger("Attack2");
                    }
                    else
                        spelltoremove.Add(spell);
                }
            }
            if (spelltoremove != null)
            {
                foreach (GameObject spell in spelltoremove)
                {
                    if (spells.Contains(spell))
                    {
                        spells.Remove(spell);
                    }
                }
            }
        }
    }
}
