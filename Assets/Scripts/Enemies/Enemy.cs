using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed = 1f;
    [SerializeField] private float _yPosBound = 15f;
    [SerializeField] private Collider[] _colliders;
    [SerializeField] private ParticleSystem _explosion;
    [SerializeField] private MeshRenderer _mesh;

    [Header("Fighter Controls")]
    [SerializeField] private bool _canShoot;
    [SerializeField] private GameObject _enemyLaserPrefab;
    [SerializeField] private int _enemyLaserDamageValue = 1;
    [SerializeField] private float _fireRate = 3;
    private float _canFire = 2;

    [Header("Damage and Point Val")]
    [SerializeField] private int _damage = 1;
    [SerializeField] private int _pointVal = 10;
    private UIManager _uiManager;
    private AudioManager _audioManager;
    private AudioSource _enemyLaserSound;

    private bool _isAlive = true;


    private void Start()
    {
        _enemyLaserSound = GameObject.Find("EnemyLaserSound").GetComponent<AudioSource>();
        if (_enemyLaserSound == null) Debug.LogError("_enemyLaserSound is Null");

        _uiManager = FindObjectOfType<UIManager>();
        if (_uiManager == null) Debug.LogError("_uiManager is NULL");

        _audioManager = FindObjectOfType<AudioManager>();
        if (_audioManager == null) Debug.LogError("_audioManager is NULL");

        _speed = Random.Range(_speed / 1.25f, _speed * 1.25f);
    }

    void FixedUpdate()
    {
        CalculateMovement();
        if (_isAlive) Shoot();
    }

    private void OnCollisionEnter(Collision collision)
    {
        _colliders = transform.GetComponentsInChildren<Collider>();
        _isAlive = false;

        if (collision.gameObject.CompareTag("Projectile"))
        {
            _audioManager.PlayExplosion();
            Destroy(collision.gameObject);
            _explosion.Play();
            _mesh.enabled = false;
            foreach (Collider col in _colliders) col.enabled= false;
            Destroy(gameObject, 3f);
            _uiManager.AddScore(_pointVal);
        }

        if (collision.gameObject.CompareTag("Asteroid"))
        {
            _explosion.Play();
            _mesh.enabled = false;
            foreach (Collider col in _colliders) col.enabled = false;
            Destroy(gameObject, 3f);
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            _audioManager.PlayExplosion();
            collision.transform.GetComponent<Player>().Damage(_damage);
            _explosion.Play();
            _mesh.enabled = false;
            foreach (Collider col in _colliders) col.enabled = false;
            Destroy(gameObject, 3f);
        }
    }

    void CalculateMovement()
    {
        transform.Translate(Vector3.forward * _speed * Time.deltaTime);
        if (transform.position.y < -_yPosBound + 10f) Destroy(gameObject);
    }

    void Shoot()
    {
        if (_canShoot && Time.time > _canFire)
        {
            _enemyLaserSound.Play();
            _fireRate = Random.Range(_fireRate / 2f, _fireRate * 2f);
            _canFire = Time.time + _fireRate;
            Instantiate(_enemyLaserPrefab, transform.position + new Vector3(0, -.75f, 0), transform.rotation);

            //Debug.Break();
        }
    }
}

