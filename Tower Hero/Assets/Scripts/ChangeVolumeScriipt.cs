using UnityEngine;
using UnityEngine.UI;

public class ChangeVolumeScriipt : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ChangeVolumeMusic()
    {
        AudioMixerScript.volumemusic = GetComponent<Slider>().value;
        AudioMixerScript.audiomusic.volume = GetComponent<Slider>().value;
        PlayerPrefs.SetFloat("VolumeMusic", AudioMixerScript.volumemusic);
    }
    public void ChangeVolumeSFX()
    {
        AudioMixerScript.volumesfx = GetComponent<Slider>().value;
        Movement.volume = GetComponent<Slider>().value;
        AudioListener.volume = GetComponent<Slider>().value;
        if (Movement.audiosor != null)
            Movement.audiosor.volume = GetComponent<Slider>().value;
        PlayerPrefs.SetFloat("VolumeSFX", AudioMixerScript.volumesfx);
    }
}
