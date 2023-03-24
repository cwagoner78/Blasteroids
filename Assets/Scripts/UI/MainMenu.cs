using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    private AudioManager _audioManager;
    [SerializeField] private Animator _fade;

    [SerializeField] private GameObject _startButton;
    [SerializeField] private GameObject _quitButton;

    private void Start()
    {
        _audioManager = FindObjectOfType<AudioManager>();
        if (_audioManager == null) Debug.LogError("_audioManager is NULL");

        StartCoroutine(FadeIn());

        _audioManager.StartTitleMusic();
    }

    IEnumerator FadeIn()
    {
        yield return new WaitForSeconds(1f);
        _fade.enabled= false;
    }

    public void LoadGame()
    {
        StartCoroutine(LoadGameRoutine());
        _audioManager.StartFadeTitleMusicOut();
        _audioManager.PlayExplosion();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    IEnumerator LoadGameRoutine()
    {
        _startButton.SetActive(false);
        _quitButton.SetActive(false);
        _fade.enabled = true;
        _fade.SetBool("FadeOut", true);
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(1);
    }

}
