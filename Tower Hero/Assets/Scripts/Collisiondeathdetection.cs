using UnityEngine;

public class Collisiondeathdetection : MonoBehaviour {
    public float magnitude;
	void OnDestroy()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, GetComponent<SphereCollider>().radius * transform.localScale.x+10);
        sbyte i = -1; while (++i < hitColliders.Length)
        {
            hitColliders[i].SendMessage("OnTriggerExit", GetComponent<SphereCollider>(), SendMessageOptions.DontRequireReceiver);
        }
    }
}