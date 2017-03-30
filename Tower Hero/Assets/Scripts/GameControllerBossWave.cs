using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameControllerBossWave : MonoBehaviour
{
    [SerializeField]
    GameObject[] bosses = new GameObject[11];

    [SerializeField]
    List<GameObject>[] arrayofBosses = new List<GameObject>[5];

    [SerializeField]
    GameObject winmenu;
    [SerializeField]
    public AudioClip spin;
    public float startWait = 2, waveWait = 15, speed = 50;
    public Transform waveCount;
    static public byte mywave;
    static public int saveLevel = 0;
    bool spinning;
    public Text completionTimeText, manaLeftText, enemiesKilledText, accuracyText;
    UnityEngine.UI.Image leftIndicator, rightIndicator;



    //Use this for initialization

    void Start()
    {
        arrayofBosses[0] = new List<GameObject>();
        arrayofBosses[1] = new List<GameObject>();
        arrayofBosses[2] = new List<GameObject>();
        arrayofBosses[3] = new List<GameObject>();
        arrayofBosses[4] = new List<GameObject>();
        arrayofBosses[0].Add(bosses[0]);
        arrayofBosses[1].Add(bosses[1]);
        arrayofBosses[1].Add(bosses[2]);
        arrayofBosses[2].Add(bosses[3]);
        arrayofBosses[2].Add(bosses[4]);
        arrayofBosses[2].Add(bosses[5]);
        arrayofBosses[3].Add(bosses[6]);
        arrayofBosses[3].Add(bosses[7]);
        arrayofBosses[3].Add(bosses[8]);
        arrayofBosses[3].Add(bosses[9]);
        arrayofBosses[4].Add(bosses[10]);

        leftIndicator = GameObject.Find("Left").GetComponent<UnityEngine.UI.Image>();
        rightIndicator = GameObject.Find("Right").GetComponent<UnityEngine.UI.Image>();
        StartCoroutine(SpawnWaves());
        saveLevel = PlayerPrefs.GetInt("Level");
    }

    void FixedUpdate()
    {

        if (spinning)
        {
            waveCount.transform.parent.Rotate(transform.right, speed);
            speed -= .2f;
            if (speed <= 0)
            {
                waveCount.transform.parent.rotation = new Quaternion(0, 0, 0, 0);
                spinning = false;
            }
        }
    }

    IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(startWait);
        GetComponent<ChangeMusic>().SwitchMusic((int)ChangeMusic.Music.Boss);
        int j = -1; while (++j != arrayofBosses.Length)//the number of waves to produce
        {
            mywave += (byte)arrayofBosses[j].Count;
            Movement.audiosor.PlayOneShot(spin);
            spinning = true;
            int spawn_area = Random.Range(0, 4);
            Movement.audiosor.PlayOneShot(spin);
            sbyte i = -1; while (++i != arrayofBosses[j].Count)//the number of enemies in a wave
            {
                float angle = Mathf.PI / 4 + Random.Range(0, Mathf.PI / 2);
                Vector3 spawnPosition = new Vector3(                            //sets the object in a certain angle around the position of this object
                    transform.position.x + 168 * Mathf.Cos((Mathf.PI * .5f) * spawn_area - angle),
                    0,
                    transform.position.z + 168 * Mathf.Sin((Mathf.PI * .5f) * spawn_area - angle)
                    );

                Instantiate(arrayofBosses[j][i], spawnPosition, Quaternion.identity);
                Vector3 camdir = Camera.main.WorldToScreenPoint(spawnPosition);
                if (camdir.y < 0 || camdir.y > Screen.height || camdir.x < 0 || camdir.x > Screen.width)
                    SelectFlash(spawnPosition);
                yield return new WaitForSeconds(Random.Range(0, 1f));//float is inclusive I made it random for a better effect
            }
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

        }

        yield return new WaitForSeconds(1);
        Movement.eventsys.enabled = true;
        GetComponent<ChangeMusic>().SwitchMusic((int)ChangeMusic.Music.Victory);
        Camera.main.GetComponent<CameraShake>().Stop_Shake();
        winmenu.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        //stats
        completionTimeText.text = "Completion Time: " + string.Format("{0}:{1:00}", (int)Movement.levelTime / 60, Movement.levelTime % 60);
        Movement.manaRemaining = Movement.mana;// *10;
        Movement.manaPercent = 100 - ((Movement.manaRemaining / Movement.maxMana) * 100);
        manaLeftText.text = "Remaining Mana: " + (int)Movement.manaPercent + "%";
        enemiesKilledText.text = "Enemies Killed: " + Movement.enemiesKilled;
        Movement.accuracy = (Movement.shotsHit / Movement.shotsTaken) * 100;
        accuracyText.text = "Accuracy: " + (int)Movement.accuracy + "%";

        Time.timeScale = 0;
        saveLevel = Movement.level;
        SaveText();
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
    //completionTimeText, manaLeftText, enemiesKilledText, accuracyText;
}