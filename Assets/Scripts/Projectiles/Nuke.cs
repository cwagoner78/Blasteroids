using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nuke : MonoBehaviour
{
    [SerializeField] int _killPointValue = 100;
    private CameraShake _shake;
    private UIManager _uiManager;
    private AudioManager _audioManager;

    private void Start()
    {
        _shake = FindObjectOfType<CameraShake>();
        if (_shake == null) Debug.Log("_shake is NULL");

        _uiManager = FindObjectOfType<UIManager>();
        if (_uiManager == null) Debug.Log("_uiManager is NULL");

        _audioManager = FindObjectOfType<AudioManager>();
        if (_audioManager == null) Debug.Log("_audioManager is NULL");
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag != "Player")
        {
            _audioManager.PlayExplosion();
            _uiManager.AddScore(_killPointValue);
            _shake.ShakeCamera();
            Destroy(other.gameObject);
        } 
    }
}
