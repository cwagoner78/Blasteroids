using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private bool _isGameOver;
    [SerializeField] private Animator _fade;
    [SerializeField] private GameObject _pauseScreen; 
    private AudioManager _audioManager;
    private PowerUp _powerUp;
    public Scene currentScene;
    public bool gamePaused;

    private void Start()
    {
        _audioManager = FindObjectOfType<AudioManager>();
        if (_audioManager == null) Debug.LogError("_audioManager is NULL");

        _fade.SetBool("FadeIn", true);

        currentScene = SceneManager.GetActiveScene();   
    }

    private void Update()
    {
        ManageScene();
    }

    void ManageScene()
    {
        if (Input.GetKeyDown(KeyCode.R) && _isGameOver) StartCoroutine(LoadGameRoutine());
        if (Input.GetKeyDown(KeyCode.Q) && _isGameOver) StartCoroutine(LoadMenuRoutine());
        if (currentScene.name == "Title" && Input.GetKeyDown(KeyCode.Escape)) QuitGame();
        if (currentScene.name == "Game" && Input.GetKeyDown(KeyCode.Escape)) SetPauseGame();
        if (gamePaused && Input.GetKeyDown(KeyCode.Q)) QuitGame();

    }

    void SetPauseGame()
    {
        if (_powerUp == null) _powerUp = FindObjectOfType<PowerUp>();

        if (!gamePaused)
        {
            Debug.Log("Game Paused");
            gamePaused = true;
            _pauseScreen.SetActive(true);
            _audioManager.GamePaused(true);
            _powerUp.GamePaused(true);
            Time.timeScale = 0;

        }
        else
        {
            Debug.Log("Game Resumed");
            Time.timeScale = 1;
            _pauseScreen.SetActive(false);
            _audioManager.GamePaused(false);
            _powerUp.GamePaused(false);
            gamePaused = false;
        }
    }


    public void GameOver()
    {
        _isGameOver = true;
        _audioManager.StartTitleMusic();
    }

    IEnumerator LoadGameRoutine()
    {
        _fade.SetBool("FadeOut", true);
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(1); //Game Scene
    }

    IEnumerator LoadMenuRoutine()
    {
        _audioManager.StartFadeGameMusicOut();
        _fade.SetBool("FadeOut", true);
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(0); //Main Menu
    }

    void QuitGame()
    {
        Debug.Log("Application quit...");
        Application.Quit();
    }



}