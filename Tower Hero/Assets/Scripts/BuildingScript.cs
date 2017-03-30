using UnityEngine;
public class BuildingScript : Health
{

    public GameObject rubble;

    public override void Start()
    {
        base.Start();
        scale = healthbar.GetChild(0).localScale;
        if (tag != "Mana")
        {
            int i = Random.Range(-1, 3); while (++i != 3)
            {
                float angle = Random.Range(0, 2 * Mathf.PI);
                ((GameObject)Instantiate(Resources.Load<GameObject>("Villager"),
                    new Vector3(transform.position.x + (radius + 10) * Mathf.Cos(angle), 0, transform.position.z + (radius + 10) * Mathf.Sin(angle)),
                    Quaternion.identity)).GetComponent<villagerScript>().runPos = transform.position;
            }
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if (htimer != 0)
        {
            ++htimer;
            if (htimer == 5)
            {
                htimer = 0;
                healthbar.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(0, 1, 0, .5f);
                healthbar.GetChild(0).localScale = new Vector2(currhp / (float)HP * .9f, scale.y);
            }
        }
    }

    public override void TakeDamage(int _dmg, Color _color, float _magnitude = 1)
    {
        if (currhp != 0)
        {
            base.TakeDamage(_dmg, Color.red);

            healthbar.GetChild(0).GetComponent<SpriteRenderer>().color = Color.red;
            healthbar.GetChild(0).localScale *= 3;
            htimer = 1;
            if(Movement.level < 10)
                GameObject.Find("GameController").GetComponent<GameControllerScript>().ContinueFlash(transform.position);
            else if(Movement.level == 10)
            {
                GameObject.Find("GameController").GetComponent<GameControllerBossWave>().ContinueFlash(transform.position);
            }

            if (currhp < 1)
            {
                currhp = 0;
                healthbar.gameObject.SetActive(false);
                if (rubble != null)
                {
                    rubble.SetActive(true);
                    if(tag != "Gate")
                        rubble.transform.position = transform.position;
                    gameObject.SetActive(false);
                }
                if (tag == "Gate")
                {
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                }
                if (currhp <= 0 && tag == "Gate")
                {
                    Movement.charging = false;
                    Movement.audiosor2.Stop();
                    GameObject.Find("CameraParent").GetComponent<CameraMovement>().Lose();
                }
            }
        }
    }
    public void Heal()
    {
        if (currhp == 0)
        {
            if (rubble != null)
            {
                rubble.SetActive(false);
                rubble.transform.position = transform.position;
                gameObject.SetActive(true);
            }
            if (healthbar.parent.gameObject == null)
            {
                healthbar.parent.gameObject.SetActive(true);
            }
        }
        currhp += (short)(HP *.5f);
        if (currhp > HP)
            currhp = HP;
        healthbar.GetChild(0).localScale = new Vector2((float)currhp / (float)HP * .9f, healthbar.GetChild(0).localScale.y);
        if (currhp == HP)
        {
            healthbar.gameObject.SetActive(false);
        }
        //healthbar.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(0, 255, 0, 128);
    }


}