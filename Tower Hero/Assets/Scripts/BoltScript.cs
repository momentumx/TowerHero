using UnityEngine;

public class BoltScript : MonoBehaviour
{

    Rigidbody myrigidbogy;
    Transform target;
    [HideInInspector]
    public int damage;

    public void initialize(Transform _target)
    {
        Destroy(gameObject, 10);
        target = _target;
        float distz = (target.position.z - transform.position.z) *.5f;
        float distx = (target.position.x - transform.position.x) *.5f;
        myrigidbogy = GetComponent<Rigidbody>();
        myrigidbogy.velocity = new Vector3(distx, (Mathf.Abs(distx) + Mathf.Abs(distz)) * 2, distz).normalized * 50;
    }

    void FixedUpdate()
    {
        if ((target == null || !target.gameObject.activeSelf))
        {
            if (transform.position.y < 1)
                Destroy(gameObject);
            return;
        }
        myrigidbogy.rotation = Quaternion.LookRotation(myrigidbogy.velocity);
        myrigidbogy.velocity = Vector3.Lerp(
            myrigidbogy.velocity,
            target.position - transform.position,
            4.3f / (Mathf.Abs(target.position.x - transform.position.x) + Mathf.Abs(target.position.z - transform.position.z))).normalized * 50;

    }

    void OnTriggerEnter(Collider _other)
    {
        if (_other.GetComponent<Health>() && myrigidbogy.velocity.y < 0)
        {
            _other.GetComponent<Health>().TakeDamage(damage, Color.red);
            Destroy(gameObject);
        }
    }

    void OnTriggerStay(Collider _other)
    {
        if (_other.tag == "Shield")
        {
            myrigidbogy.angularVelocity = new Vector3(Random.Range(-1, 1f), Random.Range(-1, 1f), Random.Range(-1, 1f))*300;
            myrigidbogy.useGravity = true;
            myrigidbogy.velocity = (transform.position - _other.transform.position).normalized*20;
            target = null;
            return;
        }
    }
}
