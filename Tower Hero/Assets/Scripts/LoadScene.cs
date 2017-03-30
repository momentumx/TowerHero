using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour {

	AsyncOperation aOp;
	public GameObject loadingScreen;
	public Button[] buttons;
	public Animator loadingAnim;
	public Slider loadingProgress;
	public float loadTime;
	public bool new_game;
	string sceneName;


	public void Load(string _scenename)
	{
		sceneName = _scenename;

		Time.timeScale = 1;
        if (buttons != null)
        {
            foreach (Button button in buttons)
            {
                button.interactable = false;
            }
        }

		loadingScreen.SetActive(true);

		StartCoroutine(LoadLevelWithProgressBar());
	}

	IEnumerator LoadLevelWithProgressBar ()
	{
		yield return new WaitForSeconds (1);
		if(new_game == true)
		{
			Movement.level = 0;
            PlayerPrefs.SetInt("Level", 0);
            GameControllerScript.saveLevel = 0;
            PlayerPrefs.SetInt("Minion", 0);
            PlayerPrefs.SetInt("Archer", 0);
            PlayerPrefs.SetInt("Raider", 0);
            PlayerPrefs.SetInt("Tunneler", 0);
            PlayerPrefs.SetInt("Flanker", 0);
            PlayerPrefs.SetInt("Demolitioner", 0);
            PlayerPrefs.SetInt("Shaman", 0);
            PlayerPrefs.SetInt("Drummer", 0);
            PlayerPrefs.SetInt("Annihilator", 0);
            PlayerPrefs.SetInt("Bomber", 0);
            PlayerPrefs.SetInt("Infiltrator", 0);
            PlayerPrefs.SetInt("Ogre", 0);
            PlayerPrefs.SetInt("MinionBoss", 0);
            PlayerPrefs.SetInt("FlankerBoss", 0);
            PlayerPrefs.SetInt("DemolitionerBoss", 0);
            PlayerPrefs.SetInt("OgreBoss", 0);
            PlayerPrefs.SetInt("DrummerBoss", 0);
            PlayerPrefs.SetInt("ShamanBoss", 0);
            PlayerPrefs.SetInt("EarthBoss", 0);
            PlayerPrefs.SetInt("WaterBoss", 0);
            PlayerPrefs.SetInt("AirBoss", 0);
            PlayerPrefs.SetInt("FireBoss", 0);
            PlayerPrefs.SetInt("GoblinDemonLord", 0);
            PlayerPrefs.SetInt("HouseHealthUpgrade", 0);
			PlayerPrefs.SetInt("WellHealthUpgrade", 0);
			PlayerPrefs.SetInt("StartingManaUpgrade", 0);			
			PlayerPrefs.SetInt("ManaGainUpgrade", 0);			
			PlayerPrefs.SetInt("TowerHealthUpgrade", 0);			
		}
		if (sceneName == "")
		{
			aOp = SceneManager.LoadSceneAsync ("Level" + (Movement.level + 1));
		}

		else
		{
			aOp = SceneManager.LoadSceneAsync (sceneName);
		}

    	aOp.allowSceneActivation = false;

    	while(!aOp.isDone)
    	{
    		loadingProgress.value = aOp.progress;

    		if(aOp.progress == 0.9f)
    		{
    			loadingProgress.value = 1.0f;
    			aOp.allowSceneActivation = true;
    		}
            
			yield return null;
    	}

    }

}
