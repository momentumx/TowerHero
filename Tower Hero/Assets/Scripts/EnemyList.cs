using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyList : MonoBehaviour {

    public Image image;
    public Text my_name;
    public Text stats;
    public Text description;
    public Text my_description;
    public GameObject rectTransform;
    public Image mySprite;
    public Shadow shadow;
    public Outline new_outline;
    //public Outline upcoming_outline;
    public byte unlock;
    public byte[] next;
    public Button this_button;
    public Scrollbar scroll1, scroll2;

    Animator anim;
    bool timer;
    public bool dis;
    byte timercount;
    UnityEngine.EventSystems.EventSystem eventsys;
    // Use this for initialization
    void Start () {
		Time.timeScale = 1;
		anim = GetComponent<Animator>();
        if (unlock == PlayerPrefs.GetInt("Level"))
        {
            new_outline.enabled = true;
        }
        //foreach (byte whatever in next)
        //{
        //    if (whatever == Movement.level + 1)
        //        upcoming_outline.enabled = true;
        //}
        if(unlock > PlayerPrefs.GetInt("Level"))
        {
            dis = true;
            this_button.interactable = false;
        }
		else
		{
			dis = false;
			this_button.interactable = true;
		}
        if (dis)
            anim.SetBool("Dis", true);
		else
			anim.SetBool("Dis", false);
        eventsys = GameObject.Find("EventSystem").GetComponent<UnityEngine.EventSystems.EventSystem>();
    }
	
	// Update is called once per frame
	void Update () {
        if (timer)
            ++timercount;
        if (timercount == 2)
        {
            //timer = false;
            anim.SetBool("Timer", false);
        }
        if (eventsys.currentSelectedGameObject == null || !eventsys.currentSelectedGameObject.activeSelf)
        {
            eventsys.SetSelectedGameObject(eventsys.firstSelectedGameObject);

        }
        if ((Input.GetButtonDown("Submit") || Input.GetButtonDown("SubmitAlt")) && anim.GetCurrentAnimatorStateInfo(0).IsName("Pressed") || Input.GetButtonDown("HorizontalControl"))
        {
            timer = true;
            anim.SetBool("Timer", true);
            timercount = 0;
        }
    }

    public void Selected()
    {
        shadow.enabled = true;
        if (rectTransform.activeInHierarchy == false)
        {
            rectTransform.SetActive(true);
        }
        image.sprite = mySprite.sprite;
        image.color = mySprite.color;
        my_name.text = gameObject.transform.name.ToString();
        if (stats != null)
        {
            stats.text = "";
            if (PlayerPrefs.HasKey(gameObject.transform.name.ToString()))
                stats.text = "Killed: " + PlayerPrefs.GetInt(gameObject.transform.name.ToString());
            else
                stats.text = "";
        }
        description.text = my_description.text;
    }
    public void Deselect()
    {
        shadow.enabled = !shadow.enabled;
    }
}
