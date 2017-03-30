using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadCheck : MonoBehaviour {

	// Use this for initialization
	void Start () {
	if(!(PlayerPrefs.GetInt("Level") > 0))
        {
            gameObject.GetComponent<Button>().interactable = false;
        }
	}
}
