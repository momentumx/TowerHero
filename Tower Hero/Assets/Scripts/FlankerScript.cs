using UnityEngine;

public class FlankerScript : EnemyAI {

    bool falling = true;
    // Use this for initialization
    public override void Start()
    {
        float angle = Random.Range(0, 2*Mathf.PI);
        transform.position = new Vector3(                            //sets the object in a certain angle around the position of this object
            GameObject.Find("GameController").transform.position.x + 35 * Mathf.Cos(angle),
            90,
            GameObject.Find("GameController").transform.position.z + 35 * Mathf.Sin(angle)
            );
        base.Start();
    }
    public override void FixedUpdate()
    {
        if (falling)
        {
            transform.LookAt(camerapos);
            transform.position = new Vector3(transform.position.x, transform.position.y - .1f, transform.position.z);
            if (transform.position.y < 15)
            {
                transform.position = new Vector3(transform.position.x, 0, transform.position.z);
                falling = false;
                agent.enabled = true;
                m_Animator.SetBool("IsMoving", true);
                m_Animator.SetTrigger("Drop");
                tag = "Enemy";
            }
            return;
        }
        base.FixedUpdate();
    }
}
