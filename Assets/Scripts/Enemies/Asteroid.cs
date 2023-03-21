using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private bool _isWaveAsteroid;
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _rotSpeed;
    [SerializeField] private GameObject _asteroid;
    [SerializeField] private ParticleSystem _explosion;
    [SerializeField] private Collider _collider;
    [SerializeField] private MeshRenderer _mesh;
    [SerializeField] private Rigidbody _rb;
    private GameObject _spawnContainer;

    private Vector3 _rotation;

    [Header("Damage and Point Val")]
    [SerializeField] private int _damage = 1;
    [SerializeField] private int _pointVal = 1;
    private UIManager _uiManager;
    private Player _player;
    private SpawnManager _asteroidSpawner;
    private SpawnManager _enemySpawner;
    private SpawnManager _powerUpSpawner;
    private AudioManager _audioManager;



    void Start()
    {
        _spawnContainer = GameObject.Find("AsteroidContainer");
        if (_spawnContainer == null) Debug.LogError("Asteriod Container is NULL");

        _uiManager = FindObjectOfType<UIManager>();
        if (_spawnContainer == null) Debug.LogError("_uiManager is NULL");

        _asteroidSpawner = GameObject.Find("AsteroidSpawner").GetComponent<SpawnManager>();
        if (_asteroidSpawner == null) Debug.LogError("_asteroidSpawner is NULL");

        _enemySpawner = GameObject.Find("EnemySpawner").GetComponent<SpawnManager>();
        if (_enemySpawner == null) Debug.LogError("_enemySpawner is NULL");

        _powerUpSpawner = GameObject.Find("PowerUpSpawner").GetComponent<SpawnManager>();
        if (_powerUpSpawner == null) Debug.LogError("_powerUpSpawner is NULL");

        _player = FindObjectOfType<Player>();
        //if (_player == null) Debug.LogError("_player is NULL");

        _audioManager = FindObjectOfType<AudioManager>();
        if (_audioManager == null) Debug.LogError("_audioManager is NULL");

        _rb.AddForce(Vector3.down * Random.Range(_movementSpeed, _movementSpeed * 2), ForceMode.Impulse);
        _rotation = new Vector3(Random.Range(-_rotSpeed, _rotSpeed), Random.Range(-_rotSpeed, _rotSpeed), Random.Range(-_rotSpeed, _rotSpeed));
    }

    void Update()
    {
        HandleMovement();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_isWaveAsteroid)
        {
            _asteroidSpawner.StartSpawning();
            _enemySpawner.StartSpawning();
            _powerUpSpawner.StartSpawning();
            _audioManager.StartGameMusic();
            _audioManager.PlayExplosion();

        } 

        if (collision.gameObject.CompareTag("Projectile"))
        {

            Destroy(collision.gameObject);

            if (transform.localScale.x > .25f)
            {
                _audioManager.PlayExplosion();
                _explosion.Play();
                transform.localScale = transform.localScale / 2;
                _rb.mass = _rb.mass / 1.5f;
                GameObject newSpawn = Instantiate(_asteroid);
                newSpawn.transform.parent = _spawnContainer.transform;
                _uiManager.AddScore(_pointVal);
            }
            else
            {
                _audioManager.PlayExplosion();
                _explosion.Play();
                _mesh.enabled = false;
                _collider.enabled = false;
                Destroy(gameObject, 3f);
                _uiManager.AddScore(_pointVal);
            } 
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (_spawnContainer == null) return;
            if (transform.localScale.x > .25f)
            {
                _audioManager.PlayExplosion();
                _explosion.Play();
                transform.localScale = transform.localScale / 2;
                _rb.mass = _rb.mass / 1.5f;
                GameObject newSpawn = Instantiate(_asteroid);
                newSpawn.transform.parent = _spawnContainer.transform;
            }
            else
            {
                _audioManager.PlayExplosion();
                _explosion.Play();
                _mesh.enabled = false;
                _collider.enabled = false;
                Destroy(gameObject, 3f);
            }
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.GetComponent<Player>().Damage(_damage);

            if (transform.localScale.x > .25f)
            {
                _audioManager.PlayExplosion();
                _explosion.Play();
                transform.localScale = transform.localScale / 2;
                _rb.mass = _rb.mass / 1.5f;
                GameObject newSpawn = Instantiate(_asteroid);
                newSpawn.transform.parent = _spawnContainer.transform;
            }
            else
            {
                _audioManager.PlayExplosion();
                _explosion.Play();
                _mesh.enabled = false;
                _collider.enabled = false;
                Destroy(gameObject, 3f);
            }
        }
    }
    void HandleMovement()
    {
        transform.Rotate(_rotation);

        //Bounds
        if (transform.position.x > 30 ||
            transform.position.x < -30 ||
            transform.position.y > 30 ||
            transform.position.y < -30) Destroy(gameObject);
    }
}
