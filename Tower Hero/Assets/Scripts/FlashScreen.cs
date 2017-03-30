using UnityEngine;
using UnityEngine.UI;  // add to the top
using System.Collections;
using UnityEngine.EventSystems;

public class FlashScreen : MonoBehaviour
{

    public CanvasGroup myCG;
    static private bool flash = false;
    public int flash_duration;
    //[SerializeField]
    //EventSystem _eventsys;

    void Update()
    {
        if (flash)
        {
            myCG.alpha = myCG.alpha - (Time.deltaTime / flash_duration);
            if (myCG.alpha <= 0)
            {
                myCG.alpha = 0;
                flash = false;
            }
        }
    }

    public void Bang()
    {
        flash = true;
        myCG.alpha = 1;
    }

    static public bool Flashing()
    {
        return flash;
    }

    
}
