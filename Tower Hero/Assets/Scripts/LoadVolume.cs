using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadVolume : MonoBehaviour
{

    public bool music = false;
    // Use this for initialization
    void Start()
    {
        if (music == true)
        {
            if (PlayerPrefs.HasKey("VolumeMusic"))
                GetComponent<Slider>().value = PlayerPrefs.GetFloat("VolumeMusic");
            else
                GetComponent<Slider>().value = .5f;

        }
        else
        {
            if (PlayerPrefs.HasKey("VolumeSFX"))
                GetComponent<Slider>().value = PlayerPrefs.GetFloat("VolumeSFX");
            else
                GetComponent<Slider>().value = .5f;

        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
