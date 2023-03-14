using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed = 1f;
    [SerializeField] private float _yPosBound = 15f;
    [SerializeField] private Collider _collider;
    [SerializeField] private ParticleSystem _explosion;
    [SerializeField] private MeshRenderer _mesh;
    public int damage = 1;

    void Update()
    {
        transform.Translate(Vector3.forward * Random.Range(_speed / 1.5f, _speed * 1.5f)  * Time.deltaTime) ;
        if (transform.position.y < -_yPosBound + 10f) Destroy(gameObject); 
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            Destroy(collision.gameObject);
            _explosion.Play();
            _mesh.enabled = false;
            _collider.enabled = false;
            Destroy(gameObject, 5f);
        }

        if (collision.gameObject.CompareTag("Asteroid"))
        {
            _explosion.Play();
            _mesh.enabled = false;
            _collider.enabled = false;
            Destroy(gameObject, 5f);
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.GetComponent<Player>().Damage(damage);
            _explosion.Play();
            _mesh.enabled = false;
            _collider.enabled = false;
            Destroy(gameObject, 5f);
        }
    }
}

