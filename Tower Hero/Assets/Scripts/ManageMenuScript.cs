using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ManageMenuScript : MonoBehaviour {
    public enum TRANSITION {
        None,
        Fade
    }
    AsyncOperation aOp;
    float alpha = 1f;
    static public GameObject loadingScreen, winSreen, loseSreen;
    [SerializeField]
    Button [] buttons;

    void LoadScreen (string _sceneName ) {
        if ( loadingScreen ) {
            loadingScreen.SetActive ( true );
            StartCoroutine ( LoadLevelWithProgressBar ( _sceneName ) );
        } else {
            if ( _sceneName == "" )
                SceneManager.LoadSceneAsync ( "Level" + ( MasterControllerScript.level + 1 ) );
            else
                SceneManager.LoadSceneAsync ( _sceneName );
        }
    }
    //fade is not implemented yet
    public void ChangeScene ( string _sceneName, MasterControllerScript.MUSIC_CLIP _music = MasterControllerScript.MUSIC_CLIP.None, TRANSITION _transition = TRANSITION.None) {
        Time.timeScale = 1;
        if ( _music != MasterControllerScript.MUSIC_CLIP.None )
            MasterControllerScript.SwitchMusic ( _music );

        foreach ( Button button in buttons )
            button.interactable = false;

        if ( _transition == TRANSITION.Fade )
            LoadScreen ( _sceneName );
        else if ( _transition == TRANSITION.None )
            LoadScreen ( _sceneName );
    }

    IEnumerator LoadLevelWithProgressBar ( string _sceneName ) {
        yield return new WaitForSeconds ( 1 );

        if ( _sceneName == "" )
            aOp = SceneManager.LoadSceneAsync ( "Level" + ( MasterControllerScript.level + 1 ) );
        else
            aOp = SceneManager.LoadSceneAsync ( _sceneName );

        aOp.allowSceneActivation = false;
        Slider loadingProgress = null;
        if ( loadingScreen )
            loadingProgress = loadingScreen.transform.GetChild(0).GetComponent<Slider>();
        while ( !aOp.isDone ) {
            if ( loadingProgress )
                loadingProgress.value = aOp.progress;

            if ( aOp.progress == 0.9f ) {
                if ( loadingProgress)
                    loadingProgress.value = 1.0f;
                aOp.allowSceneActivation = true;
            }

            yield return null;
        }

    }

    public static void Win () {

    }

}
