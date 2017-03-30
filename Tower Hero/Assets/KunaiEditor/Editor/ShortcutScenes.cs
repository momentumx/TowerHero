using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class ShortcutScenes : MonoBehaviour {
   
    static string currentScene = "Level1";
    [MenuItem("Edit/Play-Stop, But From Prelaunch Scene %0")]
    public static void PlayFromPrelaunchScene()
    {
        //PlayFromPrelaunchScenewait();
        if (EditorApplication.isPlaying == true)
        {
            EditorApplication.isPlaying = false;
            return;
        }

        EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        currentScene = EditorSceneManager.GetActiveScene().path;
        EditorSceneManager.OpenScene("Assets/scenes/InitialScene.unity");
        EditorApplication.isPlaying = true;
    }

    [MenuItem("Edit/GoToLastScene %9")]
    public static void GoToSelectedScene()
    {
        EditorSceneManager.OpenScene("Assets/scenes/" + currentScene + ".unity");
    }

}
