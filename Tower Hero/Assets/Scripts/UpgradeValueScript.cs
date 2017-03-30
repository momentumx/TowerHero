using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeValueScript : MonoBehaviour {
    public MasterControllerScript.SAVEKEYINT saveKey;
    public float cost;
    public UnityEngine.UI.Slider slider;
    // Use this for initialization
    void Start () {
        slider.value = PlayerPrefs.GetInt ( saveKey.ToString () );
        transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = (100*cost).ToString ("F0") + "% (" + (Player.maxMana * cost).ToString("F0") + ')';
        cost *= Player.maxMana;
    }
}
