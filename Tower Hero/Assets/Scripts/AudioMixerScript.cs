using UnityEngine;
using System.Collections;
using UnityEngine.Audio;
using UnityEngine.UI;


public class AudioMixerScript : MonoBehaviour
{
    public static float volumemusic, volumesfx;
    public static AudioSource audiomusic;
    

    public AudioClip[] music_clips;
    // float gameMusic;
    // float gameSFX;

    void Awake()
    {
        //if (GameObject.Find("SoundMFController") == gameObject)
            DontDestroyOnLoad(gameObject);
        //else
        //{
        //    Destroy(gameObject);
        //    return;
        //}
        audiomusic = GetComponent<AudioSource>();
        audiomusic.ignoreListenerVolume = true;
        if (PlayerPrefs.HasKey("VolumeMusic"))
        {
            volumemusic = PlayerPrefs.GetFloat("VolumeMusic");
            audiomusic.volume = volumemusic;
        }
        else
            volumemusic = .5f;
        if (PlayerPrefs.HasKey("VolumeSFX"))
        {
            volumesfx = PlayerPrefs.GetFloat("VolumeSFX");
            AudioListener.volume = volumesfx;
        }
        else
            volumesfx = .5f;
    }

    
}
