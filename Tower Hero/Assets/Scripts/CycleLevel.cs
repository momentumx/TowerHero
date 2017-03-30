using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CycleLevel : MonoBehaviour {

    public Sprite[] levels;//= new Sprite[11];
	static public int current_level;
    int max_level;
    public Image level_image;
    public Text level_text;
	// Use this for initialization
	void Start () {
        levels = Resources.LoadAll<Sprite>("Levels");
        max_level = GameControllerScript.saveLevel = PlayerPrefs.GetInt("Level");

        level_text.text = "Level: " + (/*current_level*/max_level + 1);
        level_image.sprite = levels[/*0*/max_level];
        current_level = max_level;
    }

    // Update is called once per frame
    void Update () {
	}

    public void Cycle_Left()
    {
        current_level -= 1;
        if(current_level < 0)
        {
            current_level = /*levels.Length - 1*/max_level;
        }
        level_image.sprite = levels[current_level];
        level_text.text = "Level: " + (current_level + 1);
    }

    public void Cycle_Right()
    {
        current_level += 1;
        if (current_level >/* levels.Length - 1*/max_level)
        {
            current_level = 0;
        }
        level_image.sprite = levels[current_level];
        level_text.text = "Level: " + (current_level + 1);
    }

    static public int Curr_level()
    {
		return current_level;
    }
}
