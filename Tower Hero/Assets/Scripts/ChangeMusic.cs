using UnityEngine;
using System.Collections;

public class ChangeMusic : MonoBehaviour {

    public enum Music
    {
        Start = 0,
        Gameplay,
        Boss,
        Victory,
        Lose
    };

    public void SwitchMusic(int _newclip)
    {
        if(AudioMixerScript.audiomusic.clip == GameObject.Find("SoundMFController").GetComponent<AudioMixerScript>().music_clips[_newclip])
        {
            return;
        }
        AudioMixerScript.audiomusic.clip = GameObject.Find("SoundMFController").GetComponent<AudioMixerScript>().music_clips[_newclip];
        AudioMixerScript.audiomusic.Play();
    }
}
