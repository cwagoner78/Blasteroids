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
    [SerializeField] private Collider _collisionDetect;
    [SerializeField] private Animator _anim;

    [Header("Controls")]
    [SerializeField] private float _turnSpeed = 0.5f;
    [SerializeField] private bool _canShoot;
    [SerializeField] private GameObject _enemyLaserPrefab;
    [SerializeField] private float _fireRate = 3;
    private float _canFire = 2;

    [Header("Damage and Point Val")]
    [SerializeField] private int _damage = 1;
    [SerializeField] private int _pointVal = 10;
    private UIManager _uiManager;
    private AudioManager _audioManager;
    private AudioSource _enemyLaserSound;

    private bool _isAlive = true;
    private bool _movingLeft = false;
    private bool _movingRight = false;



    private void Start()
    {
        _enemyLaserSound = GameObject.Find("EnemyLaserSound").GetComponent<AudioSource>();
        if (_enemyLaserSound == null) Debug.LogError("_enemyLaserSound is Null");

        _uiManager = FindObjectOfType<UIManager>();
        if (_uiManager == null) Debug.LogError("_uiManager is NULL");

        _audioManager = FindObjectOfType<AudioManager>();
        if (_audioManager == null) Debug.LogError("_audioManager is NULL");

        _anim = GetComponent<Animator>();
        if (_anim == null) Debug.LogError("_anim is NULL");

        _speed = Random.Range(_speed / 1.25f, _speed * 1.25f);
    }

    void FixedUpdate()
    {
        CalculateMovement();
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player" && other.transform.position.x - transform.position.x > 0)
        {
            _movingRight = true;
        }

        if (other.tag != "Player" && other.transform.position.x - transform.position.x < 0)
        {
            _movingLeft = true;
        }

        if (other.tag == "Player" || other.tag == "Asteroid") Shoot();
    }

    private void OnTriggerExit(Collider other)
    {
        _movingRight = false;
        _movingLeft = false;

    }

    void CalculateMovement()
    {
        Vector3 forward = new Vector3(0,0,1);
        Vector3 left = new Vector3(-_turnSpeed, 0,1);
        Vector3 right = new Vector3(_turnSpeed, 0,1);
        Vector3 position = transform.position;

        if (_movingLeft)
        {
            transform.Translate(left * _speed * Time.deltaTime);
            _anim.SetBool("MovingLeft", true);
        }
        else if (_movingRight)
        {
            transform.Translate(right * _speed * Time.deltaTime);
            _anim.SetBool("MovingRight", true);
        }
        else if (!_movingLeft || !_movingRight)
        {
            transform.Translate(forward * _speed * Time.deltaTime);
            _anim.SetBool("MovingLeft", false);
            _anim.SetBool("MovingRight", false);
        }

        if (position.y < -_yPosBound + 10f) Destroy(gameObject);
        if (position.z > 0 || position.z < 0) position = new Vector3(position.x, position.y, 0);
    }

    void Shoot()
    {
        if (_canShoot && Time.time > _canFire)
        {
            _enemyLaserSound.Play();
            _fireRate = Random.Range(_fireRate / 2f, _fireRate * 2f);
            _canFire = Time.time + _fireRate;
            Instantiate(_enemyLaserPrefab, transform.position + new Vector3(0, -1f, 0), transform.rotation);
        }
    }
}

