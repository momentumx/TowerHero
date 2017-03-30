using UnityEngine;

public class UltmeteorScript : MonoBehaviour {

    float timer;
    GameObject fireball;
    AudioSource audiosor;

    void Start()
    {
        transform.position = GameObject.Find("GameController").transform.position;
        fireball = Resources.Load<GameObject>("Spells/Spell8Meteor");
        audiosor = GameObject.Find("cursor").GetComponent<AudioSource>();
    }

	void FixedUpdate () {
        ++timer;
	    if(timer%25 == 0)
        {
            float angle = Random.Range(0, 2 * Mathf.PI);
            float length = Random.Range(10, 160);
            Instantiate(fireball,
                new Vector3(transform.position.x+length*Mathf.Cos(angle), 100, transform.position.z + length * Mathf.Sin(angle)),
                Quaternion.identity);
            audiosor.Play();
            if (timer == 500)
                Destroy(gameObject);
        }

	}
}
