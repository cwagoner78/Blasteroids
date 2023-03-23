using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    [Header("Object Assignments")]
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private GameObject _tripleShotPrefab;
    [SerializeField] private GameObject _muzzleFlash;
    [SerializeField] private GameObject _LeftWingFlash;
    [SerializeField] private GameObject _RightWingFlash;
    [SerializeField] private GameObject _laserContainer;
    [SerializeField] private GameObject _tripleShotContainer;
    private AudioSource _laserSound;
    private AudioSource _tripleShotSound;
    private GameManager _gameManager;

    [Header("Cool Downs")]    
    [SerializeField] private float _bulletWaitTime = 0.25f;

    [Header("Flags")]
    [SerializeField] private bool _canShoot = true;
    public bool hasTripleShot = false;

    private void Start()
    {
        _laserSound = GameObject.Find("LaserSound").GetComponent<AudioSource>();
        if (_laserSound == null) Debug.LogError("_laserSound is Null");

        _tripleShotSound = GameObject.Find("TripleShotSound").GetComponent<AudioSource>();
        if (_tripleShotSound == null) Debug.LogError("_tripleShotSound is Null");

        _gameManager = FindObjectOfType<GameManager>();
        if (_gameManager == null) Debug.LogError("_gameManager is Null");
    }

    void Update()
    {
        if (!_gameManager.gamePaused && _canShoot && Input.GetButtonDown("Fire1")) Shoot();
    }

    public void Shoot()
    {
        if (!hasTripleShot)
        {
            _laserSound.Play();
            _muzzleFlash.GetComponent<ParticleSystem>().Play();
            Instantiate(_laserPrefab, transform.position + new Vector3(0,1.5f,0), Quaternion.identity);
            
        }
        else 
        {
            _tripleShotSound.Play();
            _muzzleFlash.GetComponent<ParticleSystem>().Play();
            _LeftWingFlash.GetComponent<ParticleSystem>().Play();
            _RightWingFlash.GetComponent<ParticleSystem>().Play();
            Instantiate(_tripleShotPrefab, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
        }
        _canShoot = false;
        StartCoroutine(BulletWaitTimer());
    }

    IEnumerator BulletWaitTimer()
    { 
        yield return new WaitForSeconds(_bulletWaitTime);
        _canShoot = true;
    }

    public void TripleShotActive()
    {
        hasTripleShot = true;
    }

    IEnumerator PowerUpTimer(float timer)
    {
        yield return new WaitForSeconds(timer);
        hasTripleShot = false;
    }

    public void DisableTripleShot()
    {
        hasTripleShot = false;
    }
}
