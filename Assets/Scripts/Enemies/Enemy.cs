using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Class")]
    [SerializeField] private bool isBug;
    [SerializeField] private bool isFighter;

    [Header("Components")]
    [SerializeField] private ParticleSystem _explosion;
    [SerializeField] private MeshRenderer _mesh;
    [SerializeField] private Animator _anim;
    [SerializeField] private GameObject _shield;
    [SerializeField] private GameObject _homingTrigger;
    private Collider[] _colliders;

    [Header("Controls")]
    [SerializeField] private float _speed = 1f;
    [SerializeField] private float _turnSpeed = 0.5f;
    [SerializeField] private GameObject _enemyLaserPrefab;
    [SerializeField] private float _fireRate = 3;
    private float _canFire = 2;
    private bool _canBeTargeted = true;

    [Header("Damage and Point Val")]
    [SerializeField] private int _damage = 1;
    [SerializeField] private int _pointVal = 10;
    
    private UIManager _uiManager;
    private AudioManager _audioManager;
    private AudioSource _enemyLaserSound;
    private Player _player;
    private bool _movingLeft = false;
    private bool _movingRight = false;
    private bool _isHoming = false;
    
    private void Start()
    {
        _player = FindObjectOfType<Player>();
        if (_player == null) Debug.LogError("_player is NULL");

        _enemyLaserSound = GameObject.Find("EnemyLaserSound").GetComponent<AudioSource>();
        if (_enemyLaserSound == null) Debug.LogError("_enemyLaserSound is Null");

        _uiManager = FindObjectOfType<UIManager>();
        if (_uiManager == null) Debug.LogError("_uiManager is NULL");

        _audioManager = FindObjectOfType<AudioManager>();
        if (_audioManager == null) Debug.LogError("_audioManager is NULL");

        _speed = Random.Range(_speed / 1.25f, _speed * 1.25f);

        if (isBug) transform.Rotate(Vector3.forward, Random.Range(-30,30));

    }

    void FixedUpdate()
    {
        CalculateMovement();
    }

    private void OnCollisionEnter(Collision collision)
    {
        _colliders = transform.GetComponentsInChildren<Collider>();

        if (collision.gameObject.CompareTag("Projectile"))
        {
            _audioManager.PlayExplosion();
            Destroy(collision.gameObject);
            _explosion.Play();
            _mesh.enabled = false;
            foreach (Collider col in _colliders) col.enabled= false;
            Destroy(gameObject, 3f);
            _uiManager.AddScore(_pointVal);
            _canBeTargeted = false;
        }

        if (collision.gameObject.CompareTag("Asteroid"))
        {
            _explosion.Play();
            _mesh.enabled = false;
            foreach (Collider col in _colliders) col.enabled = false;
            Destroy(gameObject, 3f);
            _canBeTargeted = false;
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            _audioManager.PlayExplosion();
            collision.transform.GetComponent<Player>().Damage(_damage);
            _explosion.Play();
            _mesh.enabled = false;
            foreach (Collider col in _colliders) col.enabled = false;
            Destroy(gameObject, 3f);
            _canBeTargeted = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Projectile")
        {
            if (other.transform.position.x - transform.position.x > 0) _movingLeft = true;
            if (other.transform.position.x - transform.position.x < 0) _movingRight = true;
        }

        if (other.tag == "Player")
        {
            if (other.transform.position.x - transform.position.x > 0) _movingRight = true;
            if (other.transform.position.x - transform.position.x < 0) _movingLeft = true;
            if (isFighter) Shoot();
        }

        if (other.tag == "Asteroid")
        {
            if (other.transform.position.x - transform.position.x > 0) _movingLeft = true;
            if (other.transform.position.x - transform.position.x < 0) _movingRight = true;
            if (isFighter) Shoot();
        }

    }

    private void OnTriggerExit(Collider other)
    {
        _isHoming = false;
        StartCoroutine(StopDodging());
    }

    IEnumerator StopDodging()
    {
        yield return new WaitForSeconds(0.25f);
        _movingRight = false;
        _movingLeft = false;
    }

    void CalculateMovement()
    {
        Vector3 direction = Vector3.down;
        Vector3 left = new Vector3(-_turnSpeed, -1, 0);
        Vector3 right = new Vector3(_turnSpeed, -1, 0);
        Vector3 position = transform.position;


        //Bound to zero on Z axis. Not currently working for some reason.
        if (position.z > 0 || position.z < 0) position.z = 0;

        if (_isHoming && transform.position.y > _player.transform.position.y)
        { 
            Vector3 diff = _player.transform.position - transform.position;
            diff.Normalize();
            float rot_Z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
            Vector3 targetRot = new Vector3(0, 0, rot_Z + 90);
            transform.rotation = Quaternion.Euler(Vector3.Slerp(transform.rotation.eulerAngles, targetRot, 1f));
        }

        if (_isHoming && transform.position.y < _player.transform.position.y)
        {
            _isHoming = false;
            Destroy(_homingTrigger);
        } 

        if (position.y < -40 || position.x < -50 || position.x > 50) Destroy(gameObject);

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
            transform.Translate(direction * _speed * Time.deltaTime);
            _anim.SetBool("MovingLeft", false);
            _anim.SetBool("MovingRight", false);
        }
    }

    void Shoot()
    {
        if (isFighter && Time.time > _canFire)
        {
            _enemyLaserSound.Play();
            _fireRate = Random.Range(_fireRate / 2f, _fireRate * 2f);
            _canFire = Time.time + _fireRate;
            Instantiate(_enemyLaserPrefab, transform.position + new Vector3(0, -1.5f, 0), transform.rotation);
        }
    }

    public bool IsBug()
    {
        return isBug;
    }

    public bool IsFighter()
    {
        return isFighter;
    }

    public void StartHoming()
    { 
        _isHoming = true;    
    }

    public bool IsTargetable()
    {
        return _canBeTargeted;
    }
}

