using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Rigidbody rb;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private GameObject _muzzleFlash;
    
    [SerializeField] private float _bulletForce = 20f;
    public float bulletWaitTime = 0.1f;

    public bool isShooting = false;
    public bool canShoot = true;


    private void Start()
    {

    }

    void Update()
    {
        if (canShoot && Input.GetButton("Fire1"))
        {
            Shoot();
        }
    }

    public void Shoot()
    {
        isShooting = true;
        GameObject bullet = Instantiate(_bulletPrefab, _firePoint.position, Quaternion.identity);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.AddForce(_firePoint.forward * _bulletForce, ForceMode.Impulse);
        _muzzleFlash.GetComponent<ParticleSystem>().Play();
        canShoot = false;
        StartCoroutine(BulletWaitTimer());
    }

    IEnumerator BulletWaitTimer()
    { 
        yield return new WaitForSeconds(bulletWaitTime);
        canShoot = true;
        isShooting = false;
    }
}
