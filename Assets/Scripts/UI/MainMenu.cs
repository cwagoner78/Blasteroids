using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private AudioManager _audioManager;


    public void LoadGame()
    {
        SceneManager.LoadScene(1);
        _audioManager.StartFade() ;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
