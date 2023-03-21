using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private AudioManager _audioManager;
    [SerializeField] private Animator _fade;

    private void Start()
    {
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        yield return new WaitForSeconds(1f);
        _fade.enabled= false;
    }

    public void LoadGame()
    {
        StartCoroutine(LoadGameRoutine());
        _audioManager.StartFadeOutMenuMusic() ;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    IEnumerator LoadGameRoutine()
    {
        _fade.enabled = true;
        _fade.SetBool("FadeOut", true);
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(1);
    }

}
