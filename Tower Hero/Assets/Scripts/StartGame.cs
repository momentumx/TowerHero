using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class StartGame : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //PlayerPrefs.DeleteAll();
        Cursor.lockState = CursorLockMode.Confined;
        GetComponent<LoadScene>().Load("MainMenu");
        //SceneManager.LoadScene("MainMenu");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
