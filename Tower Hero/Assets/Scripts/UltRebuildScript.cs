using UnityEngine;
using System.Collections;

public class UltRebuildScript : MonoBehaviour {

    AudioSource audiosor;

    void Start()
    {
        audiosor = GameObject.Find("cursor").GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        audiosor.Play();
       
    }
}
