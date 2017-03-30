using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ResolutionScript : MonoBehaviour
{
#if !UNITY_WEBPLAYER
    public Text text;
    void Start()
    {
        if (Screen.fullScreen)
        {
            text.text = "Windowed";
            GetComponent<Image>().color = GetComponent<Button>().colors.normalColor;
        }
        else
        {
            text.text = "Fullscreen";
            GetComponent<Image>().color = GetComponent<Button>().colors.pressedColor;
        }
    }
    public void SetReso()
    {
        Cursor.lockState = CursorLockMode.None;
        Screen.fullScreen = !Screen.fullScreen;
        Cursor.lockState = CursorLockMode.Confined;
    }
    void FixedUpdate()
    {
        if(Screen.fullScreen)
        {
            text.text = "Windowed";
            GetComponent<Image>().color = GetComponent<Button>().colors.normalColor;
        }
        else
        {
            text.text = "Fullscreen";
            GetComponent<Image>().color = GetComponent<Button>().colors.pressedColor;
        }
    }
#endif
}
