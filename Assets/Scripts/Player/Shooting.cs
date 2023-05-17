using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    [Header("Object Assignments")]
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private GameObject _tripleShotPrefab;
    [SerializeField] private GameObject _homingMissilePrefab;
    [SerializeField] private GameObject _nukePrefab;
    [SerializeField] private GameObject _muzzleFlash;
    [SerializeField] private GameObject _missileFlashL;
    [SerializeField] private GameObject _missileFlashR;
    [SerializeField] private GameObject _LeftWingFlash;
    [SerializeField] private GameObject _RightWingFlash;
    [SerializeField] private GameObject _laserContainer;
    [SerializeField] private GameObject _tripleShotContainer;
    private AudioSource _laserSound;
    private AudioSource _tripleShotSound;
    private AudioSource _homingMissileSound;
    private AudioSource _noAmmoSound;
    private GameManager _gameManager;
    private UIManager _uiManager;

    [Header("Cool Downs")]    
    [SerializeField] private float _bulletWaitTime = 0.25f;

    [Header("Flags")]
    [SerializeField] private int _maxAmmo = 15;
    [SerializeField] private bool _canShoot = true;
    
    private bool _hasNuke = false;
    private bool _hasTripleShot = false;    
    private int _missileCount;


    private void Start()
    {
        _laserSound = GameObject.Find("LaserSound").GetComponent<AudioSource>();
        if (_laserSound == null) Debug.LogError("_laserSound is Null");

        _tripleShotSound = GameObject.Find("TripleShotSound").GetComponent<AudioSource>();
        if (_tripleShotSound == null) Debug.LogError("_tripleShotSound is Null");

        _homingMissileSound = GameObject.Find("HomingMissileSound").GetComponent<AudioSource>();
        if (_homingMissileSound == null) Debug.LogError("_homingMissileSound is Null");

        _noAmmoSound = GameObject.Find("NoAmmoSound").GetComponent<AudioSource>();
        if (_noAmmoSound == null) Debug.LogError("_noAmmoSound is Null");

        _gameManager = FindObjectOfType<GameManager>();
        if (_gameManager == null) Debug.LogError("_gameManager is Null");

        _uiManager = FindObjectOfType<UIManager>();
        if (_uiManager == null) Debug.LogError("_uiManager is Null");

        _missileCount = _maxAmmo;
        _uiManager.UpdateAmmoCount(_missileCount);
    }

    void Update()
    {
        if (!_gameManager.gamePaused && _canShoot && Input.GetButtonDown("Fire1")) Shoot();
        if (!_gameManager.gamePaused && _canShoot && Input.GetButtonDown("Fire2")) FireMissiles();
        if (!_gameManager.gamePaused && _hasNuke && Input.GetKeyDown(KeyCode.Space)) FireNuke();
    }

    public void Shoot()
    {
        if (!_hasTripleShot)
        {
            _laserSound.Play();
            _muzzleFlash.GetComponent<ParticleSystem>().Play();
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.5f, 0), Quaternion.identity);

        }
        else if (_hasTripleShot)
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

    public void NukeGained()
    {
        _hasNuke = true;
        _uiManager.EnableNukeIcon();
        _uiManager.UpdateHudText("Nuke Gained!");
    }

    void FireNuke()
    {
        GameObject nuke = Instantiate(_nukePrefab, transform.position, Quaternion.identity);
        _hasNuke = false;
        _uiManager.DisableNukeIcon();
        Destroy(nuke, 10);
    }

    void FireMissiles()
    {
        if (_missileCount == 0)
        {
            _noAmmoSound.Play();
            return;
        }

        _homingMissileSound.Play();
        _missileFlashL.GetComponent<ParticleSystem>().Play();
        _missileFlashR.GetComponent<ParticleSystem>().Play();
        Instantiate(_homingMissilePrefab, transform.position + new Vector3(0, 0, 0), Quaternion.identity);

        _missileCount -= 2;
        _uiManager.UpdateAmmoCount(_missileCount);
    }

    public void AmmoGained()
    {
        _missileCount = _maxAmmo;
        _uiManager.UpdateAmmoCount(_missileCount);
        _uiManager.UpdateHudText("Ammo Gained!");
    }

    IEnumerator BulletWaitTimer()
    { 
        yield return new WaitForSeconds(_bulletWaitTime);
        _canShoot = true;
    }

    public void TripleShotActive()
    {
        _hasTripleShot = true;
    }

    public void DisableTripleShot()
    {
        _hasTripleShot = false;
    }

    public bool HasNuke()
    { 
        return _hasNuke;
    }

    public bool HasTripleShot()
    {
        return _hasTripleShot;
    }

    public int GetMissleCount()
    {
        return _missileCount;
    }

}
