using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private int _damage = 1;
    private Rigidbody _rb;
    [SerializeField] private float _speed = 20f;
    private Player _player;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        if (_rb == null) Debug.LogError("_rb is NULL");

        _player = FindObjectOfType<Player>();
        //if (_player == null) Debug.LogError("_player is NULL");
        
        _rb.AddForce(Vector3.up * _speed, ForceMode.Impulse);

    }

    private void Update()
    {
        if (transform.position.y > 25 || transform.position.y < -25)
        {
            if (transform.parent != null) Destroy(transform.parent.gameObject);
            Destroy(gameObject);
        }


    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (_player != null) _player.Damage(_damage);
            Destroy(gameObject);
        }
    }

}
