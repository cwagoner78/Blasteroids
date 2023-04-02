using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _moveForce = 5;
    [SerializeField] private bool _speedBoostActive = false;
    [SerializeField] private float _speedBoostMultiplier = 1.5f;
    [SerializeField] private Slider _boostSlider;
    public int maxBoost = 500;
    [SerializeField] private int _boostDecrement = 1;
    [HideInInspector]
    public int currentBoost;

    [Header("Boundaries")]
    [SerializeField] private float _xBounds = 12f;
    [SerializeField] private float _yBounds = 5;

    [Header("Player Health")]
    [SerializeField] private int _startingHealth = 1;
    public int lives = 3;
    [SerializeField] private float _invincibilityTimer = 3f;
    private bool _shieldsActive = false;
    public int shieldHealth;
    private bool _isInvincible = false;
    private int _health = 1;

    [Header("Effects")]
    [SerializeField] private ParticleSystem _explosionEffect;
    [SerializeField] private GameObject _explosionEffectPrefab;
    [SerializeField] private ParticleSystem _leftDamage;
    [SerializeField] private ParticleSystem _rightDamage;
    [SerializeField] private GameObject _jets;

    private Rigidbody _rigidbody;
    private Animator _anim;
    private UIManager _uiManager;
    private GameManager _gameManager;
    private SpawnManager _spawnManagerAsteroid;
    private SpawnManager _spawnManagerEnemy;
    private SpawnManager _spawnManagerPowerUp;
    private AudioManager _audioManager; 
    private ParticleSystem _speedBoostStream;
    private TrailRenderer _thruster;
    private SpriteRenderer _shields;
    private Collider[] _colliders;
    private MeshRenderer _renderer;
    private AudioSource _source;
    private GameOverAnimation _gameOver;
    private Shooting _shooting;
    private CameraShake _shake;
    private float _startingMoveForce;
    private float _inputX;
    private float _inputY;
    private bool _inputEnabled = true;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        if (_rigidbody == null) Debug.LogError("player _rb is NULL");

        _anim = GetComponent<Animator>();
        if (_anim == null) Debug.LogError("player _anim is NULL");

        _gameManager = FindObjectOfType<GameManager>();
        if (_gameManager == null) Debug.LogError("_gameManager is NULL");

        _spawnManagerAsteroid = GameObject.Find("AsteroidSpawner").GetComponent<SpawnManager>();
        if (_spawnManagerAsteroid == null) Debug.LogError("_spawnManagerAsteroid is NULL");

        _spawnManagerEnemy = GameObject.Find("EnemySpawner").GetComponent<SpawnManager>();
        if (_spawnManagerEnemy == null) Debug.LogError("_spawnManagerEnemy is NULL");

        _spawnManagerPowerUp = GameObject.Find("PowerUpSpawner").GetComponent<SpawnManager>();
        if (_spawnManagerPowerUp == null) Debug.LogError("_spawnManagerPowerUp is NULL");

        _audioManager = FindObjectOfType<AudioManager>();
        if (_audioManager == null) Debug.LogError("_audioManager is NULL");

        _gameOver = FindObjectOfType<GameOverAnimation>();
        if (_gameOver == null) Debug.LogError("_gameOver is NULL");

        _uiManager = FindObjectOfType<UIManager>();
        if (_uiManager == null) Debug.LogError("_uiManager is NULL");

        _speedBoostStream = GameObject.Find("PlayerSpeedStream").GetComponent<ParticleSystem>();
        if (_speedBoostStream == null) Debug.LogError("_speedBoostStream is NULL");

        _thruster = GameObject.Find("Thruster").GetComponent<TrailRenderer>();
        if (_speedBoostStream == null) Debug.LogError("_speedBoostStream is NULL");

        _shields = GameObject.Find("ShieldSprite").GetComponent<SpriteRenderer>();
        if (_shields == null) Debug.LogError("_shields is NULL");

        _shooting = FindObjectOfType<Shooting>();
        if (_shooting == null) Debug.LogError("_shooting is NULL");

        _colliders = GetComponents<Collider>();
        if (_colliders == null) Debug.LogError("_collider is NULL");

        _renderer = GetComponent<MeshRenderer>();
        if (_renderer == null) Debug.LogError("_renderer is NULL");

        _source = GetComponent<AudioSource>();
        if (_source == null) Debug.LogError("_source is NULL");

        _shake = FindObjectOfType<CameraShake>();
        if (_shake == null) Debug.LogError("_shake is NULL");

        transform.position = new Vector3(0, 0, 0);
        _startingMoveForce = _moveForce;
        _uiManager.UpdateLives(lives);

        _boostSlider.maxValue = maxBoost;
        currentBoost = maxBoost;
        _boostSlider.value = currentBoost;

    }

    void FixedUpdate()
    {
        if (!_gameManager.gamePaused && _inputEnabled)
        {
            CheckForSpeedBoost();
            HandleMovement();
            HandleAnimation();
        }
    }

    void HandleMovement()
    {
        //Movement
        _inputX = Input.GetAxisRaw("Horizontal");
        _inputY = Input.GetAxisRaw("Vertical");
        Vector3 movement = new Vector3(_inputX, _inputY);

        if (_speedBoostActive && _moveForce < _startingMoveForce * _speedBoostMultiplier)
        {
            _moveForce *= _speedBoostMultiplier;
            _speedBoostStream.Play();
            _thruster.enabled = true;

        }
        else if (!_speedBoostActive)
        {
            _moveForce = _startingMoveForce;
            _speedBoostStream.Stop();
            _thruster.enabled = false;
        }

        _rigidbody.AddForce(movement * _moveForce);

        //Shield Movement
        _shields.transform.position = transform.position - new Vector3(0,0,1);

        //Bounds
        Vector3 position = transform.position;
        if (position.y >= _yBounds * 1.5f) _rigidbody.AddForce(movement.x, movement.y - _yBounds * (_moveForce / 4), movement.z);
        if (position.y <= -_yBounds * 1.5f) _rigidbody.AddForce(movement.x, movement.y + _yBounds * (_moveForce / 4), movement.z);
        if (position.x >= _xBounds * 1.5f) _rigidbody.AddForce(movement.x - _xBounds * (_moveForce / 4), movement.y, movement.z);
        if (position.x <= -_xBounds * 1.5f) _rigidbody.AddForce(movement.x + _xBounds * (_moveForce / 4), movement.y, movement.z);
    }

    void HandleAnimation()
    {
        if (_inputX < -0.2f)
        {
            _anim.SetBool("MovingLeft", true);
            _anim.SetBool("MovingRight", false);
            _anim.SetBool("Idle", false);
        }
        else if (_inputX > 0.2f)
        {
            _anim.SetBool("MovingLeft", false);
            _anim.SetBool("MovingRight", true);
            _anim.SetBool("Idle", false);
        }
        else if (_inputX > -0.2f && _inputX < 0.2f)
        {
            _anim.SetBool("MovingLeft", false);
            _anim.SetBool("MovingRight", false);
            _anim.SetBool("Idle", true);
        }
    }


    public void SpeedBoostGained()
    {
        currentBoost = maxBoost;
    }

    void CheckForSpeedBoost()
    {
        if (Input.GetKey(KeyCode.LeftShift) && currentBoost > 0)
        {
            _speedBoostActive = true;
            StartCoroutine(DecreaseBoost());
        } 
        else _speedBoostActive = false;

        _boostSlider.value = currentBoost;
    }

    IEnumerator DecreaseBoost()
    {
        while (_speedBoostActive && currentBoost > 0)
        {
            currentBoost -= _boostDecrement;
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void ShieldPowerUp()
    {
        _source.volume = .5f;
        _source.Play();
        _shieldsActive = true;
        _shields.enabled = true;
        _shields.color = new Color32(0, 255, 255, 65);
        shieldHealth = 3;
    }

    public void Damage(int damage)
    {
        if (_isInvincible) return;
        else if (_shieldsActive)
        {
            shieldHealth -= 1;
            _explosionEffect.Play();
            _audioManager.PlayExplosion();
            if (shieldHealth <= 0)
            {
                _shieldsActive = false;
                _shields.enabled = false;
                _source.Stop();
            }
        }
        else
        {
            _health -= damage;
            _explosionEffect.Play();
            _audioManager.PlayExplosion();
            StartCoroutine(InvincibilityRoutine());
            _uiManager.StartDamageStreaks();
            _shake.ShakeCamera();
        }

        if (_health <= 0)
        {
            GameObject newInstance = Instantiate(_explosionEffectPrefab, transform.position, Quaternion.identity);
            newInstance.GetComponent<ParticleSystem>().Play();
            lives--;
            _uiManager.UpdateLives(lives);
            _shooting.DisableTripleShot();
            _health = _startingHealth;
        }

        if (shieldHealth == 2) _shields.color = new Color32(255, 255, 0, 65);
        if (shieldHealth == 1) _shields.color = new Color32(255, 0, 0, 65);

        if (lives == 2) _leftDamage.Play();
        if (lives == 1) _rightDamage.Play();
        if (lives == 0) GameOver();

    }

    public void HealthGained()
    {
        if (lives < 3) lives++;
        _uiManager.UpdateLives(lives);
        if (lives == 2) _rightDamage.Stop();
        if (lives == 3) _leftDamage.Stop();
    }

    IEnumerator InvincibilityRoutine()
    {
        _isInvincible = true;
        foreach (Collider col in _colliders) col.enabled = false;
        yield return new WaitForSeconds(_invincibilityTimer);
        _isInvincible = false;
        foreach (Collider col in _colliders) col.enabled = true;
    }

    public void GameOver()
    {
        _inputEnabled = false;
        _gameOver.OnGameOver();
        _gameManager.GameOver();
        _spawnManagerAsteroid.OnGameOver();
        _spawnManagerEnemy.OnGameOver();
        _spawnManagerPowerUp.OnGameOver();
        _uiManager.OnGameOver();

        gameObject.SetActive(false);
    }
}
