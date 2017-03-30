using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

public class ButtonScript : MonoBehaviour
{
    [SerializeField]
    string sceneName;
    public bool exit;
    public bool new_game;
    public bool load_level;
    public bool restart;
    public bool restart_mainmenu;
    GameObject myEventSystem;

    void Start()
    {
        myEventSystem = GameObject.Find("EventSystem");
    }

    public void ChangeScene()
    {
        Time.timeScale = 1;
        if (!exit)
        {
            if (new_game)
            {
                Movement.spells = new byte[5] { 0, 1, 2, 3, 8 };
                Movement.level = 0;
                PlayerPrefs.SetInt("Level", 0);
                GameControllerScript.saveLevel = 0;
                //PlayerPrefs.DeleteAll();
            }
            if (load_level)
            {
                Movement.level = (byte)CycleLevel.current_level;
                SceneManager.LoadScene(/*transform.GetChild(0).GetComponent<Text>().text*/sceneName);
            }
            else if (restart)
            {
                Movement.mana = 0;
                if (Movement.level == 0)
                {
                    SceneManager.LoadScene("Level" + (Movement.level + 1));
                }
                else
                    SceneManager.LoadScene("Spells&Enemies");
                //SceneManager.LoadScene("Level" + (Movement.level + 1));
            }
            else
            {
                if (GetComponent<LoadScene>())
                    return;
                if (sceneName == "MainMenu" && restart_mainmenu == true)
                {
                    Movement.mana = 0;
                }
                SceneManager.LoadScene(sceneName);
            }
        }
    }
    public void OnMouseEnter()
    {
        myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);
        myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(this.gameObject);
    }
}
