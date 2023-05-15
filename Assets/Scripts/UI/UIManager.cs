using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _ammoText;
    [SerializeField] private TMP_Text _restartText;
    [SerializeField] private GameObject _nukeIcon;
    [SerializeField] private GameObject _hudTextContainer;
    [SerializeField] private TMP_Text _hudText;
    private byte _alpha;
    private int _ammoCount;

    [SerializeField] private int _score;
    
    [SerializeField] private Sprite[] _livesSprites;
    [SerializeField] private Image _livesImage;

    [SerializeField] private Sprite[] _ammoSprites;
    [SerializeField] private Image _ammoImage;

    [SerializeField] private Animator _damageStreakAnim;

    public void AddScore(int points)
    {
        _score += points;
        _scoreText.text = "Score: " + _score.ToString();
    }

    public int GetScore()
    { 
        return _score;
    }

    public void UpdateLives(int currentLives)
    {
        _livesImage.sprite = _livesSprites[currentLives];
    }

    public void UpdateAmmoCount(int ammo)
    {
        _ammoCount = ammo;
        _ammoText.text = _ammoCount.ToString() + " Homing";
        _ammoImage.sprite = _ammoSprites[_ammoCount];
    }

    public void OnGameOver()
    {
        StartCoroutine(EnableRestartText());
    }

    IEnumerator EnableRestartText()
    {
        yield return new WaitForSeconds(1f);
        while (_alpha < 255)
        {
            _alpha++;
            yield return new WaitForSeconds(0.01f);
            _restartText.color = new Color32(255, 0, 0, _alpha);
        }
    }

    public void EnableNukeIcon()
    {
        _nukeIcon.SetActive(true);
    }

    public void DisableNukeIcon()
    {
        _nukeIcon.SetActive(false);
    }

    public void StartDamageStreaks()
    {
        _damageStreakAnim.SetBool("TookDamage", true);
        StartCoroutine(StopDamageStreaks());
    }

    IEnumerator StopDamageStreaks()
    {
        yield return new WaitForSeconds(.35f);
        _damageStreakAnim.SetBool("TookDamage", false);
    }

    public void UpdateHudText(string text)
    { 
        _hudText.text = text;
        _hudTextContainer.SetActive(true);
        StartCoroutine(HudTextTimer());
    }

    IEnumerator HudTextTimer()
    {
        yield return new WaitForSeconds(3);
        _hudTextContainer.SetActive(false);

    }
}
