using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Music")]
    [SerializeField] private AudioSource _titleMusic;
    [SerializeField] private AudioSource _gameMusic;
    [SerializeField] private bool _gameMusicPlaying = false;

    [Header("Explosions")]
    [SerializeField] private AudioSource _explosionSource;
    [SerializeField] private AudioClip[] _explosionClips;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

    }

        // Update is called once per frame
        void Update()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void StartGameMusic()
    {
        if (_titleMusic != null) _titleMusic.Stop();
        if (_gameMusic != null && !_gameMusicPlaying)
        {
            _gameMusic.volume = 0.5f;
            _gameMusic.Play();
            _gameMusicPlaying = true;
        } 
    }

    public void StartFadeOutMenuMusic()
    { 
        StartCoroutine(FadeOutTitleMusic());
    }

    public void StartFadeOutGameMusic()
    {
        StartCoroutine(FadeOutGameMusic());
    }

    private IEnumerator FadeOutTitleMusic()
    {
        if (_titleMusic.volume == 0)
        {
            _titleMusic.Stop();
            _titleMusic.volume = 0.5f;
        }

        while (_titleMusic.volume > 0)
        {
            _titleMusic.volume -= 0.001f;
            yield return new WaitForSeconds(0.01f);
        }
    }

    private IEnumerator FadeOutGameMusic()
    {
        _gameMusicPlaying = false;

        if (_gameMusic.volume == 0)
        {
            _gameMusic.Stop();
            _gameMusic.volume = 0.5f;
        }

        while (_gameMusic.volume > 0)
        {
            _gameMusic.volume -= 0.01f;
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
