using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEditor.VersionControl;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private int _score;

    void Update()
    {
        _scoreText.text = "Score: " + _score.ToString();

    }

    public void AddScore(int points)
    {
        _score += points;

    }
}
