using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] private int _powerUpID; // '0' = SpeedBoost, '1' = TripleShot, '2' Shields
    [SerializeField] private float _moveSpeed = 3f;
    [SerializeField] private ParticleSystem _trailParticles;
    [SerializeField] private ParticleSystem _collectParticles;

    private Collider[] _colliders;
    private Player _player;
    private Shooting _shooting;
    private AudioSource _audioSource;
    private AudioSource _collectSound;

    [Header("Point Value")]
    [SerializeField] private int _pointVal = 50;
    private UIManager _uiManager;

    // Start is called before the first frame update
    void Start()
    {
        _player = FindObjectOfType<Player>();
        if (_player == null) Debug.LogError("_player is NULL");

        _shooting = GameObject.Find("Player").GetComponent<Shooting>();
        if (_shooting == null) Debug.LogError("_shooting is NULL");

        _uiManager = FindObjectOfType<UIManager>();
        if (_uiManager == null) Debug.LogError("_uiManager is NULL");

        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null) Debug.LogError("_audioSource is NULL");

        _collectSound = GameObject.Find("CollectPowerUpSound").GetComponent<AudioSource>();
        if (_collectSound == null) Debug.LogError("_collectSound is NULL");

        StartCoroutine(SoundFadeIn());
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * Random.Range(_moveSpeed / 1.5f, _moveSpeed * 1.5f) * Time.deltaTime);
        if (transform.position.y < -20f) Destroy(gameObject); 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _audioSource.Stop();
            _collectSound.Play();
            switch (_powerUpID)
            {
                case 0: _player.SpeedPowerUp(); break;
                case 1: _shooting.TripleShotActive(); break;
                case 2: _player.ShieldPowerUp(); break;
                default: Debug.Log("Default Value"); break;
            }

            //Disable colliders
            _colliders = GetComponents<Collider>();
            for (int i = 0; i < _colliders.Length; i++)
            {
                _colliders[i].enabled = false;
            }

            _uiManager.AddScore(_pointVal);
            transform.GetComponent<SpriteRenderer>().enabled = false;
            _collectParticles.Play();
            _trailParticles.Stop();
            GetComponentInChildren<Light>().enabled = false;
            Destroy(gameObject, 3);
        }
    }

    IEnumerator SoundFadeIn()
    {
        while (_audioSource.volume < 1)
        {
            _audioSource.volume += 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
    }

    public void GamePaused(bool paused)
    {
        if (paused) _audioSource.Pause();
        if (!paused) _audioSource.UnPause();
    }

}
