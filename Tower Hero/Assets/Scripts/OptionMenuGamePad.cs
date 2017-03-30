using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class OptionMenuGamePad : MonoBehaviour
{
    public GameObject optionsMenu;
    public GameObject[] itemsToSelect;
    public EventSystem eventSystem;
    int selected = 0;
    int count = 0;
    private bool axisisbeingused = false;

    void Start()
    {
        //Time.timeScale = 1;
        optionsMenu = GameObject.Find("OptionMenu");
        eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
    }

    void Update()
    {
        if (optionsMenu.activeInHierarchy == true && count == 0)
        {
            count++;
            eventSystem.SetSelectedGameObject(itemsToSelect[selected]);
        }
        else if (optionsMenu.activeInHierarchy == false)
        {
            count = 0;
        }
        
        if (Input.GetAxisRaw("VerticalControl") != 0)
        {
            if(axisisbeingused == false)
            {
                //float temp = Input.GetAxisRaw("VerticalControl");
                //int temp2 = (int)Input.GetAxisRaw("VerticalControl");
                if(Input.GetAxisRaw("VerticalControl") > 0)
                {
                    selected -= 1;
                }
                if (Input.GetAxisRaw("VerticalControl") < 0)
                {
                    selected += 1;
                }
                //selected -= (int)Input.GetAxisRaw("VerticalControl");
                if(selected > 4)
                {
                    selected = 0;
                }
                if(selected < 0)
                {
                    selected = 4;
                }
            eventSystem.SetSelectedGameObject(itemsToSelect[selected]);
            }
            axisisbeingused = true;
        }
        if (Input.GetAxisRaw("VerticalControl") == 0)
        {
            axisisbeingused = false;
        }
        //else if(Input.GetAxis("VerticalControl") > 0 && selected == 0)
        //{
        //    selected = 4;
        //    eventSystem.SetSelectedGameObject(itemsToSelect[selected]);
        //}
        //else if (Input.GetAxis("VerticalControl") < 0 && selected != 4)
        //{
        //    selected += 1;
        //    eventSystem.SetSelectedGameObject(itemsToSelect[selected]);
        //}
        //else if (Input.GetAxis("VerticalControl") < 0 && selected == 4)
        //{
        //    selected = 0;
        //    eventSystem.SetSelectedGameObject(itemsToSelect[selected]);
        //}
        //if (selected > 4 || selected < 0)
        //    selected = 0;
        if (Input.GetAxisRaw("HorizontalControl2") != 0)
        {
            if (itemsToSelect[selected].GetComponent<Slider>() != null)
            {
                if(selected == 0)
                {
                itemsToSelect[selected].GetComponent<Slider>().value += Input.GetAxisRaw("HorizontalControl2");
                }
                else
                    itemsToSelect[selected].GetComponent<Slider>().value += Input.GetAxisRaw("HorizontalControl2")*.01f;
            }
        }
        if(Input.GetButton("Submit"))
        {
            if(selected == 3 || selected == 4)
            {
                itemsToSelect[selected].GetComponent<Button>().onClick.Invoke();
            }
        }

    }
}
