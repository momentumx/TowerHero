using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DummyLoad : MonoBehaviour
{
    Text lvl;

    void Start()
    {
        GameControllerScript.saveLevel = PlayerPrefs.GetInt("Level");
        lvl = this.GetComponent<Text>();
        UpdateText();
    }

    void UpdateText()
    {
        lvl.text = "Level to load: " + GameControllerScript.saveLevel.ToString();
    }
}
