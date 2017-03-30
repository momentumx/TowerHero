using UnityEngine;

public class ArcherAI : EnemyAI
{
    [SerializeField]
    GameObject arrow;
    Transform wagon;
    Vector3 middle;
    [SerializeField]
    byte range2;

    void Awake()
    {
        wagon = GameObject.Find("Wagon").transform;
        middle = GameObject.Find("GameController").transform.position;
    }

    public void ddmg()
    {
        DealDamage();
    }

    public override void DealDamage()
    {
        if (target == null)
            return;
        audiosource.PlayOneShot(audio_attack);
        GameObject temp = Instantiate(arrow);
        temp.transform.position = new Vector3(transform.position.x, 3, transform.position.z);
        temp.GetComponent<BoltScript>().damage = damage;

        if (GetDistance(wagon.position.x, wagon.position.z) < range2 && tag != "Untagged" && wagon.tag !="Untagged")
            temp.GetComponent<BoltScript>().initialize(wagon);
        else
            temp.GetComponent<BoltScript>().initialize(target);

    }

    public override void FixedUpdate()
    {
        if (GetDistance(middle.x, middle.z) > 198)
        {
            transform.position += (middle - transform.position).normalized*.2f;
            Vector3 lookat = camerapos.position;
            lookat.y = 0;
            transform.LookAt(lookat);
            return;
        }
        base.FixedUpdate();
    }

    //public override void GetStuff()
    //{
    //    m_Animator.SetBool("IsMoving", true);
    //    agent.SetDestination(new Vector3(target.position.x, 0, target.position.z));
    //    agent.Resume();
    //}
}
