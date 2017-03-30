using UnityEngine;
using UnityEngine.UI;

public class SelectSpellScript : MonoBehaviour {

    public byte spell, unlockedAt, slot;
    bool dis;
    // Use this for initialization
    void Start() {
        if (PlayerPrefs.GetInt("Level") < unlockedAt)
        {
            GetComponent<Animator>().SetBool("Dis", true);
            transform.GetChild(0).GetComponent<Text>().text += unlockedAt;
            dis = true;
        }
	}

    public void Clicked()
    {

        if (!GetComponent<Animator>().GetBool("Dis"))
        {

            Movement.spells[slot] = spell;
            transform.parent.parent.GetComponent<Button>().image.sprite = GetComponent<Button>().image.sprite;
            
        }
    }

    void Update()
    {
        GetComponent<Animator>().SetBool("Dis", dis);
    }
}