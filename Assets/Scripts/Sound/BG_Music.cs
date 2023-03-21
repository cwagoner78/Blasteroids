using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BG_Music : MonoBehaviour
{

    private AudioSource _source;
    public bool musicPlaying;

    private void Start()
    {
        _source = GetComponent<AudioSource>();
    }

    public void PlayMusic()
    {
        _source.Play();
        musicPlaying = true;
    }
}
