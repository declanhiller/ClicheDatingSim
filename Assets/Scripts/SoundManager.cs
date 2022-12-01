using System;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    public static SoundManager INSTANCE;

    public AudioSource introMusicSource;
    public AudioSource loopMusicSource;
    
    [SerializeField] private AudioClip hallwayIntro;
    [SerializeField] private AudioClip hallway;
    [SerializeField] private AudioClip creepyIntro;
    [SerializeField] private AudioClip creepyLoop;
    private Music currentlyPlayingMusic;

    private void Awake() {
        if (INSTANCE == null) {
            PlayMusic(Music.HALLWAY_INTRO);
            INSTANCE = this;
        }else if (INSTANCE != this) {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
    

    public void PlayMusic(Music music) {
        currentlyPlayingMusic = music;
        introMusicSource.clip = music == Music.HALLWAY_INTRO ? hallwayIntro : creepyIntro;
        introMusicSource.loop = false;
        introMusicSource.Play();
        loopMusicSource.clip = music == Music.HALLWAY_INTRO ? hallway : creepyLoop;
        loopMusicSource.loop = true;
        loopMusicSource.PlayScheduled(AudioSettings.dspTime + introMusicSource.clip.length);
    }
    

}