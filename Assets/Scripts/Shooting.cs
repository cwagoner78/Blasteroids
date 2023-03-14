using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    //public Rigidbody rb;
    [Header("Object Assignments")]
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private GameObject _tripleShotPrefab;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private GameObject _muzzleFlash;
    [SerializeField] private GameObject _LeftWingFlash;
    [SerializeField] private GameObject _RightWingFlash;
    
    [Header("Cool Downs")]    
    [SerializeField] private float _bulletWaitTime = 0.25f;

    [Header("Flags")]
    [SerializeField] private bool _canShoot = true;
    public bool hasTripleShot = false;


    void Update()
    {
        if (_canShoot && Input.GetButton("Fire1")) Shoot();
    }

    public void Shoot()
    {
        if (!hasTripleShot)
        {
            _muzzleFlash.GetComponent<ParticleSystem>().Play();
            Instantiate(_laserPrefab, _firePoint.position, Quaternion.identity);
        }
        else 
        {
            _muzzleFlash.GetComponent<ParticleSystem>().Play();
            _LeftWingFlash.GetComponent<ParticleSystem>().Play();
            _RightWingFlash.GetComponent<ParticleSystem>().Play();
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
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
