using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public AudioClip digLoop;
    public AudioClip WormDig;

    public AudioSource source;
    private static AudioManager _instance;
    public static AudioManager Instance => _instance;



    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else if (_instance != this)
            Destroy(this.gameObject);
    }
    private void Start()
    {
        source= GetComponent<AudioSource>();
    }

    public void PlayAudio(AudioClip clip,bool isLoop,bool is2D)
    {
        source.clip= clip;
        source.loop= isLoop;

        source.spatialBlend = is2D ? 0 : 1;

        source.Play();


    }
    public void StopAudio(AudioClip clip)
    {
        source.clip = clip;
        source.Stop();
    }
}
