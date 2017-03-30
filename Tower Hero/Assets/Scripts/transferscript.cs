using UnityEngine;

public class transferscript : MonoBehaviour {

    Animator anim;
    bool timer;
    public byte unlocked;
    byte timercount;
    public byte slot;
    UnityEngine.EventSystems.EventSystem eventsys;

    void Start () {
        Time.timeScale = 1;
        anim = GetComponent<Animator>();
        if (Movement.level<unlocked)
            anim.SetBool("Dis", true);
        eventsys = GameObject.Find("EventSystem").GetComponent<UnityEngine.EventSystems.EventSystem>();
        GetComponent<UnityEngine.UI.Button>().image.sprite = Resources.LoadAll<Sprite>("button sprites")[Movement.spells[slot]];
    }

    // Update is called once per frame
    void Update () {
        if(timer)
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
}