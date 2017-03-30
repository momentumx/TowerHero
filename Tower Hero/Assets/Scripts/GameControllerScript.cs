using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class GameControllerScript : MonoBehaviour
{
    [SerializeField]
    GameObject[] hazards;
    [SerializeField]
    GameObject bosses;
    [SerializeField]
    GameObject winmenu;
    [SerializeField]
    public AudioClip nextWave, spin;
    public float startWait, waveWait;
    float speed = 50;
    static public int saveLevel = 0;
    bool startWave,spinning;
    public UnityEngine.UI.Slider waveCount, waveTimer;
    public UnityEngine.UI.Text completionTimeText, manaLeftText, enemiesKilledText, accuracyText;
    UnityEngine.UI.Image leftIndicator, rightIndicator;
    static public System.Collections.Generic.List<EnemyAI> converted;

    //static public byte[][] wavesize =
    //{
    //    new byte[] {4 , 6 , 8 },                              //the numbers in the {} is the individual wave size (defaulted to 10)
    //    new byte[] {5 , 7 , 14},
    //    new byte[] {9 , 8 , 16},
    //    new byte[] {10, 10, 18},
    //    new byte[] {11, 12, 20},
    //    new byte[] {12, 14, 22},
    //    new byte[] {13, 16, 24},
    //    new byte[] {14, 18, 26},
    //    new byte[] {15, 20, 28},
    //    new byte[] {16, 22, 30},
    //};
    static public byte[][] wavesize =
    {
        new byte[] {3, 4, 5},                              //the numbers in the {} is the individual wave size (defaulted to 10)
        new byte[] {4,5, 6},
        new byte[] {5, 6, 7},
        new byte[] {6, 7, 8},
        new byte[] {7, 8, 9},
        new byte[] {8, 9, 10},
        new byte[] {9, 10, 11},
        new byte[] {10, 11, 12},
        new byte[] {11, 12, 13},
        new byte[] {12, 13, 14},
    };


    byte[] spawnRatio = { //this takes up less size and i can use it becasue i dont need .leangth()
//    mini arch  raid tunnler  demo infil  ogre flank  bomb shaman  drum ann
        10,9,       5,8,          8,2,        4,7,        6,3,         1,4};

    // Use this for initialization
    void Start()
    {
        converted = new System.Collections.Generic.List<EnemyAI>();
        waveWait = 1 / waveWait;
        leftIndicator = GameObject.Find("Left").GetComponent<UnityEngine.UI.Image>();
        rightIndicator = GameObject.Find("Right").GetComponent<UnityEngine.UI.Image>();
        StartCoroutine(SpawnWaves());
        saveLevel = PlayerPrefs.GetInt("Level");
    }

    void FixedUpdate()
    {
        if (startWave)
        {
            waveTimer.value += Time.fixedDeltaTime * waveWait;
            if (waveTimer.value >= 1)
                waveTimer.value = 0;
        }
        if (spinning)
        {
            waveCount.transform.parent.Rotate(speed, 0,0);
            speed-=.2f;
            if (speed <= 0)
            {
                waveCount.transform.parent.rotation = new Quaternion(0,0,0,0);
                spinning = false;
            }
        }
    }

    IEnumerator SpawnWaves()
    {
        short arrsize = 0;
        sbyte j = -1; while (++j != /*Movement.level + */hazards.Length)//sets the size of the array
            arrsize += spawnRatio[j];
        GameObject[] enemies = new GameObject[arrsize];

        arrsize = 0;
        j = -1; while (++j != /*Movement.level + */hazards.Length)
        {
            sbyte k = -1; while (++k != spawnRatio[j])
            {//fills the array
                enemies[arrsize] = hazards[j];
                ++arrsize;
            }
        }
        yield return new WaitForSeconds(/*startWait*/3);
        j = -1; while (++j != wavesize[Movement.level].Length)//the number of waves to produce
        {
            Movement.audiosor.PlayOneShot(nextWave);
            ++waveCount.value;
            startWave = false;

            int spawn_area = Random.Range(0, 4);
            sbyte i = -1; while (++i != wavesize[Movement.level][j])//the number of enemies in a wave
            {
                GameObject hazard = enemies[Random.Range(0, arrsize)];
                float angle = Mathf.PI *.25f + Random.Range(0, Mathf.PI *.5f);
                Vector3 spawnPosition = new Vector3(                            //sets the object in a certain angle around the position of this object
                    transform.position.x + 168 * Mathf.Cos((Mathf.PI * .5f) * spawn_area - angle),
                    0,
                    transform.position.z + 168 * Mathf.Sin((Mathf.PI * .5f) * spawn_area - angle)
                    );
                Instantiate(hazard, spawnPosition, Quaternion.identity);
                if (i == 0)
                {
                    Vector3 camdir = Camera.main.WorldToScreenPoint(spawnPosition);
                    if (camdir.y < 0 || camdir.y > Screen.height || camdir.x < 0 || camdir.x > Screen.width)
                        SelectFlash(spawnPosition);
                }

                yield return new WaitForSeconds(Random.Range(0, 1f));//float is inclusive I made it random for a better effect
            }
            startWave = true;
            yield return new WaitForSeconds(1/waveWait);
            
        }
        startWave = false;
        waveTimer.value = 1;
        waveTimer.GetComponent<Animator>().enabled = true;
        while (true)
        {
            bool dead = true;
            foreach (Health convertedEnemy in Movement.buildings)
                if (convertedEnemy.tag == "Enemy")
                {
                    dead = false;
                    break;
                }
            if (dead)
                break;
            yield return new WaitForSeconds(1);

        }
        foreach (EnemyAI convertedEnemy in converted)
            if(convertedEnemy != null)
                convertedEnemy.TakeDamage(convertedEnemy.HP+convertedEnemy.armor, Color.magenta);
        //Movement.audiosor.PlayOneShot(boss);
        GetComponent<ChangeMusic>().SwitchMusic((int)ChangeMusic.Music.Boss);
        Movement.audiosor.PlayOneShot(spin);
        spinning = true;
        int o = -1; while (++o!=4)
            waveCount.transform.parent.GetChild(o).gameObject.SetActive(false);
        waveCount.transform.parent.GetChild(4).gameObject.SetActive(true);
        yield return new WaitForSeconds(3);
        int spawn_area2 = Random.Range(0, 4);
        float angle2 = Mathf.PI / 4 + Random.Range(0, Mathf.PI / 2);
        Vector3 spawnPosition2 = new Vector3(                            //sets the object in a certain angle around the position of this object
            transform.position.x + 168 * Mathf.Cos((Mathf.PI * .5f) * spawn_area2 - angle2),
            0,
            transform.position.z + 168 * Mathf.Sin((Mathf.PI * .5f) * spawn_area2 - angle2)
            );
        GameObject temp = (GameObject)Instantiate(bosses, spawnPosition2, Quaternion.identity);
        SelectFlash(spawnPosition2);
        while (temp != null)
            yield return new WaitForSeconds(1);

        GetComponent<ChangeMusic>().SwitchMusic((int)ChangeMusic.Music.Victory);
        Movement.eventsys.enabled = true;
        Camera.main.GetComponent<CameraShake>().Stop_Shake();
        winmenu.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        //stats
        completionTimeText.text = "Completion Time: " + string.Format("{0}:{1:00}", (int)Movement.levelTime / 60, Movement.levelTime % 60);
        Movement.manaRemaining = Movement.mana;// *10;
        Movement.manaPercent = /*100-*/((Movement.manaRemaining / Movement.maxMana)*100);
        manaLeftText.text = "Remaining Mana: " + (int)Movement.manaPercent + "%";
        enemiesKilledText.text = "Enemies Killed: " + Movement.enemiesKilled;
        Movement.accuracy = (Movement.shotsHit / Movement.shotsTaken)*100;
        accuracyText.text = "Accuracy: " + (int)Movement.accuracy + "%";
        //
        Time.timeScale = 0;
        Movement.charging = false;
        Movement.audiosor2.Stop();
        ++Movement.level;
        if (PlayerPrefs.GetInt("Level") < Movement.level)
            {
                saveLevel = Movement.level;
                SaveText();
            }
        Destroy(gameObject);
    }

    public void ContinueFlash(Vector3 dir)
    {
        foreach (Health enemy in Movement.buildings)
            if (enemy.tag == "Enemy")
            {
                Vector3 camdir = Camera.main.WorldToScreenPoint(enemy.transform.position);
                if (camdir.y < 0 || camdir.x < 0 || camdir.x > Screen.width)
                    continue;
                else
                    return;
            }

        SelectFlash(dir);
    }

    void SelectFlash(Vector3 dir)
    {
        if (Vector3.Dot(Camera.main.transform.right, (dir - transform.position)) > 0)
            StartCoroutine(IndicatorFlash(rightIndicator, dir));
        else
            StartCoroutine(IndicatorFlash(leftIndicator, dir));
    }

    IEnumerator IndicatorFlash(UnityEngine.UI.Image _side, Vector3 _dir)
    {
        _side.enabled = true;
        yield return new WaitForSeconds(.4f);
        _side.enabled = false;
        yield return new WaitForSeconds(.8f);
        ContinueFlash(_dir);
    }



    void SaveText()
    {
        PlayerPrefs.SetInt("Level", saveLevel);
    }
    // completionTimeText, manaLeftText, enemiesKilledText, accuracyText;
}