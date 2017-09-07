using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public AudioClip[] MainGameMusic;
    public AudioSource PlayMusic;

    // Use this for initialization
    void Start() {
        ChooseGameMusicRandomly();
    }

    // Update is called once per frame
    void Update() {

    }

    public void ChooseGameMusicRandomly()
    {
        AudioClip MyAudioClip = MainGameMusic[Random.Range(0, MainGameMusic.Length)];
        PlayMusic.clip = MyAudioClip;
        PlayMusic.Play();
    }

}
