using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class WaveControlScript : MonoBehaviour {
    enum TRACKERTYPE {
        LeftAndRight,
        Arrow,
        AreaFlash
    }
    [SerializeField]
    TRACKERTYPE trackingType;

    static public List<EnemyAI> converted;
    static public List<EnemyAI> enemies;

    //the different types of things to be spawned
    [SerializeField]
    GameObject[] hazards, bosses, indicators;

    //sliders needed for wave counter and timer
    [SerializeField]
    UnityEngine.UI.Slider waveCountSlider, waveTimerSlider;

    [SerializeField]
    AudioClip nextWave, spin;

    //the chance of having a certain enemy spawn more, size of each wave and the number of waves
    [SerializeField]
    uint[] spawnRatios, waveSizes;

    //the number of pie slices
    [SerializeField]
    uint spawnPositions;

    [SerializeField]
    float startWait, waveWait, minSpawnWait, maxSpawnWait, minRadius, maxRadius;

    //set to true if they can land inside the raduis
    [SerializeField]
    bool randomAngle, oneBossAtATime;
    static public bool waitingWave;

    //game specific variables

    void Start () {
        waitingWave = false;
        waveCountSlider.maxValue = hazards.Length;
        Vector2 counterWidth = ((RectTransform)waveCountSlider.transform).sizeDelta;
        counterWidth.x *= hazards.Length;
        ( ( RectTransform )waveCountSlider.transform ).sizeDelta = counterWidth;
        
        waveTimerSlider.GetComponent<Animator> ().speed = 1/ waveWait;
        converted = new List<EnemyAI> ();
        enemies = new List<EnemyAI> ();
        StartCoroutine ( SpawnWaves () );
    }

    //Game Specific function (only delete body)
    void BossWaiting () {
        waveTimerSlider.value = 1;
        waveTimerSlider.GetComponent<Animator> ().enabled = true;
    }

    //Game specific function (only delete body)
    void BossIncoming () {
        MasterControllerScript.sfxPlayer.PlayOneShot ( spin );
        waveTimerSlider.transform.parent.GetComponent<Animator> ().SetTrigger ( "Spin" );
        waveTimerSlider.enabled = false;
        waveCountSlider.enabled = false;
    }

    IEnumerator SpawnWaves () {
        float angleWidth = 2 * Mathf.PI / spawnPositions;
        //fill list with a balanced array
        uint j,i;
        float spawn_area;
        Vector3 spawnPosition = transform.position, camdir;
        {//scope to let all hazards fall off
            List<GameObject> allHazards = new List<GameObject>();//holds a blanced array of enemies
            {
                uint k = (uint)hazards.Length; while ( --k != uint.MaxValue ) {
                    j = spawnRatios [ k ]; while ( --j != uint.MaxValue )
                        allHazards.Add ( hazards [ k ] );
                }
            }
            //setting timescale =0 also stops this
            yield return new WaitForSeconds ( startWait );
            j = (uint)waveSizes.Length; while ( --j != uint.MaxValue )//for each wave
            {
                MasterControllerScript.sfxPlayer.PlayOneShot ( nextWave );
                ++waveCountSlider.value;

                //set all minions to spawn at the same pie slice
                spawn_area = Random.Range ( 0, (int)spawnPositions )*angleWidth;
                
                i = waveSizes [ j ]; while ( --i != uint.MaxValue )//for each hazard in the wave
                {
                    if(randomAngle)
                        spawn_area += Random.Range(0,angleWidth);
                    maxRadius = Random.Range ( minRadius, maxRadius );

                    spawnPosition.x = transform.position.x + maxRadius * Mathf.Cos( spawn_area );
                    spawnPosition.z = transform.position.z + maxRadius * Mathf.Sin( spawn_area );
                    Transform spawn =  Instantiate (allHazards[Random.Range(0, allHazards.Count)]).transform;
                    spawn.position = spawnPosition;
                    enemies.Add ( spawn.GetComponent<EnemyAI>() );
                    
                    camdir = Camera.main.WorldToScreenPoint(spawnPosition);
                    if ( camdir.y < 0 || camdir.x < 0 || camdir.x > Screen.width )
                        SelectFlash ( spawn );//always does at least 1 arrow point

                    yield return new WaitForSeconds ( Random.Range (minSpawnWait, maxSpawnWait ) );
                }
                waveTimerSlider.GetComponent<Animator> ().Play ("Timer");
                yield return new WaitForSeconds ( waveWait );

            }
        }
        //Begin boss sequence
        BossWaiting ();
        {
            bool alive = true;
            while ( alive ) {
                if ( enemies.Count == 0 )
                    alive = false;
                yield return new WaitForSeconds ( 1 );
            }
        }
        foreach ( EnemyAI convertedEnemy in converted )
            convertedEnemy.TakeDamage ( ( int )( convertedEnemy.HP + convertedEnemy.armor ), Color.magenta );
        MasterControllerScript.SwitchMusic ( MasterControllerScript.MUSIC_CLIP.Boss );
        BossIncoming ();
        yield return new WaitForSeconds ( 3 );
        
        //spawn bosses
        i = uint.MaxValue; while ( ++i != ( uint )bosses.Length )//for each hazard in the wave
            {
            MasterControllerScript.sfxPlayer.PlayOneShot ( nextWave );
            spawn_area = Random.Range ( 0, ( int )spawnPositions ) * angleWidth;
            spawnPosition.x = transform.position.x + maxRadius * Mathf.Cos ( spawn_area );
            spawnPosition.z = transform.position.z + maxRadius * Mathf.Sin ( spawn_area );
            Transform spawn =  Instantiate (bosses[i]).transform;
            spawn.position = spawnPosition;
            enemies.Add ( spawn.GetComponent<EnemyAI> () );
            //point at spawnposition if the camera is NOT looking at it.
            camdir = Camera.main.WorldToScreenPoint ( spawnPosition );
            if ( camdir.y < 0 || camdir.x < 0 || camdir.x > Screen.width )
                SelectFlash ( spawn );
            yield return new WaitForSeconds ( Random.Range ( minSpawnWait, maxSpawnWait ) );
            if ( oneBossAtATime ) {
                while ( enemies.Count != 0 )
                    yield return new WaitForSeconds ( 1 );
            }
        }

        MasterControllerScript.SwitchMusic ( MasterControllerScript.MUSIC_CLIP.Victory );
        CameraMovement.Win ();
        ManageMenuScript.Win ();
        //this is turned off for windows/mac so we need to turn it back on
        Player.eventsys.enabled = true;
        ManageMenuScript.winSreen.SetActive ( true );
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0;
        if ( PlayerPrefs.GetInt ( "Level" ) == MasterControllerScript.level )
            MasterControllerScript.level = ( uint )MasterControllerScript.SaveKeyInc ( MasterControllerScript.SAVEKEYINT.Level );
        Destroy ( gameObject );
    }

    public void ContinueFlash ( Transform _dir ) {
        foreach ( EnemyAI enemy in enemies ) {
            Vector3 camdir = Camera.main.WorldToScreenPoint(enemy.transform.position);
            if ( camdir.y < 0 || camdir.x < 0 || camdir.x > Screen.width )
                continue;
            else
                return;
        }

        SelectFlash ( _dir );
    }

    void SelectFlash ( Transform _dir ) {
        GameObject indicator = null;
        switch ( trackingType ) {
            case TRACKERTYPE.LeftAndRight:
                if ( Vector3.Dot ( Camera.main.transform.right, ( _dir.position - Camera.main.transform.position ) ) > 0 )
                    indicator = indicators[1];//right
                else
                    indicator = indicators[0];//left
                break;
            case TRACKERTYPE.Arrow:
                indicator = indicators[0];
                indicator.transform.LookAt ( _dir );
                break;
            case TRACKERTYPE.AreaFlash:
                //to be finished
                //for top down view
                break;
        }
        StartCoroutine ( IndicatorFlash ( indicator, _dir ) );
    }

    IEnumerator IndicatorFlash ( GameObject _indicator, Transform _dir ) {
        _indicator.SetActive(true);
        yield return new WaitForSeconds ( .4f );
        _indicator.SetActive(false);
        yield return new WaitForSeconds ( .8f );
        ContinueFlash ( _dir );
    }
}