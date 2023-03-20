using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public int damage = 1;
    private Rigidbody _rb;
    private Transform _firePoint;
    [SerializeField] private float _speed = 20f;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        if (_rb == null) Debug.LogError("_rb is NULL");

        _firePoint = GameObject.Find("FirePoint").transform;
        _rb.AddForce(_firePoint.forward * _speed, ForceMode.Impulse);
    }

    private void Update()
    {
        if (transform.position.y > 25)
        {
            if (transform.parent != null) Destroy(transform.parent.gameObject);
            Destroy(gameObject);
        } 
    }

}
