using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

public class TabMenu : MonoBehaviour
{

    public RectTransform panelRectTransform_this;
    public RectTransform panelRectTransform_other;
    public RectTransform panelRectTransform_other_2;
    public RectTransform panelRectTransform_other_3;
    public RectTransform panelContent;
    UnityEngine.EventSystems.EventSystem eventsys;
    public GameObject button;
    public Scrollbar scrollbar1;
    public Scrollbar scrollbar2;
    public Button other_tab;
    public Button other_tab_2;
    public Button other_tab_3;
    public Button other_tab_4;
    public Button other_tab_5;
    public bool start_page;
    public bool main_menu;
    void Start()
    {
        eventsys = GameObject.Find("EventSystem").GetComponent<UnityEngine.EventSystems.EventSystem>();
        if(scrollbar1 != null && scrollbar2 != null)
            scrollbar1.value = scrollbar2.value = 1.0f;
        if (start_page == true && main_menu == false)
        {
            GetComponent<Image>().color = GetComponent<Button>().colors.pressedColor;
        }
        if (button != null && start_page == true)
            eventsys.SetSelectedGameObject(button);
    }
    public void Disable()
    {

        StartCoroutine(Activate());
    }
    IEnumerator Activate()
    {
        yield return new WaitForSeconds(.3f);
        panelRectTransform_this.gameObject.SetActive(true);
        if (panelContent != null)
        {
            panelContent.gameObject.SetActive(false);
        }
        if (scrollbar1 != null && scrollbar2 != null)
        {
            scrollbar1.value = scrollbar2.value = 1.0f;
        }
        GetComponent<Image>().color = GetComponent<Button>().colors.pressedColor;
        if (other_tab != null)
            other_tab.GetComponent<Image>().color = other_tab.GetComponent<Button>().colors.normalColor;
        if (other_tab_2 != null)
            other_tab_2.GetComponent<Image>().color = other_tab_2.GetComponent<Button>().colors.normalColor;
        if (other_tab_3 != null)
            other_tab_3.GetComponent<Image>().color = other_tab_3.GetComponent<Button>().colors.normalColor;
        if (other_tab_4 != null)
            other_tab_4.GetComponent<Image>().color = other_tab_4.GetComponent<Button>().colors.normalColor;
        if (other_tab_5 != null)
            other_tab_5.GetComponent<Image>().color = other_tab_5.GetComponent<Button>().colors.normalColor;
        if (panelRectTransform_other != null)
            panelRectTransform_other.gameObject.SetActive(false);
        if (panelRectTransform_other_2 != null)
            panelRectTransform_other_2.gameObject.SetActive(false);
        if (panelRectTransform_other_3 != null)
            panelRectTransform_other_3.gameObject.SetActive(false);
        panelRectTransform_this.SetAsLastSibling();
        eventsys.SetSelectedGameObject(null);
        if (button != null)
        {
            eventsys.SetSelectedGameObject(button);
        }
    }
}
