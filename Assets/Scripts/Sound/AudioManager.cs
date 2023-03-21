using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Music")]
    [SerializeField] private AudioSource _titleMusic;
    [SerializeField] private AudioSource _gameMusic;


    [Header("Explosions")]
    [SerializeField] private AudioSource _explosionSource;
    [SerializeField] private AudioClip[] _explosionClips;

    private bool _gameMusicPLaying = false;

    // Update is called once per frame
    void Update()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void StartGameMusic()
    {
        if (_titleMusic != null) _titleMusic.Stop();
        if (_gameMusic != null && !_gameMusicPLaying)
        {
            _gameMusic.Play();
            _gameMusicPLaying = true;
        } 
    }

    public void StartFade()
    { 
        StartCoroutine(FadeOutTitleMusic());
    }

    private IEnumerator FadeOutTitleMusic()
    {
        while (_titleMusic.volume > 0)
        {
            _titleMusic.volume -= 0.001f;
            yield return new WaitForSeconds(0.01f);
        }
    }

    public void PlayExplosion()
    {
        int clip = Random.Range(0, _explosionClips.Length);
        _explosionSource.pitch = Random.Range(0.9f, 1.1f);
        _explosionSource.PlayOneShot(_explosionClips[clip]);

    }
}
