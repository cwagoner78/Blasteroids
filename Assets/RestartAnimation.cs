using System.Collections;
using TMPro;
using UnityEngine;

public class RestartAnimation : MonoBehaviour
{
    [SerializeField] TMP_Text _text;
    [SerializeField] float _animInterval = 0.01f;
    [SerializeField] float _animSpeed = 0.1f;
    [SerializeField] float _fadeSpeed = 0.01f;
    Material _mat;
    byte alpha;

    float bevOffset;

    bool _gameOver = false;

    private void Start()
    {
        _mat = _text.fontSharedMaterial;
    }

    private void Update()
    {
        if (_gameOver)
        {
            StartCoroutine(FadeIn());
            StartCoroutine(Animation());
            _gameOver = false;
        }
    }

    IEnumerator Animation()
    {
        while (bevOffset < 0.11f)
        {
            _mat.SetFloat("_BevelOffset", bevOffset += _animInterval);
            yield return new WaitForSeconds(_animSpeed);
            if (bevOffset >= 0.1f) bevOffset = -0.1f;
        }
    }

    IEnumerator FadeIn()
    {
        while (alpha < 255)
        {
            alpha++;
            _text.color = new Color32(255, 0, 0, alpha);
            yield return new WaitForSeconds(_fadeSpeed);
        }
    }

    IEnumerator Flicker()
    {
        while (alpha > 100)
        {
            alpha--;
            _text.color = new Color32(255, 0, 0, alpha);
            yield return new WaitForSeconds(_fadeSpeed);
            if (alpha == 255) StartCoroutine(FadeIn());
        }

    }

    public void OnGameOver()
    {
        _gameOver = true;
    }
}
