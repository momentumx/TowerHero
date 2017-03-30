using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Collections;



public class Player : MonoBehaviour
{
    static public EventSystem eventsys;

    //set in unity
    [SerializeField]
    GameObject options;
    [SerializeField]
    AudioClip error, fullcharge;
    static public AudioClip armor;
    [SerializeField]
    byte[] cost;
    //arrays for spells
    float cooldown, speed, spinTime;
    float[] cursorsizes;
    Vector3[] cursortransforms;
    ParticleSystem[] cursorparts;

    static public float maxMana, mana;
    static public byte[] spells = { 0, 1, 2, 3, 8};
    static public byte spell, timer;
    static public bool axisbeingused = false, charging, lost;
    static public List<Health> buildings;

    //components
    GameObject[] attacks;
    GameObject curcursor, castleparticle;
    Vector3 CastleParticlePos;
    Text cursorManaTxt;
    Button[] buttons;
    Rigidbody rb3d;
    // 1 for charging and one for blast
    static public AudioSource audiosor, audiosor2;//public static so others can turn it off.

    //Infinite Mana 
    float resetMana, multiplier, ultTimer = 20.0f;
    bool ultActive = false;

    //End Game Variables
    public static float levelTime = 0f, shotsTaken = 0f;
    public static uint enemiesKilled = 0U, shotsHit = 0u;

    void Start()
    {
        speed = PlayerPrefs.GetFloat ( MasterControllerScript.SAVEKEYFLT.Speed.ToString() );
        maxMana = PlayerPrefs.GetInt(MasterControllerScript.SAVEKEYINT.StartingManaUpgrade.ToString())*25 +200;
        mana = maxMana;

        Time.timeScale = 1;
        spell = 0;
        cursorManaTxt = GameObject.Find("mana").GetComponent<Text>();
        CastleParticlePos = GameObject.Find("Particle_castle").transform.position;
        attacks = new GameObject[4];
        buttons = new Button[5];
        sbyte i = -1; while (++i != 5)
        {
            buttons[i] = GameObject.Find("SpellSelect" + i).GetComponent<Button>();
            if (i != 4)
            {
                attacks[i] = Resources.LoadAll<GameObject>("Spells")[spells[i]];
                buttons[i].transform.GetChild(1).GetComponent<Text>().text = cost[spells[i]].ToString();
            }
            buttons[i].GetComponent<Image>().sprite = Resources.LoadAll<Sprite>("button sprites")[spells[i]];
            string[] temp = Input.GetJoystickNames();
            if (temp.Length == 0 || (temp.Length <= 1 && temp[0] == ""))
            {
                buttons[i].transform.GetChild(2).GetComponent<Text>().text = (i + 1).ToString();
            }
        }


        rb3d = GetComponent<Rigidbody>();
        audiosor = GetComponent<AudioSource>();
        audiosor2 = GetComponents<AudioSource>()[1];
        eventsys = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        //pointer = new PointerEventData(EventSystem.current);
        eventsys.SetSelectedGameObject(buttons[0].gameObject);
        eventsys.enabled = false;

        castleparticle = (GameObject)Instantiate(Resources.LoadAll<GameObject>("TowerParticles")[spells[spell]], CastleParticlePos, Quaternion.identity);
        curcursor = (GameObject)Instantiate(Resources.LoadAll<GameObject>("CursorParticles")[spells[spell]], transform.position, Quaternion.identity);
        curcursor.transform.parent = transform;

        Cursor.lockState = CursorLockMode.Locked;
        if (PlayerPrefs.GetInt("Level") < 1)
        {
            buttons[2].gameObject.SetActive(false);
            buttons[3].gameObject.SetActive(false);
        }
        if (PlayerPrefs.GetInt("Level") < 4)
            buttons[4].gameObject.SetActive(false);
    }

    public void Lose () {
        lost = true;
        CameraMovement.Lose ();
    }

    void FixedUpdate()
    {
        if ( lost )
            return;
        if (cooldown != 0)
        {
            --cooldown;
            if(cooldown == 0)
                buttons[4].gameObject.SetActive(true);
        }
        mana += .02f;
        if (mana > maxMana)
            mana = maxMana;
        levelTime += .02f;
        float shots = Mathf.Floor(mana / cost[spells[spell]]);
        if (!Input.GetButton("Rotation") && Input.GetAxis("Rotation1") == 0)
        {
            Vector3 forward = Camera.main.transform.TransformDirection(Vector3.forward);
            forward.y = 0;
            forward = forward.normalized;
            Vector3 right = new Vector3(forward.z, 0, -forward.x);
            Vector3 moveDirection = (Input.GetAxis("Horizontal") * right + Input.GetAxis("Vertical") * forward);
            moveDirection *= speed*1.2f;
            rb3d.velocity = moveDirection;
            //rb3d.velocity = new Vector3(Input.GetAxis("Horizontal") * speed, 0, Input.GetAxis("Vertical") * speed);
        }
        else
            rb3d.velocity = Vector3.zero;
        cursorManaTxt.transform.position = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y + 4, transform.position.z));
        if (charging)
        {
            if (multiplier == 20)
                return;
            multiplier += .14f;
            cursorManaTxt.text = 'x' + multiplier.ToString("F1");
            if (multiplier < 10)
                return;
            if (multiplier > 20)
            {
                multiplier = 20;
                audiosor2.PlayOneShot(fullcharge);
            }
            cursorManaTxt.fontSize = (int)(16 * multiplier*.1f);
            cursorManaTxt.color = new Color(1, (20 - multiplier)*.05f, (20 - multiplier)*.05f, 1);
            //curcursor.transform.localScale *= 1.005f;
            int i = -1; while(++i!=cursorparts.Length)
            {
                cursorparts[i].startSize = cursorsizes[i]*multiplier*.1f;
                cursorparts[i].transform.localScale = cursortransforms[i] * multiplier*.1f;
            }
            foreach (ParticleSystem child in castleparticle.GetComponentsInChildren<ParticleSystem>())
            {
                child.transform.localScale *=   1.01f;
                child.startSize *=              1.01f;
            }
        }
        else {
            cursorManaTxt.text = shots.ToString("F0");
            if (shots == 0)
                cursorManaTxt.color = Color.red;
            else
                cursorManaTxt.color = Color.white;
        }
    }

    void CreateProjectile()
    {

        ((GameObject)Instantiate(attacks[spell], CastleParticlePos, Quaternion.identity)).GetComponent<ProjectileScript>().magnitude = multiplier>1?multiplier:1;
        audiosor2.Stop();
        audiosor.Play();
        cursorManaTxt.color = Color.white;
        if(!ultActive)
        mana -= cost[spells[spell]];
        charging = false;
        Destroy(curcursor);
        Destroy(castleparticle);
        castleparticle = (GameObject)Instantiate(Resources.LoadAll<GameObject>("TowerParticles")[spells[spell]],CastleParticlePos,Quaternion.identity);
        curcursor = Instantiate(Resources.LoadAll<GameObject>("CursorParticles")[spells[spell]]);
        curcursor.transform.position = transform.position;
        curcursor.transform.parent = transform;
        cursorManaTxt.fontSize = 20;
        multiplier = 0;
    }
    // Update is called once per frame
    void Update()
    {
        if ( lost )
            return;
        if (ultActive)
        {
            ultTimer -= Time.deltaTime;
            if (ultTimer <= 0)
            {
                mana = resetMana;
                ultActive = false;
                ultTimer = 20.0f;
            }
        }
        if ( Input.GetButtonDown ( "Fire1" ) && Time.timeScale != 0 ) {
            if ( mana >= cost [ spells [ spell ] ] ) {
                charging = true;
                audiosor2.Play ();
                if ( spells [ spell ] != 6 )
                    shotsTaken += 1.0f;
                cursorparts = curcursor.GetComponentsInChildren<ParticleSystem> ();
                cursortransforms = new Vector3 [ cursorparts.Length ];
                cursorsizes = new float [ cursorparts.Length ];
                int i = -1; while ( ++i != cursorparts.Length ) {
                    cursortransforms [ i ] = cursorparts [ i ].transform.localScale;
                    cursorsizes [ i ] = cursorparts [ i ].startSize;
                }
            } else
                audiosor.PlayOneShot ( error );
        } else if ( Input.GetButtonUp ( "Fire1" ) && Time.timeScale != 0 && charging )
            CreateProjectile ();
        else if ( Input.GetAxisRaw ( "Fire2" ) != 0 && Time.timeScale != 0 ) {
            if ( axisbeingused == false ) {
                if ( mana >= cost [ spells [ spell ] ] ) {
                    charging = true;
                    audiosor2.Play ();
                    if ( spells [ spell ] != 6 )
                        shotsTaken += 1.0f;
                    cursorparts = curcursor.GetComponentsInChildren<ParticleSystem> ();
                    cursortransforms = new Vector3 [ cursorparts.Length ];
                    cursorsizes = new float [ cursorparts.Length ];
                    int i = -1; while ( ++i != cursorparts.Length ) {
                        cursortransforms [ i ] = cursorparts [ i ].transform.localScale;
                        cursorsizes [ i ] = cursorparts [ i ].startSize;
                    }
                } else
                    audiosor.PlayOneShot ( error );
                axisbeingused = true;
            }
        }
        //else if (Input.GetButtonDown("Select1"))
        //{
        //    select = 0;
        //}
        else if ( ( Input.GetButtonDown ( "Select1" ) || Input.GetButtonDown ( "Select2" ) || Input.GetButtonDown ( "Select3" ) || Input.GetButtonDown ( "Select4" ) ) && charging != true && Time.timeScale != 0 ) {
            int select;
            if ( Input.GetButtonDown ( "Select1" ) )
                select = 0;
            else if ( Input.GetButtonDown ( "Select2" ) )
                select = 1;
            else if ( Input.GetButtonDown ( "Select3" ) )
                select = 2;
            else
                select = 3;
            if ( buttons [ select ].gameObject.activeInHierarchy != false ) {

                spell = ( byte )select;
                eventsys.SetSelectedGameObject ( buttons [ select ].gameObject );
                Destroy ( curcursor );
                Destroy ( castleparticle );
                castleparticle = Instantiate ( Resources.LoadAll<GameObject> ( "TowerParticles" ) [ spells [ spell ] ] );
                curcursor = Instantiate ( Resources.LoadAll<GameObject> ( "CursorParticles" ) [ spells [ spell ] ] );
                castleparticle.transform.position = CastleParticlePos;
                curcursor.transform.position = transform.position;
                curcursor.transform.parent = transform;//make it a child of cursor
            }
        } else if ( Input.GetButtonDown ( "Fire3" ) && buttons [ 4 ].gameObject.activeSelf != false ) {
            if ( cooldown == 0 ) {
                buttons [ 4 ].gameObject.SetActive ( false );
                if ( spells [ 4 ] == 8 )
                    new GameObject ( "ult", typeof ( UltmeteorScript ) ).GetComponent<UltmeteorScript> ();
                if ( spells [ 4 ] == 9 ) {
                    audiosor.Play ();
                    foreach ( Health building in buildings ) {
                        if ( building.GetComponent<BuildingScript> () ) {
                            if ( building.tag == "Wagon" ) {
                                if ( building.GetComponent<WagonScript> ().currhp <= 0 )
                                    building.GetComponent<WagonScript> ().timer = 1000;
                                else
                                    building.GetComponent<BuildingScript> ().Heal ();
                            } else
                                building.GetComponent<BuildingScript> ().Heal ();
                        }
                    }
                }

                if ( spells [ 4 ] == 10 ) {
                    resetMana = mana;
                    mana = maxMana;
                    ultActive = true;
                }
                cooldown = 5000;

            } else
                audiosor.PlayOneShot ( error );
        }
        
            

        if (Input.GetAxis("Fire2") == 0 && axisbeingused)
        {
            CreateProjectile();
            axisbeingused = false;
        }
    }

}
