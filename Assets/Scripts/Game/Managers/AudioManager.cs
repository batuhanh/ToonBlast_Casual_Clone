using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource balloonPopAudio;
    [SerializeField] private AudioSource duckExplodeAudio;
    [SerializeField] private AudioSource cubeExplosionAudio;
    [SerializeField] private AudioSource cubeCollectAudio;

    public static AudioManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    public void PlayBalloonPopAudio()
    {
        balloonPopAudio.Play();
    }
    public void PlayDuckExplodeAudio()
    {
        duckExplodeAudio.Play();
    }
    public void PlayCubeExplosionAudio()
    {
        cubeExplosionAudio.Play();
    }
    public void PlayCubeCollectAudio()
    {
        cubeCollectAudio.Play();
    }


}
