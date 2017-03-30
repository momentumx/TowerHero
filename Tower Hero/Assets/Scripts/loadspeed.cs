using UnityEngine;

public class loadspeed : MonoBehaviour {

	// Use this for initialization
	void Start () {
        if (PlayerPrefs.HasKey("CursorSpeed"))
            GetComponent<UnityEngine.UI.Slider>().value = PlayerPrefs.GetFloat("CursorSpeed");
        else
            GetComponent<UnityEngine.UI.Slider>().value = 55f;
    }
}
