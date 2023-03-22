using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Music")]
    [SerializeField] private AudioSource _titleMusic;
    [SerializeField] private AudioSource _gameMusic;
    [SerializeField] private float _fadeTimerInterval = 0.001f;
    private bool titleMusicPlaying = false;
    private bool gameMusicPlaying = false;

    [Header("Explosions")]
    [SerializeField] private AudioSource _explosionSource;
    [SerializeField] private AudioClip[] _explosionClips;

    [Header("UI")]
    [SerializeField] private AudioSource _pause;
    [SerializeField] private AudioSource _unPause;

    public static AudioManager instance;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        //if (instance == null) instance = this;
        //else Destroy(gameObject);
    }

    public void StartTitleMusic()
    {
        if (!titleMusicPlaying)
        {
            _titleMusic.Play();
            _gameMusic.Stop();
            _titleMusic.volume = 0.5f;
            gameMusicPlaying = false;
            titleMusicPlaying = true;
        }
    }

    public void StartGameMusic()
    {
        if (!gameMusicPlaying)
        {
            _gameMusic.Play();
            _titleMusic.Stop();
            _gameMusic.volume = 0.5f;
            titleMusicPlaying = false;
            gameMusicPlaying = true;
        }
    }

    public void GamePaused(bool paused)
    {
        if (paused)
        {
            _pause.Play();  
            _gameMusic.Pause();
            _titleMusic.Play();
        }
        if (!paused)
        {
            _unPause.Play();
            _gameMusic.UnPause();
            _titleMusic.Stop();
        } 
    }

    public void StartFadeTitleMusicOut()
    {
        StartCoroutine(FadeTitleMusic(true));
    }

    public void StartFadeTitleMusicIn()
    { 
        StartCoroutine(FadeTitleMusic(false));
    }

    public void StartFadeGameMusicOut()
    {
        StartCoroutine(FadeOutGameMusic(true));
    }

    public void StartFadeGameMusicIn()
    {
        StartCoroutine(FadeOutGameMusic(false));
    }

    private IEnumerator FadeTitleMusic(bool fadeOut)
    {
        if (fadeOut)
        {
            while (_titleMusic.volume > 0)
            {
                _titleMusic.volume -= 0.001f;
                yield return new WaitForSeconds(_fadeTimerInterval);
            }
        }
        else if (!fadeOut)
        {
            while (_titleMusic.volume < 0.5f)
            {
                _titleMusic.volume += 0.001f;
                yield return new WaitForSeconds(_fadeTimerInterval);
            }
        }

    }

    private IEnumerator FadeOutGameMusic(bool fadeOut)
    {
        if (fadeOut)
        {
            Debug.Log("game music fading out.");
            _gameMusic.volume = .5f;
            while (_gameMusic.volume > 0)
            {
                _gameMusic.volume -= 0.001f;
                yield return new WaitForSeconds(_fadeTimerInterval);
            }
        }
        else if (!fadeOut)
        {
            Debug.Log("game music fading in.");
            _gameMusic.volume = 0;
            while (_gameMusic.volume < 0.5f)
            {
                _gameMusic.volume += 0.001f;
                yield return new WaitForSeconds(_fadeTimerInterval);
            }
        }
    }

    public void PlayExplosion()
    {
        int clip = Random.Range(0, _explosionClips.Length);
        _explosionSource.pitch = Random.Range(0.9f, 1.1f);
        _explosionSource.PlayOneShot(_explosionClips[clip]);

    }
}
