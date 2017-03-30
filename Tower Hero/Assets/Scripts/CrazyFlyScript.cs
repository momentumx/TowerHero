using UnityEngine;

public class CrazyFlyScript : ProjectileScript
{


    Vector3 direction, aim;
    float timer, next = .1f, acc;
    [HideInInspector]

    // Use this for initialization
    public override void Start()
    {
        base.Start();
        rigid.velocity = new Vector3(Random.Range(-1, 1f), Random.Range(-1, 1f), Random.Range(-1, 1f)).normalized * 200;
        timer = Time.time + next;
    }

    public override void FixedUpdate()
    {
        if (goingDown )
        {
            acc += .01f;
            if (acc > .9f)
                acc = .9f;
            direction = new Vector3(cursorpos.x - transform.position.x, -transform.position.y, cursorpos.z - transform.position.z).normalized * 300;
            base.FixedUpdate();
        }
        else
        {
            if (Time.time > timer)
            {
                direction = new Vector3(Random.Range(-1f, 1f), Random.Range(0, 1f), Random.Range(-1, 1f)).normalized * 200;
                timer = Time.time + next;
            }

            if (transform.position.y > 50)
                goingDown = true;

        }
        aim = (direction - rigid.velocity) * (.1f + acc);

        rigid.velocity += aim;

    }
}