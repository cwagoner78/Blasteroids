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
    public bool shieldsActive = false;

    private int _health = 1;
    private Rigidbody _rigidbody;
    private Animator _anim;
    private SpawnManager _spawnManagerAsteroid;
    private SpawnManager _spawnManagerEnemy;
    private SpawnManager _spawnManagerPowerUp;
    private ParticleSystem _speedBoostStream;
    private SpriteRenderer _shields;
    private GameOverAnimation _gameOver;
    private Shooting _shooting;
    private float _startingMoveForce;
    private float _inputX;
    private float _inputY;

    //UI Update variables
    [SerializeField] private int _score;
    private UIManager _uiManager;


    void Start()
    {
        _startingMoveForce = _moveForce;
        _rigidbody = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();
        transform.position = new Vector3(0, 0, 0);
        _spawnManagerAsteroid = GameObject.Find("AsteroidSpawner").GetComponent<SpawnManager>();
        _spawnManagerEnemy = GameObject.Find("EnemySpawner").GetComponent<SpawnManager>();
        _spawnManagerPowerUp = GameObject.Find("PowerUpSpawner").GetComponent<SpawnManager>();
        _gameOver = FindObjectOfType<GameOverAnimation>();
        _uiManager = FindObjectOfType<UIManager>();
        _uiManager.UpdateLives(_lives);
        _speedBoostStream = GameObject.Find("PlayerSpeedStream").GetComponent<ParticleSystem>();
        _shields = GameObject.Find("ShieldSprite").GetComponent<SpriteRenderer>();
        _shooting = FindObjectOfType<Shooting>();
        if (_spawnManagerAsteroid == null || _spawnManagerEnemy == null || _spawnManagerPowerUp == null) Debug.LogError("Spawner equals NULL");
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
        _speedBoostActive = false;
    }

    public void ShieldPowerUp()
    {
        shieldsActive = true;
        _shields.enabled = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        shieldsActive = false;
        _shields.enabled = false;
    }

    public void Damage(int damage)
    {
        if (shieldsActive) return;
        else _health -= damage;

        if (_health <= 0)
        {
            _lives--;
            _uiManager.UpdateLives(_lives);
            transform.position = new Vector3(0, 0, 0);
            _shooting.DisableTripleShot();
            _health = _startingHealth;
        }

        if (_lives == 0) GameOver();

    }

    public void GameOver()
    {
        gameObject.SetActive(false);
        _spawnManagerAsteroid.OnGameOver();
        _spawnManagerEnemy.OnGameOver();
        _spawnManagerPowerUp.OnGameOver();
        _uiManager.OnGameOver();
        _gameOver.OnGameOver();

    }


}
