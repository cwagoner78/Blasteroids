using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player Controls")]
    [SerializeField] private float _moveForce = 5;
    [SerializeField] private float _speedBoostMultiplier = 1.5f;
    [SerializeField] private float _speedBoostCoolDown = 5f;
    [SerializeField] private bool _speedBoostActive = false;

    [Header("Boundaries")]
    [SerializeField] private float _xBounds = 12f;
    [SerializeField] private float _yBounds = 5;

    [Header("Player Health")]
    [SerializeField] private int _startingHealth = 1;
    [SerializeField] private int _lives = 3;
    [SerializeField] private float _invincibilityTimer = 3f;
    private bool _isInvincible = false;
    public bool shieldsActive = false;
    [SerializeField] private ParticleSystem _explosionEffect;
    [SerializeField] private ParticleSystem _leftDamage;
    [SerializeField] private ParticleSystem _rightDamage;

    private int _health = 1;
    private Rigidbody _rigidbody;
    private Animator _anim;
    private GameManager _gameManager;
    private SpawnManager _spawnManagerAsteroid;
    private SpawnManager _spawnManagerEnemy;
    private SpawnManager _spawnManagerPowerUp;
    private ParticleSystem _speedBoostStream;
    private TrailRenderer _thruster;
    private SpriteRenderer _shields;
    private Collider _collider;
    private MeshRenderer _renderer;

    private GameOverAnimation _gameOver;
    private Shooting _shooting;
    private float _startingMoveForce;
    private float _inputX;
    private float _inputY;

    //UI Update variables
    private UIManager _uiManager;


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

        _collider = GetComponent<Collider>();
        if (_collider == null) Debug.LogError("_collider is NULL");

        _renderer = GetComponent<MeshRenderer>();
        if (_renderer == null) Debug.LogError("_renderer is NULL");

        transform.position = new Vector3(0, 0, 0);
        _startingMoveForce = _moveForce;
        _uiManager.UpdateLives(_lives);

    }

    void FixedUpdate()
    {
        HandleMovement();
        HandleAnimation();
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
            StartCoroutine(SpeedUpTimer(_speedBoostCoolDown));
        } 
        _rigidbody.AddForce(movement * _moveForce);

        //Shield Movement
        if (shieldsActive) ShieldPowerUp();
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

    public void SpeedPowerUp()
    {
        _speedBoostActive = true;
    }

    IEnumerator SpeedUpTimer(float timer)
    {
        yield return new WaitForSeconds(timer);
        _moveForce = _startingMoveForce;
        _speedBoostStream.Stop();
        _thruster.enabled = false;
        _speedBoostActive = false;
    }

    public void ShieldPowerUp()
    {
        shieldsActive = true;
        _shields.enabled = true;

    }

    public void Damage(int damage)
    {
        if (_isInvincible) return;
        else if (shieldsActive)
        {
            shieldsActive = false;
            _shields.enabled = false;
            StartCoroutine(InvincibilityRoutine());
        }
        else
        {
            _health -= damage;
            _explosionEffect.Play();
            StartCoroutine(InvincibilityRoutine());
        } 

        if (_health <= 0)
        {
            _lives--;
            _uiManager.UpdateLives(_lives);
            _shooting.DisableTripleShot();
            _health = _startingHealth;
        }

        if (_lives == 2) _leftDamage.Play();
        if (_lives == 1) _rightDamage.Play();
        if (_lives == 0) GameOver();

    }

    IEnumerator InvincibilityRoutine()
    {
        _isInvincible = true;
        _collider.enabled = false;
        yield return new WaitForSeconds(_invincibilityTimer);
        _isInvincible = false;
        _collider.enabled = true;
    }

    public void GameOver()
    {
        _gameOver.OnGameOver();
        _gameManager.GameOver();
        _spawnManagerAsteroid.OnGameOver();
        _spawnManagerEnemy.OnGameOver();
        _spawnManagerPowerUp.OnGameOver();
        _uiManager.OnGameOver();
        gameObject.SetActive(false);

    }


}
