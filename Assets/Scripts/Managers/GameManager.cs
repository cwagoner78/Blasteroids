using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private bool _isGameOver;

    private void Update()
    {
        ManageScene();
    }

    void ManageScene()
    {
        if (Input.GetKeyDown(KeyCode.R) && _isGameOver) SceneManager.LoadScene(1); //Game Scene
        if (Input.GetKeyDown(KeyCode.Q) && _isGameOver) SceneManager.LoadScene(0); //Main Menu
        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
    }


    public void GameOver()
    {
        _isGameOver = true;
    }



}