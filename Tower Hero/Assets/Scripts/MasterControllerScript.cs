using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterControllerScript : MonoBehaviour {

    public enum SAVEKEYINT {
        Level, Minion, Archer, Raider, Tunneler, Flanker, Demolitioner, Shaman, Drummer, Annihilator, Bomber, Infiltrator, Ogre,
        MinionBoss, FlankerBoss, DemolitionerBoss, OgreBoss, DrummerBoss, ShamanBoss, EarthBoss, WaterBoss, AirBoss, FireBoss,
        GoblinDemonLord, HouseHealthUpgrade, WellHealthUpgrade, StartingManaUpgrade, ManaGainUpgrade, TowerHealthUpgrade,
        musicVolume, sfxVolume,
        Total
    }
    public enum SAVEKEYFLT {
        MusicVolume, SfxVolume, Speed,
        Total
    }
    public enum MUSIC_CLIP {
        None,
        Start,
        Gameplay,
        Boss,
        Victory,
        Lose,
        Stop
    };

    static public MUSIC_CLIP currClip = MUSIC_CLIP.Start;
    static public AudioClip[] music_clips, audio_attacks, audio_deaths;
    public static float musicVol, sfxVol;
    public static uint level;
    public static AudioSource musicPlayer, sfxPlayer, transition;
    static public GameObject winmenu;

    void Start () {
        DontDestroyOnLoad ( gameObject );
        musicPlayer = GetComponent<AudioSource> ();
        musicPlayer.ignoreListenerVolume = true;

        if ( PlayerPrefs.HasKey ( SAVEKEYINT.Level.ToString() ) ) {
            //load ints
            level = (uint)PlayerPrefs.GetInt ( SAVEKEYINT.Level.ToString () );
            //load floats
            musicVol = PlayerPrefs.GetFloat ( SAVEKEYFLT.MusicVolume.ToString () );
            sfxVol = PlayerPrefs.GetFloat ( SAVEKEYFLT.SfxVolume.ToString () );
        } else 
            ResetKeys ();
    }

    public void ResetKeys () {
        uint i=uint.MaxValue;
        while ( ++i != ( uint )SAVEKEYINT.Total )
            PlayerPrefs.SetInt ( System.Enum.GetName ( typeof ( SAVEKEYINT ), i ), 0 );
        i = uint.MaxValue;
        while ( ++i != ( uint )SAVEKEYINT.Total )
            PlayerPrefs.SetInt ( System.Enum.GetName ( typeof ( SAVEKEYINT ), i ), 0 );
    }

    static public void SaveKeySetInt ( SAVEKEYINT _saveKey, int _val ) {
        PlayerPrefs.SetInt ( _saveKey.ToString (), _val );
    }
    static public int SaveKeyInc ( SAVEKEYINT _saveKey ) {
        int val = PlayerPrefs.GetInt(_saveKey.ToString())+1;
        PlayerPrefs.SetInt ( _saveKey.ToString (), val );
        return val;
    }
    static public void SaveKeySetFlt ( SAVEKEYFLT _saveKey, float _val ) {
        PlayerPrefs.SetFloat ( _saveKey.ToString (), _val );
    }

    static public void SwitchMusic ( MUSIC_CLIP _newclip ) {
        if ( _newclip == currClip ) {
            return;
        }
        musicPlayer.Stop ();
        currClip = _newclip;
        musicPlayer.PlayOneShot ( music_clips [ (uint)_newclip ] );
    }
}
