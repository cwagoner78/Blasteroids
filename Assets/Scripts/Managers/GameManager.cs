using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private bool _isGameOver;
    [SerializeField] private Animator _fade;
    private AudioManager _audioManager;

    private void Start()
    {
        _audioManager = FindObjectOfType<AudioManager>();
        if (_audioManager == null) Debug.LogError("_audioManager is NULL");

        _fade.SetBool("FadeIn", true);
    }

    private void Update()
    {
        ManageScene();
    }

    void ManageScene()
    {
        if (Input.GetKeyDown(KeyCode.R) && _isGameOver) StartCoroutine(LoadSceneRoutine());
        if (Input.GetKeyDown(KeyCode.Q) && _isGameOver) StartCoroutine(LoadMenuRoutine());
        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
    }

    public void GameOver()
    {
        _isGameOver = true;
    }

    IEnumerator LoadSceneRoutine()
    {
        _fade.SetBool("FadeOut", true);
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(1); //Game Scene
    }

    IEnumerator LoadMenuRoutine()
    {
        _audioManager.StartFadeOutGameMusic();
        _fade.SetBool("FadeOut", true);
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(0); //Main Menu
    }



}