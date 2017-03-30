using UnityEngine;
using System.Collections;

public class WaterBoss : EnemyAI {

    public AudioClip audio_attack2;
    public short boost;
    int buff_dist;
    int prev_buff_dist;
    Vector3 prev_dist;
    bool checking = true;

    // Use this for initialization
    public void Buff()
    {
        audiosource.PlayOneShot(audio_attack2);
        GameObject _text = (GameObject)Instantiate(dmg, Camera.main.WorldToScreenPoint(new Vector3(transform.position.x - 1, transform.localScale.x * GetComponent<SphereCollider>().radius * 4, transform.position.z)), Quaternion.identity);
        _text.GetComponent<UnityEngine.UI.Text>().text = boost.ToString();
        _text.GetComponent<UnityEngine.UI.Text>().color = Color.blue;
        damage += (byte)boost;
        if (attackRate > 1)
        {
            attackRate -= (byte)(boost / boost);
        }
        agent.speed += (byte)(boost / 10);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if (GameObject.FindGameObjectWithTag("Gate"))
        {
            if (!checking)
            {
                prev_buff_dist = (int)GetDistance(GameObject.FindGameObjectWithTag("Gate").transform.position.x,
                GameObject.FindGameObjectWithTag("Gate").transform.position.z);
            }
            else
            {
                prev_buff_dist = (int)GetDistance(GameObject.FindGameObjectWithTag("Gate").transform.position.x,
                GameObject.FindGameObjectWithTag("Gate").transform.position.z);
                buff_dist = prev_buff_dist - 50;
                checking = false;
            }
            if (prev_buff_dist <= buff_dist && prev_dist != transform.position)
            {
                prev_dist = transform.position;
                checking = true;
                m_Animator.SetTrigger("Attack2");
            }
        }
    }
}
