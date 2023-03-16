using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _restartText;
    private byte alpha;

    [SerializeField] private int _score;
    [SerializeField] private Sprite[] _livesSprites;
    [SerializeField] private Image _livesImage;

    public void AddScore(int points)
    {
        _score += points;
        _scoreText.text = "Score: " + _score.ToString();
    }

    public void UpdateLives(int currentLives)
    {
        _livesImage.sprite = _livesSprites[currentLives];
    }

    public void OnGameOver()
    {
        StartCoroutine(EnableRestartText());
    }

    IEnumerator EnableRestartText()
    {
        yield return new WaitForSeconds(1f);
        while (alpha < 255)
        {
            alpha++;
            yield return new WaitForSeconds(0.01f);
            _restartText.color = new Color32(255, 0, 0, alpha);
        }
        
    }
}
