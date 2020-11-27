using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    private static SFXManager instance = null;
    public static SFXManager Instance { get { return instance; } }

    //Är nog bättre om det görs om till en array om vi ska spara ljuden på det här sättet.
    [SerializeField] private AudioClip[] winSounds;
    [SerializeField] private AudioClip[] placeSounds;

    AudioSource audioSource;

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this.gameObject);
        else
            instance = this;

        audioSource = gameObject.GetComponent<AudioSource>();
    }

    public void PlayWin()
    {
        AudioClip sound = winSounds[UnityEngine.Random.Range(0, winSounds.Length)];
        audioSource.PlayOneShot(sound);
    }

    public void PlayPlace()
    {
        AudioClip sound = placeSounds[UnityEngine.Random.Range(0, winSounds.Length)];
        audioSource.PlayOneShot(sound);
    }
}
