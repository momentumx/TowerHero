using UnityEngine;

public class Health : MonoBehaviour
{

    protected Transform healthbar, camerapos;
    [HideInInspector]
    public float radius;
    public short armor;
    public int HP;
    protected short htimer;
    [HideInInspector]
    public int currhp;
    [HideInInspector]
    public uint occupiedbits, occupiedbit;
    [HideInInspector]
    public bool occupied;
    protected Vector3 scale;



    // Use this for initialization
    virtual public void Start()
    {
        scale = transform.localScale;
        camerapos = Camera.main.transform;
        Movement.buildings.Add(this);
        radius = transform.localScale.x * GetComponent<SphereCollider>().radius;
        healthbar = transform.GetChild(0);
        currhp = HP;
    }

    virtual public void FixedUpdate()
    {
        healthbar.LookAt(camerapos);
    }

    virtual public void TakeDamage(int _dmg, Color _color, float _magnitude = 1)
    {
        if (currhp != 0)
        {
            if (_dmg > 0)
            {
                _dmg -= armor;
                if (_dmg <= 0)
                    _dmg = 1;
            }
            currhp -= _dmg;
            if (currhp >= HP)
            {
                currhp = HP;
                healthbar.gameObject.SetActive(false);
            }
            else {
                healthbar.gameObject.SetActive(true);
            }
            if(healthbar.GetChild(0) != null)
                healthbar.GetChild(0).localScale = new Vector2(currhp / (float)HP * .9f, healthbar.GetChild(0).transform.localScale.y);
        }
    }
    
}