using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    [Header("Object Assignments")]
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private GameObject _tripleShotPrefab;
    [SerializeField] private GameObject _nukePrefab;
    [SerializeField] private GameObject _muzzleFlash;
    [SerializeField] private GameObject _LeftWingFlash;
    [SerializeField] private GameObject _RightWingFlash;
    [SerializeField] private GameObject _laserContainer;
    [SerializeField] private GameObject _tripleShotContainer;
    private AudioSource _laserSound;
    private AudioSource _tripleShotSound;
    private AudioSource _noAmmoSound;
    private GameManager _gameManager;
    private UIManager _uiManager;

    [Header("Cool Downs")]    
    [SerializeField] private float _bulletWaitTime = 0.25f;

    [Header("Flags")]
    [SerializeField] private int _maxAmmo = 15;
    [SerializeField] private bool _canShoot = true;
    public bool hasNuke = false;
    public bool hasTripleShot = false;
    public int ammoCount;

    private void Start()
    {
        _laserSound = GameObject.Find("LaserSound").GetComponent<AudioSource>();
        if (_laserSound == null) Debug.LogError("_laserSound is Null");

        _tripleShotSound = GameObject.Find("TripleShotSound").GetComponent<AudioSource>();
        if (_tripleShotSound == null) Debug.LogError("_tripleShotSound is Null");

        _noAmmoSound = GameObject.Find("NoAmmoSound").GetComponent<AudioSource>();
        if (_noAmmoSound == null) Debug.LogError("_noAmmoSound is Null");

        _gameManager = FindObjectOfType<GameManager>();
        if (_gameManager == null) Debug.LogError("_gameManager is Null");

        _uiManager = FindObjectOfType<UIManager>();
        if (_uiManager == null) Debug.LogError("_uiManager is Null");

        ammoCount = _maxAmmo;
        _uiManager.UpdateAmmoCount(ammoCount);
    }

    void Update()
    {
        if (!_gameManager.gamePaused && _canShoot && Input.GetButtonDown("Fire1")) Shoot();
        if (!_gameManager.gamePaused && hasNuke && Input.GetButtonDown("Fire2")) FireNuke();
    }

    public void Shoot()
    {
        if (ammoCount == 0)
        {
            _noAmmoSound.Play();
            return;
        } 

        if (!hasTripleShot)
        {
            _laserSound.Play();
            _muzzleFlash.GetComponent<ParticleSystem>().Play();
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.5f, 0), Quaternion.identity);
            ammoCount--;
            _uiManager.UpdateAmmoCount(ammoCount);
        }
        else
        {
            _tripleShotSound.Play();
            _muzzleFlash.GetComponent<ParticleSystem>().Play();
            _LeftWingFlash.GetComponent<ParticleSystem>().Play();
            _RightWingFlash.GetComponent<ParticleSystem>().Play();
            Instantiate(_tripleShotPrefab, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
            ammoCount--;
            _uiManager.UpdateAmmoCount(ammoCount);
        }
        _canShoot = false;
        StartCoroutine(BulletWaitTimer());
    }

    public void NukeGained()
    {
        hasNuke = true;
        _uiManager.EnableNukeIcon();
        _uiManager.UpdateHudText("Nuke Gained!");
    }

    void FireNuke()
    {
        GameObject nuke = Instantiate(_nukePrefab, transform.position, Quaternion.identity);
        hasNuke = false;
        _uiManager.DisableNukeIcon();
        Destroy(nuke, 10);
    }

    public void AmmoGained()
    {
        ammoCount = _maxAmmo;
        _uiManager.UpdateAmmoCount(ammoCount);
        _uiManager.UpdateHudText("Ammo Gained!");
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

    public void DisableTripleShot()
    {
        hasTripleShot = false;
    }
}
